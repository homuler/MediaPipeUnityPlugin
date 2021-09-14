using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mediapipe.Unity.Objectron {
  public class ObjectronGraph : GraphRunner {
    [Serializable]
    public enum Category {
      Camera,
      Chair,
      Cup,
      Sneaker,
    };

    public Category category;
    public int maxNumObjects = 5;

    public Vector2 focalLength {
      get {
        if (inferenceMode == InferenceMode.GPU) {
          return new Vector2(2.0975f, 1.5731f);  // magic numbers MediaPipe uses internally
        }
        return Vector2.one;
      }
    }

    public Vector2 principalPoint {
      get { return Vector2.zero; }
    }

    public UnityEvent<FrameAnnotation> OnLiftedObjectsOutput = new UnityEvent<FrameAnnotation>();
    public UnityEvent<List<NormalizedRect>> OnMultiBoxRectsOutput = new UnityEvent<List<NormalizedRect>>();
    public UnityEvent<List<NormalizedLandmarkList>> OnMultiBoxLandmarksOutput = new UnityEvent<List<NormalizedLandmarkList>>();

    const string inputStreamName = "input_video";

    const string liftedObjectsStreamName = "lifted_objects";
    OutputStreamPoller<FrameAnnotation> liftedObjectsStreamPoller;
    FrameAnnotationPacket liftedObjectsPacket;
    protected long prevLiftedObjectsMicrosec = 0;

    const string multiBoxRectsStreamName = "multi_box_rects";
    OutputStreamPoller<List<NormalizedRect>> multiBoxRectsStreamPoller;
    NormalizedRectVectorPacket multiBoxRectsPacket;
    protected long prevMultiBoxRectsMicrosec = 0;

    const string multiBoxLandmarksStreamName = "multi_box_landmarks";
    OutputStreamPoller<List<NormalizedLandmarkList>> multiBoxLandmarksStreamPoller;
    NormalizedLandmarkListVectorPacket multiBoxLandmarksPacket;
    protected long prevMultiBoxLandmarksMicrosec = 0;

    public override Status StartRun(ImageSource imageSource) {
      liftedObjectsStreamPoller = calculatorGraph.AddOutputStreamPoller<FrameAnnotation>(liftedObjectsStreamName, true).Value();
      liftedObjectsPacket = new FrameAnnotationPacket();

      multiBoxRectsStreamPoller = calculatorGraph.AddOutputStreamPoller<List<NormalizedRect>>(multiBoxRectsStreamName, true).Value();
      multiBoxRectsPacket = new NormalizedRectVectorPacket();

      multiBoxLandmarksStreamPoller = calculatorGraph.AddOutputStreamPoller<List<NormalizedLandmarkList>>(multiBoxLandmarksStreamName, true).Value();
      multiBoxLandmarksPacket = new NormalizedLandmarkListVectorPacket();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource) {
      calculatorGraph.ObserveOutputStream(liftedObjectsStreamName, LiftedObjectsCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(multiBoxRectsStreamName, MultiBoxRectsCallback, true).AssertOk();
      calculatorGraph.ObserveOutputStream(multiBoxLandmarksStreamName, MultiBoxLandmarksCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop() {
      base.Stop();
      OnLiftedObjectsOutput.RemoveAllListeners();
      OnMultiBoxRectsOutput.RemoveAllListeners();
      OnMultiBoxLandmarksOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame) {
      return AddTextureFrameToInputStream(inputStreamName, textureFrame);
    }

    public ObjectronValue FetchNextValue() {
      var liftedObjects = FetchNext(liftedObjectsStreamPoller, liftedObjectsPacket, liftedObjectsStreamName);
      var multiBoxRects = FetchNext(multiBoxRectsStreamPoller, multiBoxRectsPacket, multiBoxRectsStreamName);
      var multiBoxLandmarks = FetchNext(multiBoxLandmarksStreamPoller, multiBoxLandmarksPacket, multiBoxLandmarksStreamName);

      OnLiftedObjectsOutput.Invoke(liftedObjects);
      OnMultiBoxRectsOutput.Invoke(multiBoxRects);
      OnMultiBoxLandmarksOutput.Invoke(multiBoxLandmarks);

      return new ObjectronValue(liftedObjects, multiBoxRects, multiBoxLandmarks);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr LiftedObjectsCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<ObjectronGraph>(graphPtr, packetPtr, (objectronGraph, ptr) => {
        using (var packet = new FrameAnnotationPacket(ptr, false)) {
          if (objectronGraph.TryGetPacketValue(packet, ref objectronGraph.prevLiftedObjectsMicrosec, out var value)) {
            objectronGraph.OnLiftedObjectsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr MultiBoxRectsCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<ObjectronGraph>(graphPtr, packetPtr, (objectronGraph, ptr) => {
        using (var packet = new NormalizedRectVectorPacket(ptr, false)) {
          if (objectronGraph.TryGetPacketValue(packet, ref objectronGraph.prevMultiBoxRectsMicrosec, out var value)) {
            objectronGraph.OnMultiBoxRectsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    static IntPtr MultiBoxLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr){
      return InvokeIfGraphRunnerFound<ObjectronGraph>(graphPtr, packetPtr, (objectronGraph, ptr) => {
        using (var packet = new NormalizedLandmarkListVectorPacket(ptr, false)) {
          if (objectronGraph.TryGetPacketValue(packet, ref objectronGraph.prevMultiBoxLandmarksMicrosec, out var value)) {
            objectronGraph.OnMultiBoxLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets() {
      return new List<WaitForResult> {
        WaitForAsset("object_detection_ssd_mobilenetv2_oidv4_fp16.bytes"),
        WaitForAsset("object_detection_oidv4_labelmap.txt"),
        WaitForAsset(GetModelAssetName(category), "object_detection_3d.bytes", true),
      };
    }

    SidePacket BuildSidePacket(ImageSource imageSource) {
      var sidePacket = new SidePacket();
      sidePacket.Emplace("allowed_labels", new StringPacket(GetAllowedLabels(category)));
      sidePacket.Emplace("max_num_objects", new IntPacket(maxNumObjects));

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

    string GetAllowedLabels(Category category) {
      switch (category) {
        case Category.Camera: {
          return "Camera";
        }
        case Category.Chair: {
          return "Chair";
        }
        case Category.Cup: {
          return "Coffee cup,Mug";
        }
        default: {
          return "Footwear";
        }
      }
    }

    string GetModelAssetName(Category category) {
      switch (category) {
        case Category.Camera: {
          return "object_detection_3d_camera.bytes";
        }
        case Category.Chair: {
          return "object_detection_3d_chair.bytes";
        }
        case Category.Cup: {
          return "object_detection_3d_chair.bytes";
        }
        default: {
          return "object_detection_3d_sneakers.bytes";
        }
      }
    }
  }
}
