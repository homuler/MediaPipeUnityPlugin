using Mediapipe;
using UnityEngine;

public interface IDemoGraph<T> {
  /// <summary>
  ///   This method must be called (only) once before calling StartRun.
  ///   CalculatorGraph should be initialized here.
  /// </summary>
  void Initialize();

  /// <summary>
  ///   Initialize the graph with GPU enabled.
  /// </summary>
  void Initialize(GpuResources gpuResources, GlCalculatorHelper gpuHelper);

  /// <summary>
  ///   This method must be called (only) once before starting to process images.
  ///   At least, `CalculatorGraph#StartRun` must be called here.
  ///   It is also necessary to initialize OutputStreamPollers.
  /// </summary>
  Status StartRun();
  Status StartRun(Texture texture);

  Status PushInput(T input);

  /// <summary>
  ///   Fetch output packets and render the result.
  /// </summary>
  /// <param name="screenController">Controller of the screen where the result is rendered</param>
  /// <param name="input">
  ///   Input data that is already sent to an input stream.
  ///   Its timestamp should correspond to that of the next output packet (if exists).
  /// </param>
  void RenderOutput(WebCamScreenController screen, T input);

  void Stop();
}
