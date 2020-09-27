using Mediapipe;
using UnityEngine;

public interface IDemoGraph {
  void Initialize();
  void Initialize(GpuResources gpuResources, GlCalculatorHelper gpuHelper);
  Status StartRun(SidePacket sidePacket);
  Status PushColor32(Color32[] colors, int width, int height);
  void RenderOutput(WebCamScreenController screen, Color32[] pixelData);
}
