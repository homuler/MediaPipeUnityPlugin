using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe {
  public class DetectionListAnnotationController : ListAnnotationController<DetectionAnnotationController> {
    /// <summary>
    ///   Renders detections on a screen.
    ///   It is assumed that the screen is vertical to terrain and not inverted.
    /// </summary>
    /// <param name="isFlipped">
    ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
    /// </param>
    /// <remarks>
    ///   In <paramref name="detections" />, y-axis is oriented from top to bottom.
    /// </remarks>
    public void Draw(Transform screenTransform, List<Detection> detections, bool isFlipped = false) {
      var drawingCount = Mathf.Min(detections.Count, MaxSize);

      for (var i = 0; i < drawingCount; i++) {
        GetAnnotationControllerAt(i).Draw(screenTransform, detections[i], isFlipped);
      }

      ClearAll(drawingCount);
    }
  }
}
