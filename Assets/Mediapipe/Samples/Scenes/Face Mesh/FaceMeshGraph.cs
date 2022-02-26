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

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _faceDetectionsStream.StartPolling().AssertOk();
        _multiFaceLandmarksStream.StartPolling().AssertOk();
        _faceRectsFromLandmarksStream.StartPolling().AssertOk();
        _faceRectsFromDetectionsStream.StartPolling().AssertOk();
      }
      else
      {
        _faceDetectionsStream.AddListener(FaceDetectionsCallback).AssertOk();
        _multiFaceLandmarksStream.AddListener(MultiFaceLandmarksCallback).AssertOk();
        _faceRectsFromLandmarksStream.AddListener(FaceRectsFromLandmarksCallback).AssertOk();
        _faceRectsFromDetectionsStream.AddListener(FaceRectsFromDetectionsCallback).AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnFaceDetectionsOutput.RemoveAllListeners();
      OnMultiFaceLandmarksOutput.RemoveAllListeners();
      OnFaceRectsFromLandmarksOutput.RemoveAllListeners();
      OnFaceRectsFromDetectionsOutput.RemoveAllListeners();
      _faceDetectionsStream = null;
      _multiFaceLandmarksStream = null;
      _faceRectsFromLandmarksStream = null;
      _faceRectsFromDetectionsStream = null;
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNext(out List<Detection> faceDetections, out List<NormalizedLandmarkList> multiFaceLandmarks,
                           out List<NormalizedRect> faceRectsFromLandmarks, out List<NormalizedRect> faceRectsFromDetections, bool allowBlock = true)
    {
      var currentTimestampMicrosec = GetCurrentTimestampMicrosec();
      var r1 = TryGetNext(_faceDetectionsStream, out faceDetections, allowBlock, currentTimestampMicrosec);
      var r2 = TryGetNext(_multiFaceLandmarksStream, out multiFaceLandmarks, allowBlock, currentTimestampMicrosec);
      var r3 = TryGetNext(_faceRectsFromLandmarksStream, out faceRectsFromLandmarks, allowBlock, currentTimestampMicrosec);
      var r4 = TryGetNext(_faceRectsFromDetectionsStream, out faceRectsFromDetections, allowBlock, currentTimestampMicrosec);

      if (r1) { OnFaceDetectionsOutput.Invoke(faceDetections); }
      if (r2) { OnMultiFaceLandmarksOutput.Invoke(multiFaceLandmarks); }
      if (r3) { OnFaceRectsFromLandmarksOutput.Invoke(faceRectsFromLandmarks); }
      if (r4) { OnFaceRectsFromDetectionsOutput.Invoke(faceRectsFromDetections); }

      return r1 || r2 || r3 || r4;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr FaceDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<FaceMeshGraph>(graphPtr, packetPtr, (faceMeshGraph, ptr) =>
      {
        using (var packet = new DetectionVectorPacket(ptr, false))
        {
          if (faceMeshGraph._faceDetectionsStream.TryGetPacketValue(packet, out var value, faceMeshGraph.timeoutMicrosec))
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
          if (faceMeshGraph._multiFaceLandmarksStream.TryGetPacketValue(packet, out var value, faceMeshGraph.timeoutMicrosec))
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
          if (faceMeshGraph._faceRectsFromLandmarksStream.TryGetPacketValue(packet, out var value, faceMeshGraph.timeoutMicrosec))
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
          if (faceMeshGraph._faceRectsFromDetectionsStream.TryGetPacketValue(packet, out var value, faceMeshGraph.timeoutMicrosec))
          {
            faceMeshGraph.OnFaceRectsFromDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (runningMode == RunningMode.NonBlockingSync)
      {
        _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _FaceDetectionsStreamName, config.AddPacketPresenceCalculator(_FaceDetectionsStreamName));
        _multiFaceLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _MultiFaceLandmarksStreamName, config.AddPacketPresenceCalculator(_MultiFaceLandmarksStreamName));
        _faceRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _FaceRectsFromLandmarksStreamName, config.AddPacketPresenceCalculator(_FaceRectsFromLandmarksStreamName));
        _faceRectsFromDetectionsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _FaceRectsFromDetectionsStreamName, config.AddPacketPresenceCalculator(_FaceDetectionsStreamName));
      }
      else
      {
        _faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, _FaceDetectionsStreamName, true);
        _multiFaceLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _MultiFaceLandmarksStreamName, true);
        _faceRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _FaceRectsFromLandmarksStreamName, true);
        _faceRectsFromDetectionsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _FaceRectsFromDetectionsStreamName, true);
      }
      return calculatorGraph.Initialize(config);
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

      Logger.LogInfo(TAG, $"Max Num Faces = {maxNumFaces}");
      Logger.LogInfo(TAG, $"Refine Landmarks = {refineLandmarks}");

      return sidePacket;
    }
  }
}
