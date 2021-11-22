// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.FaceMesh
{
  public class FaceMeshGraph : GraphRunner
  {
    public int maxNumFaces = 1;
    public bool refineLandmarks = true;
#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public UnityEvent<List<Detection>> OnFaceDetectionsOutput = new UnityEvent<List<Detection>>();
    public UnityEvent<List<NormalizedLandmarkList>> OnMultiFaceLandmarksOutput = new UnityEvent<List<NormalizedLandmarkList>>();
    public UnityEvent<List<NormalizedRect>> OnFaceRectsFromLandmarksOutput = new UnityEvent<List<NormalizedRect>>();
    public UnityEvent<List<NormalizedRect>> OnFaceRectsFromDetectionsOutput = new UnityEvent<List<NormalizedRect>>();
#pragma warning restore IDE1006

    private const string _InputStreamName = "input_video";

    private const string _FaceDetectionsStreamName = "face_detections";
    private const string _MultiFaceLandmarksStreamName = "multi_face_landmarks";
    private const string _FaceRectsFromLandmarksStreamName = "face_rects_from_landmarks";
    private const string _FaceRectsFromDetectionsStreamName = "face_rects_from_detections";

    private OutputStream<DetectionVectorPacket, List<Detection>> _faceDetectionsStream;
    private OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>> _multiFaceLandmarksStream;
    private OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>> _faceRectsFromLandmarksStream;
    private OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>> _faceRectsFromDetectionsStream;

    protected long prevFaceDetectionsMicrosec = 0;
    protected long prevMultiFaceLandmarksMicrosec = 0;
    protected long prevFaceRectsFromLandmarksMicrosec = 0;
    protected long prevFaceRectsFromDetectionsMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _faceDetectionsStream.StartPolling(true).AssertOk();
      _multiFaceLandmarksStream.StartPolling(true).AssertOk();
      _faceRectsFromLandmarksStream.StartPolling(true).AssertOk();
      _faceRectsFromDetectionsStream.StartPolling(true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _faceDetectionsStream.AddListener(FaceDetectionsCallback, true).AssertOk();
      _multiFaceLandmarksStream.AddListener(MultiFaceLandmarksCallback, true).AssertOk();
      _faceRectsFromLandmarksStream.AddListener(FaceRectsFromLandmarksCallback, true).AssertOk();
      _faceRectsFromDetectionsStream.AddListener(FaceRectsFromDetectionsCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnFaceDetectionsOutput.RemoveAllListeners();
      OnMultiFaceLandmarksOutput.RemoveAllListeners();
      OnFaceRectsFromLandmarksOutput.RemoveAllListeners();
      OnFaceRectsFromDetectionsOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      return AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public FaceMeshValue FetchNextValue()
    {
      var _ = _faceDetectionsStream.TryGetNext(out var faceDetections);
      _ = _multiFaceLandmarksStream.TryGetNext(out var multiFaceLandmarks);
      _ = _faceRectsFromLandmarksStream.TryGetNext(out var faceRectsFromLandmarks);
      _ = _faceRectsFromDetectionsStream.TryGetNext(out var faceRectsFromDetections);

      OnFaceDetectionsOutput.Invoke(faceDetections);
      OnMultiFaceLandmarksOutput.Invoke(multiFaceLandmarks);
      OnFaceRectsFromLandmarksOutput.Invoke(faceRectsFromLandmarks);
      OnFaceRectsFromDetectionsOutput.Invoke(faceRectsFromDetections);

      return new FaceMeshValue(faceDetections, multiFaceLandmarks, faceRectsFromLandmarks, faceRectsFromDetections);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr FaceDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<FaceMeshGraph>(graphPtr, packetPtr, (faceMeshGraph, ptr) =>
      {
        using (var packet = new DetectionVectorPacket(ptr, false))
        {
          if (faceMeshGraph.TryGetPacketValue(packet, ref faceMeshGraph.prevFaceDetectionsMicrosec, out var value))
          {
            faceMeshGraph.OnFaceDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr MultiFaceLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<FaceMeshGraph>(graphPtr, packetPtr, (faceMeshGraph, ptr) =>
      {
        using (var packet = new NormalizedLandmarkListVectorPacket(ptr, false))
        {
          if (faceMeshGraph.TryGetPacketValue(packet, ref faceMeshGraph.prevMultiFaceLandmarksMicrosec, out var value))
          {
            faceMeshGraph.OnMultiFaceLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr FaceRectsFromLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<FaceMeshGraph>(graphPtr, packetPtr, (faceMeshGraph, ptr) =>
      {
        using (var packet = new NormalizedRectVectorPacket(ptr, false))
        {
          if (faceMeshGraph.TryGetPacketValue(packet, ref faceMeshGraph.prevFaceRectsFromLandmarksMicrosec, out var value))
          {
            faceMeshGraph.OnFaceRectsFromLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr FaceRectsFromDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<FaceMeshGraph>(graphPtr, packetPtr, (faceMeshGraph, ptr) =>
      {
        using (var packet = new NormalizedRectVectorPacket(ptr, false))
        {
          if (faceMeshGraph.TryGetPacketValue(packet, ref faceMeshGraph.prevFaceRectsFromDetectionsMicrosec, out var value))
          {
            faceMeshGraph.OnFaceRectsFromDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected void InitializeOutputStreams()
    {
      _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _FaceDetectionsStreamName);
      _multiFaceLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _MultiFaceLandmarksStreamName);
      _faceRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _FaceRectsFromLandmarksStreamName);
      _faceRectsFromDetectionsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _FaceRectsFromDetectionsStreamName);
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset(refineLandmarks ? "face_landmark_with_attention.bytes" : "face_landmark.bytes"),
      };
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("num_faces", new IntPacket(maxNumFaces));
      sidePacket.Emplace("with_attention", new BoolPacket(refineLandmarks));

      return sidePacket;
    }
  }
}
