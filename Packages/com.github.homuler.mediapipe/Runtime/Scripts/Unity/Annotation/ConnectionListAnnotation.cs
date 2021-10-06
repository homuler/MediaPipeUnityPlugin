using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mediapipe.Unity
{
  public sealed class ConnectionListAnnotation : ListAnnotation<ConnectionAnnotation>
  {
    [SerializeField] Color color = Color.red;
    [SerializeField, Range(0, 1)] float lineWidth = 1.0f;

    void OnValidate()
    {
      ApplyColor(color);
      ApplyLineWidth(lineWidth);
    }

    public void Fill(IList<(int, int)> connections, PointListAnnotation points)
    {
      Draw(connections.Select(pair => new Connection(points[pair.Item1], points[pair.Item2])).ToList());
    }

    public void SetColor(Color color)
    {
      this.color = color;
      ApplyColor(color);
    }

    public void SetLineWidth(float lineWidth)
    {
      this.lineWidth = lineWidth;
      ApplyLineWidth(lineWidth);
    }

    public void Draw(IList<Connection> targets)
    {
      if (ActivateFor(targets))
      {
        CallActionForAll(targets, (annotation, target) => { annotation?.Draw(target); });
      }
    }

    public void Redraw()
    {
      foreach (var connection in children)
      {
        connection?.Redraw();
      }
    }

    protected override ConnectionAnnotation InstantiateChild(bool isActive = true)
    {
      var annotation = base.InstantiateChild(isActive);
      annotation.SetColor(color);
      annotation.SetLineWidth(lineWidth);
      return annotation;
    }

    void ApplyColor(Color color)
    {
      foreach (var line in children)
      {
        line?.SetColor(color);
      }
    }

    void ApplyLineWidth(float lineWidth)
    {
      foreach (var line in children)
      {
        line?.SetLineWidth(lineWidth);
      }
    }
  }
}
