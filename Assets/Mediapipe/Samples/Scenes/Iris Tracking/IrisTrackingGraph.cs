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

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _faceDetectionsStream.StartPolling().AssertOk();
        _faceRectStream.StartPolling().AssertOk();
        _faceLandmarksWithIrisStream.StartPolling().AssertOk();
      }
      else
      {
        _faceDetectionsStream.AddListener(FaceDetectionsCallback).AssertOk();
        _faceRectStream.AddListener(FaceRectCallback).AssertOk();
        _faceLandmarksWithIrisStream.AddListener(FaceLandmarksWithIrisCallback).AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnFaceDetectionsOutput.RemoveAllListeners();
      OnFaceRectOutput.RemoveAllListeners();
      OnFaceLandmarksWithIrisOutput.RemoveAllListeners();
      _faceDetectionsStream = null;
      _faceRectStream = null;
      _faceLandmarksWithIrisStream = null;
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out List<Detection> faceDetections, out NormalizedRect faceRect, out NormalizedLandmarkList faceLandmarksWithIris, bool allowBlock = true)
    {
      var currentTimestampMicrosec = GetCurrentTimestampMicrosec();
      var r1 = TryGetNext(_faceDetectionsStream, out faceDetections, allowBlock, currentTimestampMicrosec);
      var r2 = TryGetNext(_faceRectStream, out faceRect, allowBlock, currentTimestampMicrosec);
      var r3 = TryGetNext(_faceLandmarksWithIrisStream, out faceLandmarksWithIris, allowBlock, currentTimestampMicrosec);

      if (r1) { OnFaceDetectionsOutput.Invoke(faceDetections); }
      if (r2) { OnFaceRectOutput.Invoke(faceRect); }
      if (r3) { OnFaceLandmarksWithIrisOutput.Invoke(faceLandmarksWithIris); }

      return r1 || r2 || r3;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr FaceDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<IrisTrackingGraph>(graphPtr, packetPtr, (irisTrackingGraph, ptr) =>
      {
        using (var packet = new DetectionVectorPacket(ptr, false))
        {
          if (irisTrackingGraph._faceDetectionsStream.TryGetPacketValue(packet, out var value, irisTrackingGraph.timeoutMicrosec))
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
          if (irisTrackingGraph._faceRectStream.TryGetPacketValue(packet, out var value, irisTrackingGraph.timeoutMicrosec))
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
          if (irisTrackingGraph._faceLandmarksWithIrisStream.TryGetPacketValue(packet, out var value, irisTrackingGraph.timeoutMicrosec))
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

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _FaceDetectionsStreamName, config.AddPacketPresenceCalculator(_FaceDetectionsStreamName));
        _faceRectStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, _FaceRectStreamName, config.AddPacketPresenceCalculator(_FaceRectStreamName));
        _faceLandmarksWithIrisStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _FaceLandmarksWithIrisStreamName, config.AddPacketPresenceCalculator(_FaceLandmarksWithIrisStreamName));
      }
      else
      {
        _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _FaceDetectionsStreamName, true);
        _faceRectStream = new OutputStream<NormalizedRectPacket, NormalizedRect>(calculatorGraph, _FaceRectStreamName, true);
        _faceLandmarksWithIrisStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(calculatorGraph, _FaceLandmarksWithIrisStreamName, true);
      }
      return calculatorGraph.Initialize(config);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
