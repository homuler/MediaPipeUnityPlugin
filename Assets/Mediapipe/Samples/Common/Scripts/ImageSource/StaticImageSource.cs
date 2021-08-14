using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Mediapipe.Unity {
  public class StaticImageSource : ImageSource {
    [SerializeField] Texture[] availableSources;
    [SerializeField] ResolutionStruct[] defaultAvailableResolutions;

    Texture2D outputTexture;
    Texture _image;
    Texture image {
      get { return _image; }
      set {
        _image = value;
        resolution = GetDefaultResolution();
      }
    }

    public override SourceType type {
      get { return SourceType.Image; }
    }

    public override double frameRate { get { return 0; } }

    public override string sourceName {
      get { return image != null ? image.name : null; }
    }

    public override string[] sourceCandidateNames {
      get {
        if (availableSources == null) {
          return null;
        }
        return availableSources.Select(source => source.name).ToArray();
      }
    }

    public override ResolutionStruct[] availableResolutions {
      get { return defaultAvailableResolutions; }
    }

    public override bool isPrepared {
      get { return base.isPrepared && outputTexture != null; }
    }

    void Start() {
      if (availableSources != null && availableSources.Length > 0) {
        image = availableSources[0];
      }
    }

    public override void SelectSource(int sourceId) {
      if (sourceId < 0 || sourceId >= availableSources.Length) {
        throw new ArgumentException($"Invalid source ID: {sourceId}");
      }

      image = availableSources[sourceId];
    }

    public override IEnumerator Play() {
      if (image == null) {
        throw new InvalidOperationException("Image is not selected");
      }
      var imageTexture = Texture2D.CreateExternalTexture(image.width, image.height, textureFormat, false, false, image.GetNativeTexturePtr());
      outputTexture = new Texture2D(textureWidth, textureHeight, textureFormat, false);
      Graphics.ConvertTexture(imageTexture, outputTexture);

      yield return base.Play();
    }

    protected override Texture2D GetCurrentTexture() {
      return outputTexture;
    }

    ResolutionStruct GetDefaultResolution() {
      var resolutions = availableResolutions;

      if (resolutions == null || resolutions.Length == 0) {
        return new ResolutionStruct();
      }

      return resolutions[0];
    }
  }
}
