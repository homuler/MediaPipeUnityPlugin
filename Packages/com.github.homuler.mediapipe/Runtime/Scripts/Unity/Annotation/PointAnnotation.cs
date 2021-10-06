using Mediapipe.Unity.CoordinateSystem;
using UnityEngine;

using mplt = global::Mediapipe.LocationData.Types;

namespace Mediapipe.Unity
{
  public class PointAnnotation : HierarchicalAnnotation
  {
    [SerializeField] Color color = Color.green;
    [SerializeField] float radius = 15.0f;

    void OnEnable()
    {
      ApplyColor(color);
      ApplyRadius(radius);
    }

    void OnDisable()
    {
      ApplyRadius(0.0f);
    }

    public void SetColor(Color color)
    {
      this.color = color;
      ApplyColor(color);
    }

    public void SetRadius(float radius)
    {
      this.radius = radius;
      ApplyRadius(radius);
    }

    public void Draw(Vector3 position)
    {
      SetActive(true); // Vector3 is not nullable
      transform.localPosition = position;
    }

    public void Draw(Landmark target, Vector3 scale, bool visualizeZ = true)
    {
      if (ActivateFor(target))
      {
        var position = GetAnnotationLayer().GetLocalPosition(target, scale, rotationAngle, isMirrored);
        if (!visualizeZ)
        {
          position.z = 0.0f;
        }
        transform.localPosition = position;
      }
    }

    public void Draw(NormalizedLandmark target, bool visualizeZ = true)
    {
      if (ActivateFor(target))
      {
        var position = GetAnnotationLayer().GetLocalPosition(target, rotationAngle, isMirrored);
        if (!visualizeZ)
        {
          position.z = 0.0f;
        }
        transform.localPosition = position;
      }
    }

    public void Draw(NormalizedPoint2D target)
    {
      if (ActivateFor(target))
      {
        var position = GetAnnotationLayer().GetLocalPosition(target, rotationAngle, isMirrored);
        transform.localPosition = position;
      }
    }

    public void Draw(Point3D target, Vector2 focalLength, Vector2 principalPoint, float zScale, bool visualizeZ = true)
    {
      if (ActivateFor(target))
      {
        var position = GetAnnotationLayer().GetLocalPosition(target, focalLength, principalPoint, zScale, rotationAngle, isMirrored);
        if (!visualizeZ)
        {
          position.z = 0.0f;
        }
        transform.localPosition = position;
      }
    }

    public void Draw(AnnotatedKeyPoint target, Vector2 focalLength, Vector2 principalPoint, float zScale, bool visualizeZ = true)
    {
      if (visualizeZ)
      {
        Draw(target?.Point3D, focalLength, principalPoint, zScale, true);
      }
      else
      {
        Draw(target?.Point2D);
      }
    }

    public void Draw(mplt.RelativeKeypoint target, float threshold = 0.0f)
    {
      if (ActivateFor(target))
      {
        Draw(GetAnnotationLayer().GetLocalPosition(target, rotationAngle, isMirrored));
        SetColor(GetColor(target.Score, threshold));
      }
    }

    void ApplyColor(Color color)
    {
      GetComponent<Renderer>().material.color = color;
    }

    void ApplyRadius(float radius)
    {
      transform.localScale = radius * Vector3.one;
    }

    Color GetColor(float score, float threshold)
    {
      var t = (score - threshold) / (1 - threshold);
      var h = Mathf.Lerp(90, 0, t) / 360; // from yellow-green to red
      return Color.HSVToRGB(h, 1, 1);
    }
  }
}
