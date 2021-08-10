using System;
using System.Linq;
using UnityEngine;

namespace Mediapipe.Unity {
  public class StaticImageSource : ImageSource {
    [SerializeField] Texture[] availableSources;
    [SerializeField] ResolutionStruct[] defaultAvailableResolutions;

    public Texture image { get; private set; }

    public override SourceType type {
      get { return SourceType.Image; }
    }

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
  }
}
