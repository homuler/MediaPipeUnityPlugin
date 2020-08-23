using Mediapipe;
using UnityEngine;

public interface IDemoGraph {
  Status StartRun(SidePacket sidePacket);
  Status PushColor32(Color32[] colors, int width, int height);
  Color32[] FetchOutput();
}
