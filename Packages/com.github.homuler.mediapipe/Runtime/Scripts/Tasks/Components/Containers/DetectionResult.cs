// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
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
    public readonly List<Category> categories;
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
    public readonly List<NormalizedKeypoint> keypoints;

    internal Detection(List<Category> categories, Rect boundingBox, List<NormalizedKeypoint> keypoints)
    {
      this.categories = categories;
      this.boundingBox = boundingBox;
      this.keypoints = keypoints;
    }

    public static Detection CreateFrom(Mediapipe.Detection proto)
    {
      var categories = new List<Category>(proto.Score.Count);
      var keypointsCount = proto.LocationData.RelativeKeypoints.Count;
      var keypoints = keypointsCount > 0 ? new List<NormalizedKeypoint>(keypointsCount) : null;
      var detection = new Detection(categories, new Rect(), keypoints);

      Copy(proto, ref detection);
      return detection;
    }

    public static void Copy(Mediapipe.Detection proto, ref Detection destination)
    {
      var categories = destination.categories;
      categories.Clear();
      for (var idx = 0; idx < proto.Score.Count; idx++)
      {
        destination.categories.Add(new Category(
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

      if (proto.LocationData.RelativeKeypoints.Count == 0)
      {
        destination = new Detection(categories, boundingBox, null);
        return;
      }

      var keypoints = destination.keypoints ?? new List<NormalizedKeypoint>(proto.LocationData.RelativeKeypoints.Count);
      keypoints.Clear();
      for (var i = 0; i < proto.LocationData.RelativeKeypoints.Count; i++)
      {
        var keypoint = proto.LocationData.RelativeKeypoints[i];
        keypoints.Add(new NormalizedKeypoint(
          keypoint.X,
          keypoint.Y,
          keypoint.HasKeypointLabel ? keypoint.KeypointLabel : null,
#pragma warning disable IDE0004 // for Unity 2020.3.x
          keypoint.HasScore ? (float?)keypoint.Score : null
#pragma warning restore IDE0004
        ));
      }

      destination = new Detection(categories, boundingBox, keypoints);
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
    public readonly List<Detection> detections;

    internal DetectionResult(List<Detection> detections)
    {
      this.detections = detections;
    }

    public void Clear() => detections.Clear();

    public static DetectionResult Empty => Alloc(0);

    public static DetectionResult Alloc(int capacity) => new DetectionResult(new List<Detection>(capacity));

    internal static DetectionResult CreateFrom(List<Mediapipe.Detection> detectionsProto)
    {
      var result = Alloc(detectionsProto.Count);
      Copy(detectionsProto, ref result);
      return result;
    }

    internal static void Copy(List<Mediapipe.Detection> source, ref DetectionResult destination)
    {
      var detections = destination.detections;
      if (source.Count < detections.Count)
      {
        detections.RemoveRange(source.Count, detections.Count - source.Count);
      }
      var copyCount = Math.Min(source.Count, detections.Count);
      for (var i = 0; i < copyCount; i++)
      {
        var detection = detections[i];
        Detection.Copy(source[i], ref detection);
        detections[i] = detection;
      }

      for (var i = copyCount; i < source.Count; i++)
      {
        detections.Add(Detection.CreateFrom(source[i]));
      }
    }

    public override string ToString() => $"{{ \"detections\": {Util.Format(detections)} }}";
  }
}
