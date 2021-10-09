// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Mediapipe.Unity.Objectron
{
  public class ObjectronGraph : GraphRunner
  {
    [Serializable]
    public enum Category
    {
      Camera,
      Chair,
      Cup,
      Sneaker,
    };

    public Category category;
    public int maxNumObjects = 5;

    public Vector2 focalLength
    {
      get
      {
        if (inferenceMode == InferenceMode.GPU)
        {
          return new Vector2(2.0975f, 1.5731f);  // magic numbers MediaPipe uses internally
        }
        return Vector2.one;
      }
    }

    public Vector2 principalPoint => Vector2.zero;

#pragma warning disable IDE1006  // UnityEvent is PascalCase
    public UnityEvent<FrameAnnotation> OnLiftedObjectsOutput = new UnityEvent<FrameAnnotation>();
    public UnityEvent<List<NormalizedRect>> OnMultiBoxRectsOutput = new UnityEvent<List<NormalizedRect>>();
    public UnityEvent<List<NormalizedLandmarkList>> OnMultiBoxLandmarksOutput = new UnityEvent<List<NormalizedLandmarkList>>();
#pragma warning restore IDE1006

    private const string _InputStreamName = "input_video";

    private const string _LiftedObjectsStreamName = "lifted_objects";
    private const string _MultiBoxRectsStreamName = "multi_box_rects";
    private const string _MultiBoxLandmarksStreamName = "multi_box_landmarks";

    private OutputStream<FrameAnnotationPacket, FrameAnnotation> _liftedObjectsStream;
    private OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>> _multiBoxRectsStream;
    private OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>> _multiBoxLandmarksStream;

    protected long prevLiftedObjectsMicrosec = 0;
    protected long prevMultiBoxRectsMicrosec = 0;
    protected long prevMultiBoxLandmarksMicrosec = 0;

    public override Status StartRun(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _liftedObjectsStream.StartPolling(true).AssertOk();
      _multiBoxRectsStream.StartPolling(true).AssertOk();
      _multiBoxLandmarksStream.StartPolling(true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public Status StartRunAsync(ImageSource imageSource)
    {
      InitializeOutputStreams();

      _liftedObjectsStream.AddListener(LiftedObjectsCallback, true).AssertOk();
      _multiBoxRectsStream.AddListener(MultiBoxRectsCallback, true).AssertOk();
      _multiBoxLandmarksStream.AddListener(MultiBoxLandmarksCallback, true).AssertOk();

      return calculatorGraph.StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      OnLiftedObjectsOutput.RemoveAllListeners();
      OnMultiBoxRectsOutput.RemoveAllListeners();
      OnMultiBoxLandmarksOutput.RemoveAllListeners();
    }

    public Status AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      return AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public ObjectronValue FetchNextValue()
    {
      var _ = _liftedObjectsStream.TryGetNext(out var liftedObjects);
      _ = _multiBoxRectsStream.TryGetNext(out var multiBoxRects);
      _ = _multiBoxLandmarksStream.TryGetNext(out var multiBoxLandmarks);

      OnLiftedObjectsOutput.Invoke(liftedObjects);
      OnMultiBoxRectsOutput.Invoke(multiBoxRects);
      OnMultiBoxLandmarksOutput.Invoke(multiBoxLandmarks);

      return new ObjectronValue(liftedObjects, multiBoxRects, multiBoxLandmarks);
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr LiftedObjectsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<ObjectronGraph>(graphPtr, packetPtr, (objectronGraph, ptr) =>
      {
        using (var packet = new FrameAnnotationPacket(ptr, false))
        {
          if (objectronGraph.TryGetPacketValue(packet, ref objectronGraph.prevLiftedObjectsMicrosec, out var value))
          {
            objectronGraph.OnLiftedObjectsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr MultiBoxRectsCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<ObjectronGraph>(graphPtr, packetPtr, (objectronGraph, ptr) =>
      {
        using (var packet = new NormalizedRectVectorPacket(ptr, false))
        {
          if (objectronGraph.TryGetPacketValue(packet, ref objectronGraph.prevMultiBoxRectsMicrosec, out var value))
          {
            objectronGraph.OnMultiBoxRectsOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    [AOT.MonoPInvokeCallback(typeof(CalculatorGraph.NativePacketCallback))]
    private static IntPtr MultiBoxLandmarksCallback(IntPtr graphPtr, IntPtr packetPtr)
    {
      return InvokeIfGraphRunnerFound<ObjectronGraph>(graphPtr, packetPtr, (objectronGraph, ptr) =>
      {
        using (var packet = new NormalizedLandmarkListVectorPacket(ptr, false))
        {
          if (objectronGraph.TryGetPacketValue(packet, ref objectronGraph.prevMultiBoxLandmarksMicrosec, out var value))
          {
            objectronGraph.OnMultiBoxLandmarksOutput.Invoke(value);
          }
        }
      }).mpPtr;
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("object_detection_ssd_mobilenetv2_oidv4_fp16.bytes"),
        WaitForAsset("object_detection_oidv4_labelmap.txt"),
        WaitForAsset(GetModelAssetName(category), "object_detection_3d.bytes", true),
      };
    }

    protected void InitializeOutputStreams()
    {
      _liftedObjectsStream = new OutputStream<FrameAnnotationPacket, FrameAnnotation>(calculatorGraph, _LiftedObjectsStreamName);
      _multiBoxRectsStream = new OutputStream<NormalizedRectVectorPacket, List<NormalizedRect>>(calculatorGraph, _MultiBoxRectsStreamName);
      _multiBoxLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _MultiBoxLandmarksStreamName);
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("allowed_labels", new StringPacket(GetAllowedLabels(category)));
      sidePacket.Emplace("max_num_objects", new IntPacket(maxNumObjects));

      return sidePacket;
    }

    private string GetAllowedLabels(Category category)
    {
      switch (category)
      {
        case Category.Camera:
          {
            return "Camera";
          }
        case Category.Chair:
          {
            return "Chair";
          }
        case Category.Cup:
          {
            return "Coffee cup,Mug";
          }
        case Category.Sneaker:
          {
            return "Footwear";
          }
        default:
          {
            throw new ArgumentException($"Unknown category: {category}");
          }
      }
    }

    private string GetModelAssetName(Category category)
    {
      switch (category)
      {
        case Category.Camera:
          {
            return "object_detection_3d_camera.bytes";
          }
        case Category.Chair:
          {
            return "object_detection_3d_chair.bytes";
          }
        case Category.Cup:
          {
            return "object_detection_3d_chair.bytes";
          }
        case Category.Sneaker:
          {
            return "object_detection_3d_sneakers.bytes";
          }
        default:
          {
            throw new ArgumentException($"Unknown category: {category}");
          }
      }
    }
  }
}
