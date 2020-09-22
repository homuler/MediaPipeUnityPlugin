using System.Collections.Generic;

using Edge = System.Tuple<int ,int>;

namespace Mediapipe {
  public class PoseLandmarkListAnnotationController : LandmarkListAnnotationController {
    protected static readonly IList<Edge> _Connections = new List<Edge> {
      // Face
      new Edge(0, 1),
      new Edge(1, 2),
      new Edge(2, 3),
      new Edge(3, 7),
      new Edge(0, 4),
      new Edge(4, 5),
      new Edge(5, 6),
      new Edge(6, 8),
      new Edge(9, 10),
      // Right Arm
      new Edge(11, 13),
      new Edge(13, 15),
      new Edge(15, 17),
      new Edge(17, 19),
      new Edge(19, 15),
      new Edge(15, 21),
      // Left Arm
      new Edge(12, 14),
      new Edge(14, 16),
      new Edge(16, 18),
      new Edge(18, 20),
      new Edge(20, 16),
      new Edge(16, 22),
      // Torso
      new Edge(11, 12),
      new Edge(12, 24),
      new Edge(24, 23),
      new Edge(23, 11),
    };

    protected override IList<Edge> Connections {
      get { return _Connections; }
    }

    protected override int NodeSize {
      get { return 25; }
    }
  }
}
