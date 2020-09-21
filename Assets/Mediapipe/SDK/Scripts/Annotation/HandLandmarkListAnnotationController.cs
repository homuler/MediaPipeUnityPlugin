using System.Collections.Generic;

using Edge = System.Tuple<int ,int>;

namespace Mediapipe {
  public class HandLandmarkListAnnotationController : LandmarkListAnnotationController {
    protected static readonly IList<Edge> _Connections = new List<Edge> {
      new Edge(0, 1),
      new Edge(1, 2),
      new Edge(2, 3),
      new Edge(3, 4),
      new Edge(0, 5),
      new Edge(5, 9),
      new Edge(9, 13),
      new Edge(13, 17),
      new Edge(0, 17),
      new Edge(5, 6),
      new Edge(6, 7),
      new Edge(7, 8),
      new Edge(9, 10),
      new Edge(10, 11),
      new Edge(11, 12),
      new Edge(13, 14),
      new Edge(14, 15),
      new Edge(15, 16),
      new Edge(17, 18),
      new Edge(18, 19),
      new Edge(19, 20),
    };

    protected override IList<Edge> Connections {
      get { return _Connections; }
    }

    protected override int NodeSize {
      get { return 21; }
    }
  }
}
