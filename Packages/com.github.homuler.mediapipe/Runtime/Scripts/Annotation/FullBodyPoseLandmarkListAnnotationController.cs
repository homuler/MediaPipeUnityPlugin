using System.Collections.Generic;

using Edge = System.Tuple<int ,int>;

namespace Mediapipe {
  public class FullBodyPoseLandmarkListAnnotationController : LandmarkListAnnotationController {
    protected static readonly IList<Edge> _Connections = new List<Edge> {
      // Right Arm
      new Edge(11, 13),
      new Edge(13, 15),
      // Left Arm
      new Edge(12, 14),
      new Edge(14, 16),
      // Torso
      new Edge(11, 12),
      new Edge(12, 24),
      new Edge(24, 23),
      new Edge(23, 11),
      // Right Leg
      new Edge(23, 25),
      new Edge(25, 27),
      new Edge(27, 29),
      new Edge(29, 31),
      new Edge(31, 27),
      // Left Leg
      new Edge(24, 26),
      new Edge(26, 28),
      new Edge(28, 30),
      new Edge(30, 32),
      new Edge(32, 28),
    };

    protected override IList<Edge> Connections {
      get { return _Connections; }
    }

    protected override int NodeSize {
      get { return 33; }
    }
  }
}
