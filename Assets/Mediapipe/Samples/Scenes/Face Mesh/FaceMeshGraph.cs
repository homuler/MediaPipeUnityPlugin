using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.FaceMesh
{
  public class FaceMeshGraph : GraphRunner
  {
    public int maxNumFaces = 1;
    public UnityEvent<List<Detection>> OnFaceDetectionsOutput = new UnityEvent<List<Detection>>();
    public UnityEvent<List<NormalizedLandmarkList>> OnMultiFaceLandmarksOutput = new UnityEvent<List<NormalizedLandmarkList>>();
    public UnityEvent<List<NormalizedRect>> OnFaceRectsFromLandmarksOutput = new UnityEvent<List<NormalizedRect>>();

    const string inputStreamName = "input_video";

    const string faceDetectionsStreamName = "face_detections";
    const string multiFaceLandmarksStreamName = "multi_face_landmarks";
    const string faceRectsFromLandmarksStreamName = "face_rects_from_landmarks";

    OutputStream<DetectionVectorPacket, List<Detection>> faceDetectionsStream;
    OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>> multiFaceLandmarksStream;
    OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>> faceRectsFromLandmarksStream;

    protected long prevFaceDetectionsMicrosec = 0;
    protected long prevMultiFaceLandmarksMicrosec = 0;
    protected long prevFaceRectsFromLandmarksMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();

      faceDetectionsStream.StartPolling(true).AssertOk();
      multiFaceLandmarksStream.StartPolling(true).AssertOk();
      faceRectsFromLandmarksStream.StartPolling(true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();

      faceDetectionsStream.AddListener(FaceDetectionsCallback, true).AssertOk();
      multiFaceLandmarksStream.AddListener(MultiFaceLandmarksCallback, true).AssertOk();
      faceRectsFromLandmarksStream.AddListener(FaceRectsFromLandmarksCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnFaceDetectionsOutput.RemoveAllListeners();
      OnMultiFaceLandmarksOutput.RemoveAllListeners();
      OnFaceRectsFromLandmarksOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public FaceMeshValue FetchNextValue()
    {
      faceDetectionsStream.TryGetNext(out var faceDetections);
      multiFaceLandmarksStream.TryGetNext(out var multiFaceLandmarks);
      faceRectsFromLandmarksStream.TryGetNext(out var faceRectsFromLandmarks);

      OnFaceDetectionsOutput.Invoke(faceDetections);
      OnMultiFaceLandmarksOutput.Invoke(multiFaceLandmarks);
      OnFaceRectsFromLandmarksOutput.Invoke(faceRectsFromLandmarks);

      return new FaceMeshValue(faceDetections, multiFaceLandmarks, faceRectsFromLandmarks);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr)
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
    static IntPtr MultiFaceLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
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
    static IntPtr FaceRectsFromLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
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

    protected void InitializeOutputStreams()
    {
      faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, faceDetectionsStreamName);
      multiFaceLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, multiFaceLandmarksStreamName);
      faceRectsFromLandmarksStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, faceRectsFromLandmarksStreamName);
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset("face_landmark.bytes"),
      };
    }

    SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("num_faces", new IntPacket(maxNumFaces));

      return sidePacket;
    }
  }
}
