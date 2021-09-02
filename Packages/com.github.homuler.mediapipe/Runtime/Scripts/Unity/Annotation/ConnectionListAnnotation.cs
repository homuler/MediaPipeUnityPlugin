using System.Collections.Generic;
using System.Linq;

namespace Mediapipe.Unity {
  public sealed class ConnectionListAnnotation : LineListAnnotation<ConnectionAnnotation>, IAnnotatable<IList<Connection>> {
    public void Fill(IList<(int, int)> connections, PointListAnnotation points) {
      Draw(connections.Select(pair => new Connection(points[pair.Item1], points[pair.Item2])).ToList());
    }

    public void Draw(IList<Connection> targets) {
      if (ActivateFor(targets)) {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target); });
      }
    }

    public void Redraw() {
      foreach (var connection in children) {
        connection?.Redraw();
      }
    }
  }
}
