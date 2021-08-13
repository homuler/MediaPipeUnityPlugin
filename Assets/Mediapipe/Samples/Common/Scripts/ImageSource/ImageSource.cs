using System;
using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity {
  public abstract class ImageSource : MonoBehaviour {
    [System.Serializable]
    public struct ResolutionStruct {
      public uint width;
      public uint height;
      public double frameRate;

      public ResolutionStruct(uint width, uint height, double frameRate) {
        this.width = width;
        this.height = height;
        this.frameRate = frameRate;
      }

      public ResolutionStruct(Resolution resolution) {
        this.width = (uint)resolution.width;
        this.height = (uint)resolution.height;
        this.frameRate = resolution.refreshRate;
      }

      public Resolution ToResolution() {
        var resolution = new Resolution();

        resolution.width = (int)width;
        resolution.height = (int)height;
        resolution.refreshRate = (int)frameRate;

        return resolution;
      }

      public override string ToString() {
        var aspectRatio = $"{width}x{height}";
        return frameRate > 0 ? $"{aspectRatio} ({frameRate.ToString("#.##")}Hz)" : aspectRatio;
      }
    }

    public enum Rotation {
      Rotation0 = 0,
      Rotation90 = 90,
      Rotation180 = 180,
      Rotation270 = 270,
    }

    public enum SourceType {
      Camera = 0,
      Image = 1,
      Video = 2,
    }

    protected TextureFramePool textureFramePool { get; private set; }
    public ResolutionStruct resolution { get; protected set; }
    public Rotation rotation = Rotation.Rotation0;
    public bool isFlipped = false;

    // TODO: make it selectable
    TextureFormat _format = TextureFormat.RGBA32;
    public TextureFormat format { get { return _format; } }

    public uint width {
      get { return resolution.width; }
    }

    public uint height {
      get { return resolution.height; }
    }

    public double frameRate {
      get { return resolution.frameRate; }
    }

    public float focalLengthPx {
      get { return 2.0f; }
    }

    public abstract SourceType type { get; }
    public abstract string sourceName { get; }
    public abstract string[] sourceCandidateNames { get; }
    public abstract ResolutionStruct[] availableResolutions { get; }
    public virtual bool isPrepared { get { return textureFramePool != null; } }

    public abstract void SelectSource(int sourceId);

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
