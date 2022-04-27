// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class StaticImageSource : ImageSource
  {
    [SerializeField] private Texture[] _availableSources;

    [SerializeField]
    private ResolutionStruct[] _defaultAvailableResolutions = new ResolutionStruct[] {
      new ResolutionStruct(512, 512, 0),
      new ResolutionStruct(640, 480, 0),
      new ResolutionStruct(1280, 720, 0),
    };

    private Texture2D _outputTexture;
    private Texture _image;
    private Texture image
    {
      get
      {
        if (_image == null && _availableSources != null && _availableSources.Length > 0)
        {
          image = _availableSources[0];
        }
        return _image;
      }
      set
      {
        _image = value;
        resolution = GetDefaultResolution();
      }
    }

    public override double frameRate => 0;

    public override string sourceName => image != null ? image.name : null;

    public override string[] sourceCandidateNames => _availableSources?.Select(source => source.name).ToArray();

    public override ResolutionStruct[] availableResolutions => _defaultAvailableResolutions;

    public override bool isPrepared => _outputTexture != null;

    private bool _isPlaying = false;
    public override bool isPlaying => _isPlaying;

    public override void SelectSource(int sourceId)
    {
      if (sourceId < 0 || sourceId >= _availableSources.Length)
      {
        throw new ArgumentException($"Invalid source ID: {sourceId}");
      }

      image = _availableSources[sourceId];
    }

    public override IEnumerator Play()
    {
      if (image == null)
      {
        throw new InvalidOperationException("Image is not selected");
      }
      if (isPlaying)
      {
        yield break;
      }

      InitializeOutputTexture(image);
      _isPlaying = true;
      yield return null;
    }

    public override IEnumerator Resume()
    {
      if (!isPrepared)
      {
        throw new InvalidOperationException("Image is not prepared");
      }
      _isPlaying = true;

      yield return null;
    }

    public override void Pause()
    {
      _isPlaying = false;
    }
    public override void Stop()
    {
      _isPlaying = false;
      _outputTexture = null;
    }

    public override Texture GetCurrentTexture()
    {
      return _outputTexture;
    }

    private ResolutionStruct GetDefaultResolution()
    {
      var resolutions = availableResolutions;

      return (resolutions == null || resolutions.Length == 0) ? new ResolutionStruct() : resolutions[0];
    }

    private void InitializeOutputTexture(Texture src)
    {
      _outputTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);

      Texture resizedTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
      // TODO: assert ConvertTexture finishes successfully
      var _ = Graphics.ConvertTexture(src, resizedTexture);

      var currentRenderTexture = RenderTexture.active;
      var tmpRenderTexture = new RenderTexture(resizedTexture.width, resizedTexture.height, 32);
      Graphics.Blit(resizedTexture, tmpRenderTexture);
      RenderTexture.active = tmpRenderTexture;

      var rect = new UnityEngine.Rect(0, 0, _outputTexture.width, _outputTexture.height);
      _outputTexture.ReadPixels(rect, 0, 0);
      _outputTexture.Apply();

      RenderTexture.active = currentRenderTexture;
    }
  }
}
