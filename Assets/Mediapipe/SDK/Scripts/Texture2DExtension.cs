using UnityEngine;

public static class Texture2DExtension {
  public static void DrawCircle(this Texture2D texture, int x, int y, Color color, int radius = 3) {
    int rSquared = radius * radius;

    for (int i = x - radius; i <= x + radius; i++) {
      for (int j = y - radius; j <= y + radius; j++) {
        if ((x - i) * (x - i) + (y - j) * (y - j) < rSquared) {
          texture.SetPixel(i, j, color);
        }
      }
    }
  }
}
