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
        resolution = new ResolutionStruct(_video.width, _video.height, _video.frameRate);
        outputTexture = new Texture2D((int)_video.width, (int)_video.height, format, false);
      }
    }
    Texture2D outputTexture;

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
        return new ResolutionStruct[] { new ResolutionStruct(video.width, video.height, video.frameRate) };
      }
    }

    public bool isPaused { get { return videoPlayer == null ? false : videoPlayer.isPaused; } }
    public bool isPlaying { get { return videoPlayer == null ? false : videoPlayer.isPlaying; } }
    public override bool isPrepared { get { return videoPlayer == null ? false : base.isPrepared && videoPlayer.isPrepared; } }

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
      yield return base.Play();
      textureFramePool.ResizeTexture((int)video.width, (int)video.height, format);

      InitializeVideoPlayer();
      yield return new WaitUntil(() => isPrepared);
      videoPlayer.Play();
    }

    public override IEnumerator Resume() {
      if (!isPrepared) {
        throw new InvalidOperationException("VideoPlayer is not prepared yet");
      }
      if (isPlaying) {
        throw new InvalidOperationException("VideoPlayer is already playing");
      }
      yield return base.Resume();
      videoPlayer.Play();
      yield return new WaitUntil(() => isPrepared);
    }

    public override void Pause() {
      if (!isPlaying) {
        throw new InvalidOperationException("VideoPlayer is not playing videos");
      }
      base.Pause();
      videoPlayer.Pause();
    }

    public override void Stop() {
      if (!isPlaying) {
        throw new InvalidOperationException("VideoPlayer is not playing videos");
      }
      base.Stop();
      videoPlayer.Stop();
      GameObject.Destroy(gameObject.GetComponent<VideoPlayer>());
      videoPlayer = null;
    }

    protected override Texture2D GetCurrentTexture() {
      Graphics.CopyTexture(videoPlayer.texture, outputTexture);
      return outputTexture;
    }

    void InitializeVideoPlayer() {
      videoPlayer = gameObject.AddComponent<VideoPlayer>();
      videoPlayer.renderMode = VideoRenderMode.APIOnly;
      videoPlayer.isLooping = true;
      videoPlayer.clip = video;
      videoPlayer.Prepare();
    }
  }
}
