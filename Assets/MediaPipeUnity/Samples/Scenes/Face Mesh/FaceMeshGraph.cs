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

namespace Mediapipe.Unity.Sample.FaceMesh
{
  public readonly struct FaceMeshResult
  {
    public readonly List<Detection> faceDetections;
    public readonly List<NormalizedLandmarkList> multiFaceLandmarks;
    public readonly List<NormalizedRect> faceRectsFromLandmarks;
    public readonly List<NormalizedRect> faceRectsFromDetections;

    public FaceMeshResult(List<Detection> faceDetections, List<NormalizedLandmarkList> multiFaceLandmarks, List<NormalizedRect> faceRectsFromLandmarks, List<NormalizedRect> faceRectsFromDetections)
    {
      this.faceDetections = faceDetections;
      this.multiFaceLandmarks = multiFaceLandmarks;
      this.faceRectsFromLandmarks = faceRectsFromLandmarks;
      this.faceRectsFromDetections = faceRectsFromDetections;
    }
  }

  public class FaceMeshGraph : GraphRunner
  {
    public int maxNumFaces = 1;
    public bool refineLandmarks = true;

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

    public event EventHandler<OutputStream<List<Detection>>.OutputEventArgs> OnFaceDetectionsOutput
    {
      add => _faceDetectionsStream.AddListener(value, timeoutMicrosec);
      remove => _faceDetectionsStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<List<NormalizedLandmarkList>>.OutputEventArgs> OnMultiFaceLandmarksOutput
    {
      add => _multiFaceLandmarksStream.AddListener(value, timeoutMicrosec);
      remove => _multiFaceLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<List<NormalizedRect>>.OutputEventArgs> OnFaceRectsFromLandmarksOutput
    {
      add => _faceRectsFromLandmarksStream.AddListener(value, timeoutMicrosec);
      remove => _faceRectsFromLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream<List<NormalizedRect>>.OutputEventArgs> OnFaceRectsFromDetectionsOutput
    {
      add => _faceRectsFromDetectionsStream.AddListener(value, timeoutMicrosec);
      remove => _faceRectsFromDetectionsStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";

    private const string _FaceDetectionsStreamName = "face_detections";
    private const string _MultiFaceLandmarksStreamName = "multi_face_landmarks";
    private const string _FaceRectsFromLandmarksStreamName = "face_rects_from_landmarks";
    private const string _FaceRectsFromDetectionsStreamName = "face_rects_from_detections";

    private OutputStream<List<Detection>> _faceDetectionsStream;
    private OutputStream<List<NormalizedLandmarkList>> _multiFaceLandmarksStream;
    private OutputStream<List<NormalizedRect>> _faceRectsFromLandmarksStream;
    private OutputStream<List<NormalizedRect>> _faceRectsFromDetectionsStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _faceDetectionsStream.StartPolling();
        _multiFaceLandmarksStream.StartPolling();
        _faceRectsFromLandmarksStream.StartPolling();
        _faceRectsFromDetectionsStream.StartPolling();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      base.Stop();
      _faceDetectionsStream?.Dispose();
      _faceDetectionsStream = null;
      _multiFaceLandmarksStream?.Dispose();
      _multiFaceLandmarksStream = null;
      _faceRectsFromLandmarksStream?.Dispose();
      _faceRectsFromLandmarksStream = null;
      _faceRectsFromDetectionsStream?.Dispose();
      _faceRectsFromDetectionsStream = null;
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public async Task<FaceMeshResult> WaitNext()
    {
      var results = await WhenAll(
        _faceDetectionsStream.WaitNextAsync(),
        _multiFaceLandmarksStream.WaitNextAsync(),
        _faceRectsFromLandmarksStream.WaitNextAsync(),
        _faceRectsFromDetectionsStream.WaitNextAsync()
      );
      AssertResult(results);

      _ = TryGetValue(results.Item1.packet, out var faceDetections, (packet) =>
      {
        return packet.Get(Detection.Parser);
      });
      _ = TryGetValue(results.Item2.packet, out var multiFaceLandmarks, (packet) =>
      {
        return packet.Get(NormalizedLandmarkList.Parser);
      });
      _ = TryGetValue(results.Item3.packet, out var faceRectsFromLandmarks, (packet) =>
      {
        return packet.Get(NormalizedRect.Parser);
      });
      _ = TryGetValue(results.Item4.packet, out var faceRectsFromDetections, (packet) =>
      {
        return packet.Get(NormalizedRect.Parser);
      });

      return new FaceMeshResult(faceDetections, multiFaceLandmarks, faceRectsFromLandmarks, faceRectsFromDetections);
    }

    protected override void ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      _faceDetectionsStream = new OutputStream<List<Detection>>(calculatorGraph, _FaceDetectionsStreamName, true);
      _multiFaceLandmarksStream = new OutputStream<List<NormalizedLandmarkList>>(calculatorGraph, _MultiFaceLandmarksStreamName, true);
      _faceRectsFromLandmarksStream = new OutputStream<List<NormalizedRect>>(calculatorGraph, _FaceRectsFromLandmarksStreamName, true);
      _faceRectsFromDetectionsStream = new OutputStream<List<NormalizedRect>>(calculatorGraph, _FaceRectsFromDetectionsStreamName, true);

      using (var validatedGraphConfig = new ValidatedGraphConfig())
      {
        validatedGraphConfig.Initialize(config);

        var extensionRegistry = new ExtensionRegistry() { TensorsToDetectionsCalculatorOptions.Extensions.Ext, ThresholdingCalculatorOptions.Extensions.Ext };
        var cannonicalizedConfig = validatedGraphConfig.Config(extensionRegistry);
        var tensorsToDetectionsCalculators = cannonicalizedConfig.Node.Where((node) => node.Calculator == "TensorsToDetectionsCalculator").ToList();
        var thresholdingCalculators = cannonicalizedConfig.Node.Where((node) => node.Calculator == "ThresholdingCalculator").ToList();

        foreach (var calculator in tensorsToDetectionsCalculators)
        {
          foreach (var option in calculator.NodeOptions)
          {
            // The following code is a hack to work around the problem that `calculator.Options` is currently null.
            if (option.TryUnpack<TensorsToDetectionsCalculatorOptions>(out var opt))
            {
              opt.MinScoreThresh = minDetectionConfidence;
              var calculatorOptions = new CalculatorOptions();
              calculatorOptions.SetExtension(TensorsToDetectionsCalculatorOptions.Extensions.Ext, opt);
              calculator.Options = calculatorOptions;
              Debug.Log($"Min Detection Confidence = {minDetectionConfidence}");
              break;
            }
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

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset(refineLandmarks ? "face_landmark_with_attention.bytes" : "face_landmark.bytes"),
      };
    }

    private PacketMap BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new PacketMap();

      SetImageTransformationOptions(sidePacket, imageSource);
      sidePacket.Emplace("num_faces", Packet.CreateInt(maxNumFaces));
      sidePacket.Emplace("with_attention", Packet.CreateBool(refineLandmarks));

      Debug.Log($"Max Num Faces = {maxNumFaces}");
      Debug.Log($"Refine Landmarks = {refineLandmarks}");

      return sidePacket;
    }
  }
}
