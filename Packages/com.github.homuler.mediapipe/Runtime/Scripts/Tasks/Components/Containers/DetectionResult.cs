// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections.Generic;

namespace Mediapipe.Tasks.Components.Containers
{
  /// <summary>
  ///   Represents one detected object in the object detector's results.
  /// </summary>
  public readonly struct Detection
  {
    private const int _DefaultCategoryIndex = -1;

    /// <summary>
    ///   A list of <see cref="Category" /> objects.
    /// </summary>
    public readonly IReadOnlyList<Category> categories;
    /// <summary>
    ///   The bounding box location.
    /// </summary>
    public readonly Rect boundingBox;
    /// <summary>
    ///   Optional list of keypoints associated with the detection. Keypoints
    ///   represent interesting points related to the detection. For example, the
    ///   keypoints represent the eye, ear and mouth from face detection model. Or
    ///   in the template matching detection, e.g. KNIFT, they can represent the
    ///   feature points for template matching.
    /// </summary>
    public readonly IReadOnlyList<NormalizedKeypoint> keypoints;

    internal Detection(IReadOnlyList<Category> categories, Rect boundingBox, IReadOnlyList<NormalizedKeypoint> keypoints)
    {
      this.categories = categories;
      this.boundingBox = boundingBox;
      this.keypoints = keypoints;
    }

    public static Detection CreateFrom(Mediapipe.Detection proto)
    {
      var categories = new List<Category>(proto.Score.Count);
      for (var idx = 0; idx < proto.Score.Count; idx++)
      {
        categories.Add(new Category(
          proto.LabelId.Count > idx ? proto.LabelId[idx] : _DefaultCategoryIndex,
          proto.Score[idx],
          proto.Label.Count > idx ? proto.Label[idx] : "",
          proto.DisplayName.Count > idx ? proto.DisplayName[idx] : ""
        ));
      }

      var boundingBox = proto.LocationData != null ? new Rect(
        proto.LocationData.BoundingBox.Xmin,
        proto.LocationData.BoundingBox.Ymin,
        proto.LocationData.BoundingBox.Xmin + proto.LocationData.BoundingBox.Width,
        proto.LocationData.BoundingBox.Ymin + proto.LocationData.BoundingBox.Height
      ) : new Rect(0, 0, 0, 0);

      List<NormalizedKeypoint> keypoints = null;
      if (proto.LocationData.RelativeKeypoints.Count > 0)
      {
        keypoints = new List<NormalizedKeypoint>(proto.LocationData.RelativeKeypoints.Count);
        foreach (var keypoint in proto.LocationData.RelativeKeypoints)
        {
          keypoints.Add(new NormalizedKeypoint(
            keypoint.X,
            keypoint.Y,
            keypoint.HasKeypointLabel ? keypoint.KeypointLabel : null,
#pragma warning disable IDE0004 // for Unity 2020.3.x
            keypoint.HasScore ? (float?)keypoint.Score : null
#pragma warning restore IDE0004
          ));
        }
      }

      return new Detection(categories, boundingBox, keypoints);
    }

    public override string ToString()
      => $"{{ \"categories\": {Util.Format(categories)}, \"boundingBox\": {boundingBox}, \"keypoints\": {Util.Format(keypoints)} }}";
  }

  /// <summary>
  ///   Represents the list of detected objects.
  /// </summary>
  public readonly struct DetectionResult
  {
    /// <summary>
    ///   A list of <see cref="Detection" /> objects.
    /// </summary>
    public readonly IReadOnlyList<Detection> detections;

    internal DetectionResult(IReadOnlyList<Detection> detections)
    {
      this.detections = detections;
    }

    public static DetectionResult CreateFrom(IReadOnlyList<Mediapipe.Detection> detectionsProto)
    {
      var detections = new List<Detection>(detectionsProto.Count);
      foreach (var detectionProto in detectionsProto)
      {
        detections.Add(Detection.CreateFrom(detectionProto));
      }
      return new DetectionResult(detections);
    }

    public override string ToString() => $"{{ \"detections\": {Util.Format(detections)} }}";
  }
}
