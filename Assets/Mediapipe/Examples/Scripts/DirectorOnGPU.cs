using Mediapipe;
using UnityEngine;

public class DirectorOnGPU : Director {
  private DemoGraph calculatorGraph;

  [SerializeField] TextAsset config;

  protected override void Start() {
    base.Start();
  }

  void RunGraph(Color32[] pixelData, int width, int height) {}
}
