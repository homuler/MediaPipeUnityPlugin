using UnityEngine;

namespace Mediapipe {
  public partial class NormalizedLandmark {
    // TODO: implement in Annotator class
    public void Draw(Texture2D texture, Color color, int radius = 3) {
      var width = texture.width;
      var height = texture.height;

      int screenX = (int)(width * X);
      int screenY = (int)(height * Y);

      // NOTE: input image is flipped
      texture.DrawCircle(width - screenX, height - screenY, color, radius);
    }
  }
}
