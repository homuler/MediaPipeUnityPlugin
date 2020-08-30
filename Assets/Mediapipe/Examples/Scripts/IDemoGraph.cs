using Mediapipe;
using UnityEngine;

public interface IDemoGraph {
  void Initialize();
  Status StartRun(SidePacket sidePacket);
  Status PushColor32(Color32[] colors, int width, int height);
  Color32[] FetchOutput();
  void RenderOutput(Texture2D texture, Color32[] pixelData);
}
