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
  }
}
