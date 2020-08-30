using System.Runtime.InteropServices;
using UnityEngine;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public struct Landmark {
    public float x;
    public float y;
    public float z;
    public float visibility;

    public void Draw(Texture2D texture, Color color, int radius = 5) {
      var width = texture.width;
      var height = texture.height;

      int screenX = (int)(width * x);
      int screenY = (int)(height * y);

      texture.DrawCircle(screenX, screenY, color, radius);
    }
  }
}
