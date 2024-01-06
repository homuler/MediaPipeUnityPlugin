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

    public event EventHandler<OutputStream.OutputEventArgs> OnPalmDetectectionsOutput
    {
      add => _palmDetectionsStream.AddListener(value);
      remove => _palmDetectionsStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream.OutputEventArgs> OnHandRectsFromPalmDetectionsOutput
    {
      add => _handRectsFromPalmDetectionsStream.AddListener(value);
      remove => _handRectsFromPalmDetectionsStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream.OutputEventArgs> OnHandLandmarksOutput
    {
      add => _handLandmarksStream.AddListener(value);
      remove => _handLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream.OutputEventArgs> OnHandWorldLandmarksOutput
    {
      add => _handWorldLandmarksStream.AddListener(value);
      remove => _handWorldLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream.OutputEventArgs> OnHandRectsFromLandmarksOutput
    {
      add => _handRectsFromLandmarksStream.AddListener(value);
      remove => _handRectsFromLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream.OutputEventArgs> OnHandednessOutput
    {
      add => _handednessStream.AddListener(value);
      remove => _handednessStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";
    private const string _PalmDetectionsStreamName = "palm_detections";
    private const string _HandRectsFromPalmDetectionsStreamName = "hand_rects_from_palm_detections";
    private const string _HandLandmarksStreamName = "hand_landmarks";
    private const string _HandWorldLandmarksStreamName = "hand_world_landmarks";
    private const string _HandRectsFromLandmarksStreamName = "hand_rects_from_landmarks";
    private const string _HandednessStreamName = "handedness";

    private OutputStream _palmDetectionsStream;
    private OutputStream _handRectsFromPalmDetectionsStream;
    private OutputStream _handLandmarksStream;
    private OutputStream _handWorldLandmarksStream;
    private OutputStream _handRectsFromLandmarksStream;
    private OutputStream _handednessStream;

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
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public async Task<HandTrackingResult> WaitNext()
    {
      var results = await Task.WhenAll(
        _palmDetectionsStream.WaitNextAsync(),
        _handRectsFromPalmDetectionsStream.WaitNextAsync(),
        _handLandmarksStream.WaitNextAsync(),
        _handWorldLandmarksStream.WaitNextAsync(),
        _handRectsFromLandmarksStream.WaitNextAsync(),
        _handednessStream.WaitNextAsync()
      );
      AssertResult(results);

      _ = TryGetValue(results[0].packet, out var palmDetections, (packet) =>
      {
        return packet.GetProtoList(Detection.Parser);
      });
      _ = TryGetValue(results[1].packet, out var handRectsFromPalmDetections, (packet) =>
      {
        return packet.GetProtoList(NormalizedRect.Parser);
      });
      _ = TryGetValue(results[2].packet, out var handLandmarks, (packet) =>
      {
        return packet.GetProtoList(NormalizedLandmarkList.Parser);
      });
      _ = TryGetValue(results[3].packet, out var handWorldLandmarks, (packet) =>
      {
        return packet.GetProtoList(LandmarkList.Parser);
      });
      _ = TryGetValue(results[4].packet, out var handRectsFromLandmarks, (packet) =>
      {
        return packet.GetProtoList(NormalizedRect.Parser);
      });
      _ = TryGetValue(results[5].packet, out var handedness, (packet) =>
      {
        return packet.GetProtoList(ClassificationList.Parser);
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
      _palmDetectionsStream = new OutputStream(calculatorGraph, _PalmDetectionsStreamName, true);
      _handRectsFromPalmDetectionsStream = new OutputStream(calculatorGraph, _HandRectsFromPalmDetectionsStreamName, true);
      _handLandmarksStream = new OutputStream(calculatorGraph, _HandLandmarksStreamName, true);
      _handWorldLandmarksStream = new OutputStream(calculatorGraph, _HandWorldLandmarksStreamName, true);
      _handRectsFromLandmarksStream = new OutputStream(calculatorGraph, _HandRectsFromLandmarksStreamName, true);
      _handednessStream = new OutputStream(calculatorGraph, _HandednessStreamName, true);

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
