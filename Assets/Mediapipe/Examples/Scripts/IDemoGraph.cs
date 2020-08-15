using Mediapipe;
using UnityEngine;

public interface IDemoGraph {
  Status PushColor32(Color32[] colors, int width, int height);
  Color32[] FetchOutput();
}
