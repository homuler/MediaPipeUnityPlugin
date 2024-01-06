// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mediapipe.Unity.Sample.IrisTracking
{
  public readonly struct IrisTrackingResult
  {
    public readonly List<Detection> faceDetections;
    public readonly NormalizedRect faceRect;
    public readonly NormalizedLandmarkList faceLandmarksWithIris;

    public IrisTrackingResult(List<Detection> faceDetections, NormalizedRect faceRect, NormalizedLandmarkList faceLandmarksWithIris)
    {
      this.faceDetections = faceDetections;
      this.faceRect = faceRect;
      this.faceLandmarksWithIris = faceLandmarksWithIris;
    }
  }

  public class IrisTrackingGraph : GraphRunner
  {
    public event EventHandler<OutputStream.OutputEventArgs> OnFaceDetectionsOutput
    {
      add => _faceDetectionsStream.AddListener(value, timeoutMicrosec);
      remove => _faceDetectionsStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream.OutputEventArgs> OnFaceRectOutput
    {
      add => _faceRectStream.AddListener(value, timeoutMicrosec);
      remove => _faceRectStream.RemoveListener(value);
    }

    public event EventHandler<OutputStream.OutputEventArgs> OnFaceLandmarksWithIrisOutput
    {
      add => _faceLandmarksWithIrisStream.AddListener(value, timeoutMicrosec);
      remove => _faceLandmarksWithIrisStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";

    private const string _FaceDetectionsStreamName = "face_detections";
    private const string _FaceRectStreamName = "face_rect";
    private const string _FaceLandmarksWithIrisStreamName = "face_landmarks_with_iris";

    private OutputStream _faceDetectionsStream;
    private OutputStream _faceRectStream;
    private OutputStream _faceLandmarksWithIrisStream;

    public override void StartRun(ImageSource imageSource)
    {
      if (runningMode.IsSynchronous())
      {
        _faceDetectionsStream.StartPolling();
        _faceRectStream.StartPolling();
        _faceLandmarksWithIrisStream.StartPolling();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _faceDetectionsStream?.Dispose();
      _faceDetectionsStream = null;
      _faceRectStream?.Dispose();
      _faceRectStream = null;
      _faceLandmarksWithIrisStream?.Dispose();
      _faceLandmarksWithIrisStream = null;
      base.Stop();
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public async Task<IrisTrackingResult> WaitNext()
    {
      var results = await Task.WhenAll(
        _faceDetectionsStream.WaitNextAsync(),
        _faceRectStream.WaitNextAsync(),
        _faceLandmarksWithIrisStream.WaitNextAsync()
      );

      AssertResult(results);

      _ = TryGetValue(results[0].packet, out var faceDetections, (packet) =>
      {
        return packet.GetProtoList(Detection.Parser);
      });
      _ = TryGetValue(results[1].packet, out var faceRect, (packet) =>
      {
        return packet.GetProto(NormalizedRect.Parser);
      });
      _ = TryGetValue(results[2].packet, out var faceLandmarksWithIris, (packet) =>
      {
        return packet.GetProto(NormalizedLandmarkList.Parser);
      });

      return new IrisTrackingResult(faceDetections, faceRect, faceLandmarksWithIris);
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForAsset("face_detection_short_range.bytes"),
        WaitForAsset("face_landmark.bytes"),
        WaitForAsset("iris_landmark.bytes"),
      };
    }

    protected override void ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      _faceDetectionsStream = new OutputStream(calculatorGraph, _FaceDetectionsStreamName, true);
      _faceRectStream = new OutputStream(calculatorGraph, _FaceRectStreamName, true);
      _faceLandmarksWithIrisStream = new OutputStream(calculatorGraph, _FaceLandmarksWithIrisStreamName, true);
      calculatorGraph.Initialize(config);
    }

    private PacketMap BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new PacketMap();
      SetImageTransformationOptions(sidePacket, imageSource);
      return sidePacket;
    }
  }
}
