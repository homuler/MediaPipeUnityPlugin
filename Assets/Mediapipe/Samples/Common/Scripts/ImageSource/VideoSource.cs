using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace Mediapipe.Unity {
  public class VideoSource : ImageSource {
    [SerializeField] VideoClip[] availableSources;

    public VideoClip video { get; private set; }

    public override SourceType type {
      get { return SourceType.Video; }
    }

    public override string sourceName {
      get { return video != null ? video.name : null; }
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
      get {
        if (video == null) {
          return null;
        }
        return new ResolutionStruct[] { new ResolutionStruct(video.width, video.height, video.frameRate) };
      }
    }

    void Start() {
      if (availableSources != null && availableSources.Length > 0) {
        video = availableSources[0];
      }
    }

    public override void SelectSource(int sourceId) {
      if (sourceId < 0 || sourceId >= availableSources.Length) {
        throw new ArgumentException($"Invalid source ID: {sourceId}");
      }

      video = availableSources[sourceId];
    }
  }
}
