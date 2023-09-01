// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Mediapipe.Unity
{
  public abstract class ImageSource
  {
    [Serializable]
    public struct ResolutionStruct
    {
      public int width;
      public int height;
      public double frameRate;

      public ResolutionStruct(int width, int height, double frameRate)
      {
        this.width = width;
        this.height = height;
        this.frameRate = frameRate;
      }

      public ResolutionStruct(Resolution resolution)
      {
        width = resolution.width;
        height = resolution.height;
        frameRate = resolution.refreshRate;
      }

      public Resolution ToResolution()
      {
        return new Resolution() { width = width, height = height, refreshRate = (int)frameRate };
      }

      public override string ToString()
      {
        var aspectRatio = $"{width}x{height}";
        var frameRateStr = frameRate.ToString("#.##");
        return frameRate > 0 ? $"{aspectRatio} ({frameRateStr}Hz)" : aspectRatio;
      }
    }

    public ResolutionStruct resolution { get; protected set; }

    /// <remarks>
    ///   To call this method, the image source must be prepared.
    /// </remarks>
    /// <returns>
    ///   <see cref="TextureFormat" /> that is compatible with the current texture.
    /// </returns>
    public TextureFormat textureFormat => isPrepared ? TextureFormatFor(GetCurrentTexture()) : throw new InvalidOperationException("ImageSource is not prepared");
    public virtual int textureWidth => resolution.width;
    public virtual int textureHeight => resolution.height;
    /// <remarks>
    ///   If <see cref="type" /> does not support frame rate, it returns zero.
    /// </remarks>
    public virtual double frameRate => resolution.frameRate;
    public float focalLengthPx { get; } = 2.0f; // TODO: calculate at runtime
    public virtual bool isHorizontallyFlipped { get; set; } = false;
    public virtual bool isVerticallyFlipped { get; } = false;
    public virtual bool isFrontFacing { get; } = false;
    public virtual RotationAngle rotation { get; } = RotationAngle.Rotation0;

    public abstract string sourceName { get; }
    public abstract string[] sourceCandidateNames { get; }
    public abstract ResolutionStruct[] availableResolutions { get; }

    /// <remarks>
    ///   Once <see cref="Play" /> finishes successfully, it will become true.
    /// </remarks>
    /// <returns>
    ///   Returns if the image source is prepared.
    /// </returns>
    public abstract bool isPrepared { get; }

    public abstract bool isPlaying { get; }

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
    public void SelectResolution(int resolutionId)
    {
      var resolutions = availableResolutions;

      if (resolutionId < 0 || resolutionId >= resolutions.Length)
      {
        throw new ArgumentException($"Invalid resolution ID: {resolutionId}");
      }

      resolution = resolutions[resolutionId];
    }

    /// <summary>
    ///   Prepare image source.
    ///   If <see cref="isPlaying" /> is true, it will reset the image source.
    /// </summary>
    /// <remarks>
    ///   When it finishes successfully, <see cref="isPrepared" /> will return true.
    /// </remarks>
    /// <exception cref="InvalidOperation" />
    public abstract IEnumerator Play();

    /// <summary>
    ///   Resume image source.
    ///   If <see cref="isPlaying" /> is true, it will do nothing.
    /// </summary>
    /// <exception cref="InvalidOperation">
    ///   The image source has not been played.
    /// </exception>
    public abstract IEnumerator Resume();

    /// <summary>
    ///   Pause image source.
    ///   If <see cref="isPlaying" /> is false, it will do nothing.
    /// </summary>
    public abstract void Pause();

    /// <summary>
    ///   Stop image source.
    /// </summary>
    /// <remarks>
    ///   When it finishes successfully, <see cref="isPrepared" /> will return false.
    /// </remarks>
    public abstract void Stop();

    /// <remarks>
    ///   To call this method, the image source must be prepared.
    /// </remarks>
    /// <returns>
    ///   <see cref="Texture" /> that contains the current image.
    /// </returns>
    public abstract Texture GetCurrentTexture();

    protected static TextureFormat TextureFormatFor(Texture texture)
    {
      return GraphicsFormatUtility.GetTextureFormat(texture.graphicsFormat);
    }
  }
}
