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
      var result = default(Detection);

      Copy(proto, ref result);
      return result;
    }

    public static void Copy(Mediapipe.Detection proto, ref Detection destination)
    {
      var categories = destination.categories ?? new List<Category>(proto.Score.Count);
      categories.Clear();
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

    internal static void Copy(in NativeDetection source, ref Detection destination)
    {
      var categories = destination.categories ?? new List<Category>((int)source.categoriesCount);
      categories.Clear();
      foreach (var nativeCategory in source.categories)
      {
        categories.Add(new Category(nativeCategory));
      }

      var boundingBox = new Rect(source.boundingBox);

      var keypoints = destination.keypoints ?? new List<NormalizedKeypoint>((int)source.keypointsCount);
      keypoints.Clear();
      foreach (var nativeKeypoint in source.keypoints)
      {
        keypoints.Add(new NormalizedKeypoint(nativeKeypoint));
      }

      destination = new Detection(categories, boundingBox, keypoints);
    }

    public void CloneTo(ref Detection destination)
    {
      if (categories == null)
      {
        destination = default;
        return;
      }

      var dstCategories = destination.categories ?? new List<Category>(categories.Count);
      dstCategories.Clear();
      dstCategories.AddRange(categories);

      var dstKeypoints = destination.keypoints;
      if (keypoints != null)
      {
        dstKeypoints ??= new List<NormalizedKeypoint>(keypoints.Count);
        dstKeypoints.Clear();
        dstKeypoints.AddRange(keypoints);
      }

      destination = new Detection(dstCategories, boundingBox, dstKeypoints);
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

    public static DetectionResult Alloc(int capacity) => new DetectionResult(new List<Detection>(capacity));

    public static DetectionResult CreateFrom(List<Mediapipe.Detection> detectionsProto)
    {
      var result = default(DetectionResult);

      Copy(detectionsProto, ref result);
      return result;
    }

    public static void Copy(List<Mediapipe.Detection> source, ref DetectionResult destination)
    {
      var detections = destination.detections ?? new List<Detection>(source.Count);
      detections.ResizeTo(source.Count);

      for (var i = 0; i < source.Count; i++)
      {
        var detection = detections[i];
        Detection.Copy(source[i], ref detection);
        detections[i] = detection;
      }

      destination = new DetectionResult(detections);
    }

    internal static void Copy(NativeDetectionResult source, ref DetectionResult destination)
    {
      var detections = destination.detections ?? new List<Detection>((int)source.detectionsCount);
      detections.ResizeTo((int)source.detectionsCount);

      var i = 0;
      foreach (var nativeDetection in source.AsReadOnlySpan())
      {
        var detection = detections[i];
        Detection.Copy(in nativeDetection, ref detection);
        detections[i++] = detection;
      }

      destination = new DetectionResult(detections);
    }

    public void CloneTo(ref DetectionResult destination)
    {
      if (detections == null)
      {
        destination = default;
        return;
      }

      var dstDetections = destination.detections ?? new List<Detection>(detections.Count);
      dstDetections.CopyFrom(detections);

      destination = new DetectionResult(dstDetections);
    }

    public override string ToString() => $"{{ \"detections\": {Util.Format(detections)} }}";
  }
}
