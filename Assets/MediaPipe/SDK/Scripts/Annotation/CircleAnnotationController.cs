using System.Linq;
using UnityEngine;

namespace Mediapipe {
  public class CircleAnnotationController : AnnotationController {
    [SerializeField] int PositionSize = 128;
    private Vector3[] emptyPositions;

    void Awake() {
      emptyPositions = Enumerable.Repeat(Vector3.zero, PositionSize).ToArray();
      gameObject.GetComponent<LineRenderer>().positionCount = PositionSize;
    }

    public override void Clear() {
      gameObject.GetComponent<LineRenderer>().SetPositions(emptyPositions);
    }

    /// <summary>
    ///   Renders a circle on a screen.
    ///   It is assumed that the screen is vertical to terrain and not inverted.
    /// </summary>
    /// <param name="isFlipped">
    ///   if true, x axis is oriented from right to left (top-right point is (0, 0) and bottom-left is (1, 1))
    /// </param>
    /// <remarks>
    ///   In <paramref name="center" />, y-axis is oriented from top to bottom.
    /// </remarks>
    public void Draw(Transform screenTransform, NormalizedLandmark center, float r, bool isFlipped = false) {
      var centerPos = GetPosition(screenTransform, center, isFlipped);
      var startPosRel = new Vector3(r, 0, 0);
      var positions = new Vector3[PositionSize];

      for (var i = 0; i < PositionSize; i++) {
        var q = Quaternion.Euler(0, 0, i * 360 / PositionSize);
        positions[i] = q * startPosRel + centerPos;
      }

      gameObject.GetComponent<LineRenderer>().SetPositions(positions);
    }
  }
}
