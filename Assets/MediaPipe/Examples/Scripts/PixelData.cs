using UnityEngine;

public class PixelData {
  public readonly Color32[] Colors;
  public readonly int Width;
  public readonly int Height;

  public PixelData(Color32[] Colors, int Width, int Height) {
    this.Colors = Colors;
    this.Width = Width;
    this.Height = Height;
  }
}
