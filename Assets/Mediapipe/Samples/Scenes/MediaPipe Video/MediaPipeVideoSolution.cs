using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.MediaPipeVideo {
  public class MediaPipeVideoSolution : Solution {
    [SerializeField] RawImage screen;
    [SerializeField] MediaPipeVideoGraph graphRunner;
    [SerializeField] TextureFramePool textureFramePool;

    Texture2D outputTexture;
    static ImageFrame currentOutput;
    Color32[] outputBuffer;

    Coroutine coroutine;

    public RunningMode runningMode;

    public long timeoutMillisec {
      get { return graphRunner.timeoutMillisec; }
      set { graphRunner.SetTimeoutMillisec(value); }
    }

    public override void Play() {
      if (coroutine != null) {
        Stop();
      }
      base.Play();
      coroutine = StartCoroutine(Run());
    }

    public override void Pause() {
      base.Pause();
      ImageSourceProvider.imageSource.Pause();
    }

    public override void Resume() {
      base.Resume();
      StartCoroutine(ImageSourceProvider.imageSource.Resume());
    }

    public override void Stop() {
      base.Stop();
      StopCoroutine(coroutine);
      ImageSourceProvider.imageSource.Stop();
      graphRunner.Stop();
    }

    void Update() {
      if (currentOutput != null) {
        var outputVideo = currentOutput;
        currentOutput = null;

        DrawNow(outputVideo);
        outputVideo.Dispose();
      }
    }

    IEnumerator Run() {
      var graphInitRequest = graphRunner.WaitForInit();
      var imageSource = ImageSourceProvider.imageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared) {
        Logger.LogError(TAG, "Failed to start ImageSource, exiting...");
        yield break;
      }
      // NOTE: The screen will be resized later, keeping the aspect ratio.
      screen.rectTransform.sizeDelta = new Vector2(imageSource.textureWidth, imageSource.textureHeight);
      screen.rectTransform.localEulerAngles = imageSource.rotation.Reverse().GetEulerAngles();

      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);

      // Setup output texture
      if (graphRunner.configType == GraphRunner.ConfigType.OpenGLES) {
        var textureFrameRequest = textureFramePool.WaitForNextTextureFrame();
        yield return textureFrameRequest;
        var outputTexture = textureFrameRequest.result;

        // Exclude from TextureFramePool
        outputTexture.RemoveAllReleaseListeners();
        graphRunner.SetupOutputPacket(outputTexture);

        screen.texture = Texture2D.CreateExternalTexture(outputTexture.width, outputTexture.height, outputTexture.format, false, false, outputTexture.GetNativeTexturePtr());
      } else {
        outputTexture = new Texture2D(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, false);
        screen.texture = outputTexture;
        outputBuffer = new Color32[imageSource.textureWidth * imageSource.textureHeight];
      }

      yield return graphInitRequest;
      if (graphInitRequest.isError) {
        Logger.LogError(TAG, graphInitRequest.error);
        yield break;
      }

      if (runningMode == RunningMode.Async) {
        graphRunner.OnOutput.AddListener(OnOutput);
        graphRunner.StartRunAsync(imageSource).AssertOk();
      } else {
        graphRunner.StartRun(imageSource).AssertOk();
      }

      while (true) {
        yield return new WaitWhile(() => isPaused);

        var textureFrameRequest = textureFramePool.WaitForNextTextureFrame();
        yield return textureFrameRequest;
        var textureFrame = textureFrameRequest.result;

        // Copy current image to TextureFrame
        ReadFromImageSource(textureFrame, runningMode, graphRunner.configType);

        graphRunner.AddTextureFrameToInputStream(textureFrame).AssertOk();

        if (runningMode == RunningMode.Sync && graphRunner.configType != GraphRunner.ConfigType.OpenGLES) {
          // When running synchronously, wait for the outputs here (blocks the main thread).
          var output = graphRunner.FetchNextValue();
          DrawNow(output);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    void OnOutput(ImageFrame outputVideo) {
      if (outputVideo != null) {
        currentOutput = outputVideo;
      }
    }

    void DrawNow(ImageFrame imageFrame) {
      outputTexture.LoadRawTextureData(imageFrame.MutablePixelData(), imageFrame.PixelDataSize());
      outputTexture.Apply();
    }
  }
}
