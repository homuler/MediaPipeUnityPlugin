using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe {
  public class DetectionListAnnotationController : MonoBehaviour {
    [SerializeField] GameObject detectionPrefab = null;
    [SerializeField] int DefaultMaxSize = 1;

    private List<GameObject> detections;
    private int MaxSize;

    void Awake() {
      MaxSize = DefaultMaxSize;
      detections = new List<GameObject>(MaxSize);

      for (var i = 0; i < DefaultMaxSize; i++) {
        detections.Add(Instantiate(detectionPrefab));
      }
    }

    public void UpdateMaxSize(int MaxSize) {
      if (MaxSize < this.MaxSize) {
        for (var i = MaxSize; i < this.MaxSize; i++) {
          Destroy(detections[i]);
        }

        detections.RemoveRange(MaxSize + 1, this.MaxSize - MaxSize);
      } else {
        for (var i = 0; i < MaxSize - this.MaxSize; i++) {
          detections.Add(Instantiate(detectionPrefab));
        }
      }

      this.MaxSize = MaxSize;
    }

    public void Clear() {
      foreach (var detection in detections) {
        detection.GetComponent<DetectionAnnotationController>().Clear();
      }
    }

    /// <summary>
    ///   Renders bounding boxes on a screen.
    ///   It is assumed that the screen is vertical to terrain and not inverted.
    /// </summary>
    /// <param name="isFlipped">
    ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
    /// </param>
    /// <remarks>
    ///   In <paramref name="detectionList" />, y-axis is oriented from top to bottom.
    /// </remarks>
    public void Draw(Transform screenTransform, List<Detection> detectionList, bool isFlipped = false) {
      var drawingCount = Mathf.Min(MaxSize, detectionList.Count);

      for (var i = 0; i < drawingCount; i++) {
        var detection = detectionList[i];

        detections[i].GetComponent<DetectionAnnotationController>().Draw(screenTransform, detection, isFlipped);
      }

      for (var i = drawingCount; i < MaxSize; i++) {
        detections[i].GetComponent<DetectionAnnotationController>().Clear();
      }
    }
  }
}
