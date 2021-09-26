using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Mediapipe.Unity.ObjectDetection {
  public class ObjectDetectionGraph : GraphRunner {
    public UnityEvent<List<Detection>> OnOutputDetectionsOutput = new UnityEvent<List<Detection>>();

    const string inputStreamName = "input_video";

    const string outputDetectionsStreamName = "output_detections";
    OutputStream<DetectionVectorPacket, List<Detection>> outputDetectionsStream;
    protected long prevOutputDetectionsMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      InitializeOutputStreams();
      outputDetectionsStream.StartPolling(true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      InitializeOutputStreams();
      outputDetectionsStream.AddListener(OutputDetectionsCallback, true).AssertOk();
      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnOutputDetectionsOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public List<Detection> FetchNextDetections() {
      outputDetectionsStream.TryGetNext(out var outputDetections);
      OnOutputDetectionsOutput.Invoke(outputDetections);
      return outputDetections;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr OutputDetectionsCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<ObjectDetectionGraph>(graphPtr, packetPtr, (objectDetectionGraph, ptr) => {
        using (var packet = new DetectionVectorPacket(ptr, false)) {
          if (objectDetectionGraph.TryGetPacketValue(packet, ref objectDetectionGraph.prevOutputDetectionsMicrosec, out var value)) {
            objectDetectionGraph.OnOutputDetectionsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
        WaitForAsset("ssdlite_object_detection_labelmap.txt"),
        WaitForAsset("ssdlite_object_detection.bytes"),
      };
    }

    protected void InitializeOutputStreams() {
      outputDetectionsStream = new OutputStream<DetectionVectorPacket, List<Detection>>(calculatorGraph, outputDetectionsStreamName);
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
