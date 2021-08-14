using System;
using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity {
  public abstract class ImageSource : MonoBehaviour {
    [System.Serializable]
    public struct ResolutionStruct {
      public int width;
      public int height;
      public double frameRate;

      public ResolutionStruct(int width, int height, double frameRate) {
        this.width = width;
        this.height = height;
        this.frameRate = frameRate;
      }

      public ResolutionStruct(Resolution resolution) {
        this.width = resolution.width;
        this.height = resolution.height;
        this.frameRate = resolution.refreshRate;
      }

      public Resolution ToResolution() {
        var resolution = new Resolution();

        resolution.width = width;
        resolution.height = height;
        resolution.refreshRate = (int)frameRate;

        return resolution;
      }

      public override string ToString() {
        var aspectRatio = $"{width}x{height}";
        return frameRate > 0 ? $"{aspectRatio} ({frameRate.ToString("#.##")}Hz)" : aspectRatio;
      }
    }

    public enum SourceType {
      Camera = 0,
      Image = 1,
      Video = 2,
    }

    protected TextureFramePool textureFramePool { get; private set; }

    ResolutionStruct _resolution;
    public virtual ResolutionStruct resolution {
      get { return _resolution; }
      protected set {
        _resolution = value;

        if (textureFramePool != null) {
          textureFramePool.ResizeTexture(_resolution.width, _resolution.height);
        }
      }
    }

    // TODO: make it selectable
    TextureFormat _textureFormat = TextureFormat.RGBA32;
    public virtual TextureFormat textureFormat {
      get { return _textureFormat; }
      protected set {
        _textureFormat = value;

        if (textureFramePool != null) {
          textureFramePool.ResizeTexture(textureWidth, textureHeight, _textureFormat);
        }
      }
    }

    public virtual int textureWidth { get { return resolution.width; } }
    public virtual int textureHeight { get { return resolution.height; } }
    /// <remarks>
    ///   If <see cref="type" /> does not support frame rate, it returns zero.
    /// </remarks>
    public virtual double frameRate { get { return resolution.frameRate; } }
    public float focalLengthPx { get { return 2.0f; } } // TODO: calculate at runtime

    public abstract SourceType type { get; }
    public abstract string sourceName { get; }
    public abstract string[] sourceCandidateNames { get; }
    public abstract ResolutionStruct[] availableResolutions { get; }
    public virtual bool isPrepared { get { return textureFramePool != null; } }

    /// <summary>
    ///   Choose the source from <see cref="sourceCandidateNames" />.
    /// </summary>
    /// <remarks>
    ///   You need to call <see cref="Play" /> for the change to take effect.
    /// </remarks>
    /// <param name="sourceId">The index of <see cref="sourceCandidateNames" /></param>
    public abstract void SelectSource(int sourceId);

    /// <summary>
    ///   Choose the resolution from <see cref="availableResolutions" />.
    /// </summary>
    /// <remarks>
    ///   You need to call <see cref="Play" /> for the change to take effect.
    /// </remarks>
    /// <param name="resolutionId">The index of <see cref="availableResolutions" /></param>
    public void SelectResolution(int resolutionId) {
      var resolutions = availableResolutions;

      if (resolutionId < 0 || resolutionId >= resolutions.Length) {
        throw new ArgumentException($"Invalid resolution ID: {resolutionId}");
      }

      resolution = resolutions[resolutionId];
    }

    public virtual IEnumerator Play() {
      if (textureFramePool == null) {
        textureFramePool = gameObject.AddComponent<TextureFramePool>();
      }
      textureFramePool.ResizeTexture(textureWidth, textureHeight, textureFormat);
      yield return null;
    }

    public virtual IEnumerator Resume() {
      yield return null;
    }

    public virtual void Pause() {}

    public virtual void Stop() {
      if (textureFramePool != null) {
        GameObject.Destroy(gameObject.GetComponent<TextureFramePool>());
        textureFramePool = null;
      }
    }

    public WaitForResult<TextureFrame> WaitForNextTextureFrame() {
      return textureFramePool.WaitForNextTextureFrame((TextureFrame textureFrame) => {
        textureFrame.CopyTextureFrom(GetCurrentTexture());
      });
    }

    protected abstract Texture2D GetCurrentTexture();
  }
}
