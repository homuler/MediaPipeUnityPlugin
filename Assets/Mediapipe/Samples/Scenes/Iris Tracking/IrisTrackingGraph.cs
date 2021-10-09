// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.IrisTracking
{
  public class IrisTrackingGraph : GraphRunner
  {
#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public UnityEvent<List<Detection>> OnFaceDetectionsOutput = new UnityEvent<List<Detection>>();
    public UnityEvent<NormalizedRect> OnFaceRectOutput = new UnityEvent<NormalizedRect>();
    public UnityEvent<NormalizedLandmarkList> OnFaceLandmarksWithIrisOutput = new UnityEvent<NormalizedLandmarkList>();
#pragma warning restore IDE1006

    private const string _InputStreamName = "input_video";

    private const string _FaceDetectionsStreamName = "face_detections";
    private const string _FaceRectStreamName = "face_rect";
    private const string _FaceLandmarksWithIrisStreamName = "face_landmarks_with_iris";

    private OutputStream<DetectionVectorPacket, List<Detection>> _faceDetectionsStream;
    private OutputStream<NormalizedRectPacket, NormalizedRect> _faceRectStream;
    private OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList> _faceLandmarksWithIrisStream;

    protected long prevFaceDetectionsMicrosec = 0;
    protected long prevFaceRectMicrosec = 0;
    protected long prevFaceLandmarksWithIrisMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _faceDetectionsStream.StartPolling(true).AssertOk();
      _faceRectStream.StartPolling(true).AssertOk();
      _faceLandmarksWithIrisStream.StartPolling(true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _faceDetectionsStream.AddListener(FaceDetectionsCallback, true).AssertOk();
      _faceRectStream.AddListener(FaceRectCallback, true).AssertOk();
      _faceLandmarksWithIrisStream.AddListener(FaceLandmarksWithIrisCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnFaceDetectionsOutput.RemoveAllListeners();
      OnFaceRectOutput.RemoveAllListeners();
      OnFaceLandmarksWithIrisOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      return AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public IrisTrackingValue FetchNextValue()
    {
      var _ = _faceDetectionsStream.TryGetNext(out var faceDetections);
      _ = _faceRectStream.TryGetNext(out var faceRect);
      _ = _faceLandmarksWithIrisStream.TryGetNext(out var faceLandmarksWithIris);

      OnFaceDetectionsOutput.Invoke(faceDetections);
      OnFaceRectOutput.Invoke(faceRect);
      OnFaceLandmarksWithIrisOutput.Invoke(faceLandmarksWithIris);

      return new IrisTrackingValue(faceDetections, faceRect, faceLandmarksWithIris);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr FaceDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<IrisTrackingGraph>(graphPtr, packetPtr, (irisTrackingGraph, ptr) =>
      {
        using (var packet = new DetectionVectorPacket(ptr, false))
        {
          if (irisTrackingGraph.TryGetPacketValue(packet, ref irisTrackingGraph.prevFaceDetectionsMicrosec, out var value))
          {
            irisTrackingGraph.OnFaceDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr FaceRectCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<IrisTrackingGraph>(graphPtr, packetPtr, (irisTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedRectPacket(ptr, false))
        {
          if (irisTrackingGraph.TryGetPacketValue(packet, ref irisTrackingGraph.prevFaceRectMicrosec, out var value))
          {
            irisTrackingGraph.OnFaceRectOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr FaceLandmarksWithIrisCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<IrisTrackingGraph>(graphPtr, packetPtr, (irisTrackingGraph, ptr) =>
      {
        using (var packet = new NormalizedLandmarkListPacket(ptr, false))
        {
          if (irisTrackingGraph.TryGetPacketValue(packet, ref irisTrackingGraph.prevFaceLandmarksWithIrisMicrosec, out var value))
          {
            irisTrackingGraph.OnFaceLandmarksWithIrisOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset("face_landmark.bytes"),
        WaitForAsset("iris_landmark.bytes"),
      };
    }

    protected void InitializeOutputStreams()
    {
      _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _FaceDetectionsStreamName);
      _faceRectStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, _FaceRectStreamName);
      _faceLandmarksWithIrisStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _FaceLandmarksWithIrisStreamName);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
