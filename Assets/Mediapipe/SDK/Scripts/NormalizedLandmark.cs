using UnityEngine;

namespace Mediapipe {
  public partial class NormalizedLandmark {
    public void Draw(Texture2D texture, Color color, int radius = 5) {
      var width = texture.width;
      var height = texture.height;

      int screenX = (int)(width * X);
      int screenY = (int)(height * Y);

      texture.DrawCircle(screenX, screenY, color, radius);
    }
  }
}
