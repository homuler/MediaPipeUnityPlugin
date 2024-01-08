// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Google.Protobuf;
using System.Threading.Tasks;

namespace Mediapipe.Unity.Sample.HandTracking
{
  public readonly struct HandTrackingResult
  {
    public readonly List<Detection> palmDetections;
    public readonly List<NormalizedRect> handRectsFromPalmDetections;
    public readonly List<NormalizedLandmarkList> handLandmarks;
    public readonly List<LandmarkList> handWorldLandmarks;
    public readonly List<NormalizedRect> handRectsFromLandmarks;
    public readonly List<ClassificationList> handedness;

    public HandTrackingResult(List<Detection> palmDetections, List<NormalizedRect> handRectsFromPalmDetections,
      List<NormalizedLandmarkList> handLandmarks, List<LandmarkList> handWorldLandmarks,
      List<NormalizedRect> handRectsFromLandmarks, List<ClassificationList> handedness)
    {
      this.palmDetections = palmDetections;
      this.handRectsFromPalmDetections = handRectsFromPalmDetections;
      this.handLandmarks = handLandmarks;
      this.handWorldLandmarks = handWorldLandmarks;
      this.handRectsFromLandmarks = handRectsFromLandmarks;
      this.handedness = handedness;
    }
  }

  public class HandTrackingGraph : GraphRunner
  {
    public enum ModelComplexity
    {
      Lite = 0,
      Full = 1,
    }

    public ModelComplexity modelComplexity = ModelComplexity.Full;
    public int maxNumHands = 2;

    private float _minDetectionConfidence = 0.5f;
    public float minDetectionConfidence
    {
      get => _minDetectionConfidence;
      set => _minDetectionConfidence = Mathf.Clamp01(value);
    }

    private float _minTrackingConfidence = 0.5f;
    public float minTrackingConfidence
    {
      get => _minTrackingConfidence;
      set => _minTrackingConfidence = Mathf.Clamp01(value);
    }

    public event EventHandler<OutputStream<List<Detection>>.OutputEventArgs> OnPalmDetectectionsOutput
    {
      add => _palmDetectionsStream.AddListener(value, timeoutMicrosec);
      remove => _palmDetectionsStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<List<NormalizedRect>>.OutputEventArgs> OnHandRectsFromPalmDetectionsOutput
    {
      add => _handRectsFromPalmDetectionsStream.AddListener(value, timeoutMicrosec);
      remove => _handRectsFromPalmDetectionsStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<List<NormalizedLandmarkList>>.OutputEventArgs> OnHandLandmarksOutput
    {
      add => _handLandmarksStream.AddListener(value, timeoutMicrosec);
      remove => _handLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<List<LandmarkList>>.OutputEventArgs> OnHandWorldLandmarksOutput
    {
      add => _handWorldLandmarksStream.AddListener(value, timeoutMicrosec);
      remove => _handWorldLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<List<NormalizedRect>>.OutputEventArgs> OnHandRectsFromLandmarksOutput
    {
      add => _handRectsFromLandmarksStream.AddListener(value, timeoutMicrosec);
      remove => _handRectsFromLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<List<ClassificationList>>.OutputEventArgs> OnHandednessOutput
    {
      add => _handednessStream.AddListener(value, timeoutMicrosec);
      remove => _handednessStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";
    private const string _PalmDetectionsStreamName = "palm_detections";
    private const string _HandRectsFromPalmDetectionsStreamName = "hand_rects_from_palm_detections";
    private const string _HandLandmarksStreamName = "hand_landmarks";
    private const string _HandWorldLandmarksStreamName = "hand_world_landmarks";
    private const string _HandRectsFromLandmarksStreamName = "hand_rects_from_landmarks";
    private const string _HandednessStreamName = "handedness";

    private OutputStream<List<Detection>> _palmDetectionsStream;
    private OutputStream<List<NormalizedRect>> _handRectsFromPalmDetectionsStream;
    private OutputStream<List<NormalizedLandmarkList>> _handLandmarksStream;
    private OutputStream<List<LandmarkList>> _handWorldLandmarksStream;
    private OutputStream<List<NormalizedRect>> _handRectsFromLandmarksStream;
    private OutputStream<List<ClassificationList>> _handednessStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _palmDetectionsStream.StartPolling();
        _handRectsFromPalmDetectionsStream.StartPolling();
        _handLandmarksStream.StartPolling();
        _handWorldLandmarksStream.StartPolling();
        _handRectsFromLandmarksStream.StartPolling();
        _handednessStream.StartPolling();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      _palmDetectionsStream?.Dispose();
      _palmDetectionsStream = null;
      _handRectsFromPalmDetectionsStream?.Dispose();
      _handRectsFromPalmDetectionsStream = null;
      _handLandmarksStream?.Dispose();
      _handLandmarksStream = null;
      _handWorldLandmarksStream?.Dispose();
      _handWorldLandmarksStream = null;
      _handRectsFromLandmarksStream?.Dispose();
      _handRectsFromLandmarksStream = null;
      _handednessStream?.Dispose();
      _handednessStream = null;
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public async Task<HandTrackingResult> WaitNext()
    {
      var results = await WhenAll(
        _palmDetectionsStream.WaitNextAsync(),
        _handRectsFromPalmDetectionsStream.WaitNextAsync(),
        _handLandmarksStream.WaitNextAsync(),
        _handWorldLandmarksStream.WaitNextAsync(),
        _handRectsFromLandmarksStream.WaitNextAsync(),
        _handednessStream.WaitNextAsync()
      );
      AssertResult(results);

      _ = TryGetValue(results.Item1.packet, out var palmDetections, (packet) =>
      {
        return packet.Get(Detection.Parser);
      });
      _ = TryGetValue(results.Item2.packet, out var handRectsFromPalmDetections, (packet) =>
      {
        return packet.Get(NormalizedRect.Parser);
      });
      _ = TryGetValue(results.Item3.packet, out var handLandmarks, (packet) =>
      {
        return packet.Get(NormalizedLandmarkList.Parser);
      });
      _ = TryGetValue(results.Item4.packet, out var handWorldLandmarks, (packet) =>
      {
        return packet.Get(LandmarkList.Parser);
      });
      _ = TryGetValue(results.Item5.packet, out var handRectsFromLandmarks, (packet) =>
      {
        return packet.Get(NormalizedRect.Parser);
      });
      _ = TryGetValue(results.Item6.packet, out var handedness, (packet) =>
      {
        return packet.Get(ClassificationList.Parser);
      });

      return new HandTrackingResult(palmDetections, handRectsFromPalmDetections, handLandmarks, handWorldLandmarks, handRectsFromLandmarks, handedness);
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForHandLandmarkModel(),
        WaitForAsset("handedness.txt"),
        WaitForPalmDetectionModel(),
      };
    }

    protected override void ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      _palmDetectionsStream = new OutputStream<List<Detection>>(calculatorGraph, _PalmDetectionsStreamName, true);
      _handRectsFromPalmDetectionsStream = new OutputStream<List<NormalizedRect>>(calculatorGraph, _HandRectsFromPalmDetectionsStreamName, true);
      _handLandmarksStream = new OutputStream<List<NormalizedLandmarkList>>(calculatorGraph, _HandLandmarksStreamName, true);
      _handWorldLandmarksStream = new OutputStream<List<LandmarkList>>(calculatorGraph, _HandWorldLandmarksStreamName, true);
      _handRectsFromLandmarksStream = new OutputStream<List<NormalizedRect>>(calculatorGraph, _HandRectsFromLandmarksStreamName, true);
      _handednessStream = new OutputStream<List<ClassificationList>>(calculatorGraph, _HandednessStreamName, true);

      using (var validatedGraphConfig = new ValidatedGraphConfig())
      {
        validatedGraphConfig.Initialize(config);

        var extensionRegistry = new ExtensionRegistry() { TensorsToDetectionsCalculatorOptions.Extensions.Ext, ThresholdingCalculatorOptions.Extensions.Ext };
        var cannonicalizedConfig = validatedGraphConfig.Config(extensionRegistry);
        var tensorsToDetectionsCalculators = cannonicalizedConfig.Node.Where((node) => node.Calculator == "TensorsToDetectionsCalculator").ToList();
        var thresholdingCalculators = cannonicalizedConfig.Node.Where((node) => node.Calculator == "ThresholdingCalculator").ToList();

        foreach (var calculator in tensorsToDetectionsCalculators)
        {
          if (calculator.Options.HasExtension(TensorsToDetectionsCalculatorOptions.Extensions.Ext))
          {
            var options = calculator.Options.GetExtension(TensorsToDetectionsCalculatorOptions.Extensions.Ext);
            options.MinScoreThresh = minDetectionConfidence;
            Debug.Log($"Min Detection Confidence = {minDetectionConfidence}");
          }
        }

        foreach (var calculator in thresholdingCalculators)
        {
          if (calculator.Options.HasExtension(ThresholdingCalculatorOptions.Extensions.Ext))
          {
            var options = calculator.Options.GetExtension(ThresholdingCalculatorOptions.Extensions.Ext);
            options.Threshold = minTrackingConfidence;
            Debug.Log($"Min Tracking Confidence = {minTrackingConfidence}");
          }
        }
        calculatorGraph.Initialize(cannonicalizedConfig);
      }
    }

    private WaitForResult WaitForHandLandmarkModel()
    {
      switch (modelComplexity)
      {
        case ModelComplexity.Lite: return WaitForAsset("hand_landmark_lite.bytes");
        case ModelComplexity.Full: return WaitForAsset("hand_landmark_full.bytes");
        default: throw new InternalException($"Invalid model complexity: {modelComplexity}");
      }
    }

    private WaitForResult WaitForPalmDetectionModel()
    {
      switch (modelComplexity)
      {
        case ModelComplexity.Lite: return WaitForAsset("palm_detection_lite.bytes");
        case ModelComplexity.Full: return WaitForAsset("palm_detection_full.bytes");
        default: throw new InternalException($"Invalid model complexity: {modelComplexity}");
      }
    }

    private PacketMap BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new PacketMap();

      SetImageTransformationOptions(sidePacket, imageSource, true);
      sidePacket.Emplace("model_complexity", Packet.CreateInt((int)modelComplexity));
      sidePacket.Emplace("num_hands", Packet.CreateInt(maxNumHands));

      Debug.Log($"Model Complexity = {modelComplexity}");
      Debug.Log($"Max Num Hands = {maxNumHands}");

      return sidePacket;
    }
  }
}
