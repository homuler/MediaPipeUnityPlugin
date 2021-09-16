using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.FaceDetection {
  public class FaceDetectionGraph : GraphRunner {
    public enum ModelType {
      ShortRange = 0,
      FullRangeSparse = 1,
    }
    public ModelType modelType = ModelType.ShortRange;
    public UnityEvent<List<Detection>> OnFaceDetectionsOutput = new UnityEvent<List<Detection>>();

    const string inputStreamName = "input_video";

    const string faceDetectionsStreamName = "face_detections";
    OutputStream<DetectionVectorPacket, List<Detection>> faceDetectionsStream;
    protected long prevFaceDetectionsMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      InitializeOutputStreams();
      faceDetectionsStream.StartPolling(true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      InitializeOutputStreams();
      faceDetectionsStream.AddListener(FaceDetectionsCallback, true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnFaceDetectionsOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public List<Detection> FetchNextValue() {
      faceDetectionsStream.TryGetNext(out var faceDetections);
      OnFaceDetectionsOutput.Invoke(faceDetections);
      return faceDetections;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr FaceDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<FaceDetectionGraph>(graphPtr, packetPtr, (faceDetectionGraph, ptr) => {
        using (var packet = new DetectionVectorPacket(ptr, false)) {
          if (faceDetectionGraph.TryGetPacketValue(packet, ref faceDetectionGraph.prevFaceDetectionsMicrosec, out var value)) {
            faceDetectionGraph.OnFaceDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset("face_detection_full_range_sparse.bytes"),
      };
    }

    protected void InitializeOutputStreams() {
      faceDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, faceDetectionsStreamName);
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();
      sidePacket.Emplace("model_type", new IntPacket((int)modelType));

      // Coordinate transformation from Unity to MediaPipe
      if (imageSource.isMirrored) {
        sidePacket.Emplace("input_rotation", new IntPacket(180));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(false));
      } else {
        sidePacket.Emplace("input_rotation", new IntPacket(0));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(true));
      }

      return sidePacket;
    }
  }
}
