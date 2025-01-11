// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.ComponentModel;

namespace Mediapipe.Unity.Sample.FaceDetection
{
  public enum ModelType : int
  {
    [Description("BlazeFace (short-range)")]
    BlazeFaceShortRange = 0,
  }

  public class FaceDetectionConfig
  {
    public Tasks.Core.BaseOptions.Delegate Delegate { get; set; } =
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
      Tasks.Core.BaseOptions.Delegate.CPU;
#else
    Tasks.Core.BaseOptions.Delegate.GPU;
#endif

    public ImageReadMode ImageReadMode { get; set; } = ImageReadMode.CPUAsync;

    public ModelType Model { get; set; } = ModelType.BlazeFaceShortRange;

    public Tasks.Vision.Core.RunningMode RunningMode { get; set; } = Tasks.Vision.Core.RunningMode.LIVE_STREAM;

    public float MinDetectionConfidence { get; set; } = 0.5f;

    public float MinSuppressionThreshold { get; set; } = 0.3f;

    public int NumFaces { get; set; } = 3;

    public string ModelName => Model.GetDescription() ?? Model.ToString();
    public string ModelPath
    {
      get
      {
        switch (Model)
        {
          case ModelType.BlazeFaceShortRange:
            return "blaze_face_short_range.bytes";
          default:
            return null;
        }
      }
    }

    public Tasks.Vision.FaceDetector.FaceDetectorOptions GetFaceDetectorOptions(Tasks.Vision.FaceDetector.FaceDetectorOptions.ResultCallback resultCallback = null)
    {
      return new Tasks.Vision.FaceDetector.FaceDetectorOptions(
        new Tasks.Core.BaseOptions(Delegate, modelAssetPath: ModelPath),
        runningMode: RunningMode,
        minDetectionConfidence: MinDetectionConfidence,
        minSuppressionThreshold: MinSuppressionThreshold,
        numFaces: NumFaces,
        resultCallback: resultCallback
      );
    }
  }
}
