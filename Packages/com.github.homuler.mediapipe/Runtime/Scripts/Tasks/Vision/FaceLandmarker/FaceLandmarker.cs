// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;

namespace Mediapipe.Tasks.Vision.FaceLandmarker
{
  public sealed class FaceLandmarker : Core.BaseVisionTaskApi
  {
    private const string _IMAGE_IN_STREAM_NAME = "image_in";
    private const string _IMAGE_OUT_STREAM_NAME = "image_out";
    private const string _IMAGE_TAG = "IMAGE";
    private const string _NORM_RECT_STREAM_NAME = "norm_rect_in";
    private const string _NORM_RECT_TAG = "NORM_RECT";
    private const string _NORM_LANDMARKS_STREAM_NAME = "norm_landmarks";
    private const string _NORM_LANDMARKS_TAG = "NORM_LANDMARKS";
    private const string _BLENDSHAPES_STREAM_NAME = "blendshapes";
    private const string _BLENDSHAPES_TAG = "BLENDSHAPES";
    private const string _FACE_GEOMETRY_STREAM_NAME = "face_geometry";
    private const string _FACE_GEOMETRY_TAG = "FACE_GEOMETRY";
    private const string _TASK_GRAPH_NAME = "mediapipe.tasks.vision.face_landmarker.FaceLandmarkerGraph";

    private const int _MICRO_SECONDS_PER_MILLISECOND = 1000;

#pragma warning disable IDE0052 // Remove unread private members
    /// <remarks>
    ///   keep reference to prevent GC from collecting the callback instance.
    /// </remarks>
    private readonly Tasks.Core.TaskRunner.PacketsCallback _packetCallback;
#pragma warning restore IDE0052

    private readonly NormalizedRect _normalizedRect = new NormalizedRect();
    private readonly List<FaceGeometry.Proto.FaceGeometry> _faceGeometriesForRead;

    private FaceLandmarker(
      CalculatorGraphConfig graphConfig,
      Core.RunningMode runningMode,
      GpuResources gpuResources,
      List<FaceGeometry.Proto.FaceGeometry> faceGeometriesForRead,
      Tasks.Core.TaskRunner.PacketsCallback packetCallback) : base(graphConfig, runningMode, gpuResources, packetCallback)
    {
      _faceGeometriesForRead = faceGeometriesForRead;
      _packetCallback = packetCallback;
    }

    /// <summary>
    ///   Creates an <see cref="FaceLandmarker" /> object from a TensorFlow Lite model and the default <see cref="FaceLandmarkerOptions" />.
    ///
    ///   Note that the created <see cref="FaceLandmarker" /> instance is in image mode,
    ///   for detecting face landmarks on single image inputs.
    /// </summary>
    /// <param name="modelPath">Path to the model.</param>
    /// <param name="gpuResources">
    ///   <see cref="GpuResources"/> to set to the underlying <see cref="CalculatorGraph"/>.
    ///   To share the GL context with MediaPipe, <see cref="GlCalculatorHelper.InitializeForTest"/> must be called with it.
    /// </param>
    /// <returns>
    ///   <see cref="FaceLandmarker" /> object that's created from the model and the default <see cref="FaceLandmarkerOptions" />.
    /// </returns>
    public static FaceLandmarker CreateFromModelPath(string modelPath, GpuResources gpuResources = null)
    {
      var baseOptions = new Tasks.Core.BaseOptions(modelAssetPath: modelPath);
      var options = new FaceLandmarkerOptions(baseOptions, runningMode: Core.RunningMode.IMAGE);
      return CreateFromOptions(options, gpuResources);
    }

    /// <summary>
    ///   Creates the <see cref="FaceLandmarker" /> object from <paramref name="FaceLandmarkerOptions" />.
    /// </summary>
    /// <param name="options">Options for the face landmarker task.</param>
    /// <param name="gpuResources">
    ///   <see cref="GpuResources"/> to set to the underlying <see cref="CalculatorGraph"/>.
    ///   To share the GL context with MediaPipe, <see cref="GlCalculatorHelper.InitializeForTest"/> must be called with it.
    /// </param>
    /// <returns>
    ///   <see cref="FaceLandmarker" /> object that's created from <paramref name="options" />.
    /// </returns>
    public static FaceLandmarker CreateFromOptions(FaceLandmarkerOptions options, GpuResources gpuResources = null)
    {
      var outputStreams = new List<string> {
        string.Join(":", _NORM_LANDMARKS_TAG, _NORM_LANDMARKS_STREAM_NAME),
        string.Join(":", _IMAGE_TAG, _IMAGE_OUT_STREAM_NAME),
      };
      if (options.outputFaceBlendshapes)
      {
        outputStreams.Add(string.Join(":", _BLENDSHAPES_TAG, _BLENDSHAPES_STREAM_NAME));
      }
      if (options.outputFaceTransformationMatrixes)
      {
        outputStreams.Add(string.Join(":", _FACE_GEOMETRY_TAG, _FACE_GEOMETRY_STREAM_NAME));
      }
      var taskInfo = new Tasks.Core.TaskInfo<FaceLandmarkerOptions>(
        taskGraph: _TASK_GRAPH_NAME,
        inputStreams: new List<string> {
          string.Join(":", _IMAGE_TAG, _IMAGE_IN_STREAM_NAME),
          string.Join(":", _NORM_RECT_TAG, _NORM_RECT_STREAM_NAME),
        },
        outputStreams: outputStreams,
        taskOptions: options);

      var faceGeometriesForRead = options.outputFaceTransformationMatrixes ? new List<FaceGeometry.Proto.FaceGeometry>(options.numFaces) : null;
      return new FaceLandmarker(
        taskInfo.GenerateGraphConfig(options.runningMode == Core.RunningMode.LIVE_STREAM),
        options.runningMode,
        gpuResources,
        faceGeometriesForRead,
        BuildPacketsCallback(options, faceGeometriesForRead));
    }

    /// <summary>
    ///   Performs face landmarks detection on the provided MediaPipe Image.
    ///
    ///   Only use this method when the <see cref="FaceLandmarker" /> is created with the image running mode.
    ///   The image can be of any size with format RGB or RGBA.
    /// </summary>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="imageProcessingOptions">Options for image processing.</param>
    /// <returns>
    ///   The face landmarks detection results.
    /// </returns>
    public FaceLandmarkerResult Detect(Image image, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      using var outputPackets = DetectInternal(image, imageProcessingOptions);

      var result = default(FaceLandmarkerResult);
      _ = TryBuildFaceLandmarkerResult(outputPackets, ref result);
      return result;
    }

    /// <summary>
    ///   Performs face landmarks detection on the provided MediaPipe Image.
    ///
    ///   Only use this method when the <see cref="FaceLandmarker" /> is created with the image running mode.
    ///   The image can be of any size with format RGB or RGBA.
    /// </summary>
    /// <remarks>
    ///   When faces are not found, <paramref name="result"/> won't be overwritten.
    /// </remarks>
    /// <param name="image">MediaPipe Image.</param>
    /// <param name="imageProcessingOptions">Options for image processing.</param>
    /// <param name="result">
    ///   <see cref="FaceLandmarkerResult"/> to which the result will be written.
    /// </param>
    /// <returns>
    ///   <see langword="true"/> if some faces are detected, <see langword="false"/> otherwise.
    /// </returns>
    public bool TryDetect(Image image, Core.ImageProcessingOptions? imageProcessingOptions, ref FaceLandmarkerResult result)
    {
      using var outputPackets = DetectInternal(image, imageProcessingOptions);
      return TryBuildFaceLandmarkerResult(outputPackets, ref result);
    }

    private PacketMap DetectInternal(Image image, Core.ImageProcessingOptions? imageProcessingOptions)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImage(image));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, Packet.CreateProto(_normalizedRect));

      return ProcessImageData(packetMap);
    }

    /// <summary>
    ///   Performs face landmarks detection on the provided video frames.
    ///
    ///   Only use this method when the FaceLandmarker is created with the video
    ///   running mode. It's required to provide the video frame's timestamp (in
    ///   milliseconds) along with the video frame. The input timestamps should be
    ///   monotonically increasing for adjacent calls of this method.
    /// </summary>
    /// <returns>
    ///   The face landmarks detection results.
    /// </returns>
    public FaceLandmarkerResult DetectForVideo(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      using var outputPackets = DetectForVideoInternal(image, timestampMillisec, imageProcessingOptions);

      var result = default(FaceLandmarkerResult);
      _ = TryBuildFaceLandmarkerResult(outputPackets, ref result);
      return result;
    }

    /// <summary>
    ///   Performs face landmarks detection on the provided video frames.
    ///
    ///   Only use this method when the FaceLandmarker is created with the video
    ///   running mode. It's required to provide the video frame's timestamp (in
    ///   milliseconds) along with the video frame. The input timestamps should be
    ///   monotonically increasing for adjacent calls of this method.
    /// </summary>
    /// <remarks>
    ///   When faces are not found, <paramref name="result"/> won't be overwritten.
    /// </remarks>
    /// <param name="result">
    ///   <see cref="FaceLandmarkerResult"/> to which the result will be written.
    /// </param>
    /// <returns>
    ///   <see langword="true"/> if some faces are detected, <see langword="false"/> otherwise.
    /// </returns>
    public bool TryDetectForVideo(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions, ref FaceLandmarkerResult result)
    {
      using var outputPackets = DetectForVideoInternal(image, timestampMillisec, imageProcessingOptions);
      return TryBuildFaceLandmarkerResult(outputPackets, ref result);
    }

    private PacketMap DetectForVideoInternal(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);
      var timestampMicrosec = timestampMillisec * _MICRO_SECONDS_PER_MILLISECOND;

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImageAt(image, timestampMicrosec));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, Packet.CreateProtoAt(_normalizedRect, timestampMicrosec));

      return ProcessVideoData(packetMap);
    }

    /// <summary>
    ///   Sends live image data to perform face landmarks detection.
    ///
    ///   Only use this method when the FaceLandmarker is created with the live stream
    ///   running mode. The input timestamps should be monotonically increasing for
    ///   adjacent calls of this method. This method will return immediately after the
    ///   input image is accepted. The results will be available via the
    ///   <see cref="FaceLandmarkerOptions.ResultCallback" /> provided in the <see cref="FaceLandmarkerOptions" />.
    ///   The <see cref="DetectAsync" /> method is designed to process live stream data such as camera
    ///   input. To lower the overall latency, face landmarker may drop the input
    ///   images if needed. In other words, it's not guaranteed to have output per
    ///   input image.
    /// </summary>
    public void DetectAsync(Image image, long timestampMillisec, Core.ImageProcessingOptions? imageProcessingOptions = null)
    {
      ConfigureNormalizedRect(_normalizedRect, imageProcessingOptions, image, roiAllowed: false);
      var timestampMicrosec = timestampMillisec * _MICRO_SECONDS_PER_MILLISECOND;

      var packetMap = new PacketMap();
      packetMap.Emplace(_IMAGE_IN_STREAM_NAME, Packet.CreateImageAt(image, timestampMicrosec));
      packetMap.Emplace(_NORM_RECT_STREAM_NAME, Packet.CreateProtoAt(_normalizedRect, timestampMicrosec));

      SendLiveStreamData(packetMap);
    }

    private bool TryBuildFaceLandmarkerResult(PacketMap outputPackets, ref FaceLandmarkerResult result)
        => TryBuildFaceLandmarkerResult(outputPackets, _faceGeometriesForRead, ref result);

    private static Tasks.Core.TaskRunner.PacketsCallback BuildPacketsCallback(FaceLandmarkerOptions options,
        List<FaceGeometry.Proto.FaceGeometry> faceGeometriesForRead)
    {
      var resultCallback = options.resultCallback;
      if (resultCallback == null)
      {
        return null;
      }

      var lockObj = new object();
      var faceLandmarkerResult = FaceLandmarkerResult.Alloc(options.numFaces, options.outputFaceBlendshapes, options.outputFaceTransformationMatrixes);

      return (PacketMap outputPackets) =>
      {
        using var outImagePacket = outputPackets.At<Image>(_IMAGE_OUT_STREAM_NAME);
        if (outImagePacket == null || outImagePacket.IsEmpty())
        {
          return;
        }

        using var image = outImagePacket.Get();
        var timestamp = outImagePacket.TimestampMicroseconds() / _MICRO_SECONDS_PER_MILLISECOND;

        lock (lockObj)
        {
          if (TryBuildFaceLandmarkerResult(outputPackets, faceGeometriesForRead, ref faceLandmarkerResult))
          {
            resultCallback(faceLandmarkerResult, image, timestamp);
          }
          else
          {
            resultCallback(default, image, timestamp);
          }
        }
      };
    }

    private static void GetFaceGeometryList(Packet<List<FaceGeometry.Proto.FaceGeometry>> packet, List<FaceGeometry.Proto.FaceGeometry> outs)
    {
      foreach (var geometry in outs)
      {
        geometry.Clear();
      }

      var size = packet.WriteTo(FaceGeometry.Proto.FaceGeometry.Parser, outs);
      outs.RemoveRange(size, outs.Count - size);
    }

    private static bool TryBuildFaceLandmarkerResult(PacketMap outputPackets,
        List<FaceGeometry.Proto.FaceGeometry> faceGeometriesForRead,
        ref FaceLandmarkerResult result)
    {
      using var faceLandmarksPacket = outputPackets.At<List<NormalizedLandmarks>>(_NORM_LANDMARKS_STREAM_NAME);
      if (faceLandmarksPacket.IsEmpty())
      {
        return false;
      }

      var faceLandmarks = result.faceLandmarks ?? new List<NormalizedLandmarks>();
      faceLandmarksPacket.Get(faceLandmarks);

      var faceBlendshapesList = result.faceBlendshapes;
      using var faceBlendshapesPacket = outputPackets.At<List<Classifications>>(_BLENDSHAPES_STREAM_NAME);
      if (faceBlendshapesPacket != null)
      {
        faceBlendshapesList ??= new List<Classifications>();
        faceBlendshapesPacket.Get(faceBlendshapesList);
      }

      var faceTransformationMatrixes = result.facialTransformationMatrixes;
      using var faceTransformationMatrixesPacket = outputPackets.At<List<FaceGeometry.Proto.FaceGeometry>>(_FACE_GEOMETRY_STREAM_NAME);
      if (faceTransformationMatrixesPacket != null)
      {
        GetFaceGeometryList(faceTransformationMatrixesPacket, faceGeometriesForRead);
        faceTransformationMatrixes ??= new List<UnityEngine.Matrix4x4>(faceGeometriesForRead.Count);

        faceTransformationMatrixes.Clear();
        foreach (var faceGeometry in faceGeometriesForRead)
        {
          faceTransformationMatrixes.Add(faceGeometry.PoseTransformMatrix.ToMatrix4x4());
        }
      }

      result = new FaceLandmarkerResult(faceLandmarks, faceBlendshapesList, faceTransformationMatrixes);
      return true;
    }
  }
}
