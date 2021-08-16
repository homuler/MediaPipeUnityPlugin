using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace Mediapipe.Unity {
  public class VideoSource : ImageSource {
    [SerializeField] VideoClip[] availableSources;

    VideoClip _video;
    VideoClip video {
      get { return _video; }
      set {
        _video = value;
        resolution = new ResolutionStruct((int)_video.width, (int)_video.height, _video.frameRate);
      }
    }

    VideoPlayer videoPlayer;

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
        return new ResolutionStruct[] { new ResolutionStruct((int)video.width, (int)video.height, video.frameRate) };
      }
    }

    public override bool isPlaying { get { return videoPlayer == null ? false : videoPlayer.isPlaying; } }
    public override bool isPrepared { get { return videoPlayer == null ? false : videoPlayer.isPrepared; } }

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
      if (videoPlayer != null) {
        videoPlayer.clip = video;
      }
    }

    public override IEnumerator Play() {
      if (video == null) {
        throw new InvalidOperationException("Video is not selected");
      }
      videoPlayer = gameObject.AddComponent<VideoPlayer>();
      videoPlayer.renderMode = VideoRenderMode.APIOnly;
      videoPlayer.isLooping = true;
      videoPlayer.clip = video;
      videoPlayer.Prepare();

      yield return new WaitUntil(() => videoPlayer.isPrepared);
      videoPlayer.Play();
    }

    public override IEnumerator Resume() {
      if (!isPrepared) {
        throw new InvalidOperationException("VideoPlayer is not prepared");
      }
      if (!isPlaying) {
        videoPlayer.Play();
      }
      yield return null;
    }

    public override void Pause() {
      if (!isPlaying) {
        return;
      }
      videoPlayer.Pause();
    }

    public override void Stop() {
      if (videoPlayer == null) {
        return;
      }
      videoPlayer.Stop();
      GameObject.Destroy(gameObject.GetComponent<VideoPlayer>());
      videoPlayer = null;
    }

    public override Texture GetCurrentTexture() {
      return videoPlayer != null ? videoPlayer.texture : null;
    }
  }
}
