// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe
{
  public static class ProtoMessageClearExtension
  {
    public static void Clear(this Detection detection)
    {
      detection.Label.Clear();
      detection.LabelId.Clear();
      detection.Score.Clear();

      detection.LocationData.ClearFormat();
      if (detection.LocationData.BoundingBox != null)
      {
        detection.LocationData.BoundingBox.ClearXmin();
        detection.LocationData.BoundingBox.ClearYmin();
        detection.LocationData.BoundingBox.ClearWidth();
        detection.LocationData.BoundingBox.ClearHeight();
      }
      if (detection.LocationData.RelativeBoundingBox != null)
      {
        detection.LocationData.RelativeBoundingBox.ClearXmin();
        detection.LocationData.RelativeBoundingBox.ClearYmin();
        detection.LocationData.RelativeBoundingBox.ClearWidth();
        detection.LocationData.RelativeBoundingBox.ClearHeight();
      }
      if (detection.LocationData.Mask != null)
      {
        detection.LocationData.Mask.ClearWidth();
        detection.LocationData.Mask.ClearHeight();
        detection.LocationData.Mask.Rasterization.Interval.Clear();
      }
      detection.LocationData.RelativeKeypoints.Clear();

      detection.ClearFeatureTag();
      detection.ClearTrackId();
      detection.ClearDetectionId();
      detection.AssociatedDetections.Clear();
      detection.DisplayName.Clear();
      detection.ClearTimestampUsec();
    }
  }
}
