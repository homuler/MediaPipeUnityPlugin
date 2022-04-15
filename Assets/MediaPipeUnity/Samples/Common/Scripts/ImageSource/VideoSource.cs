// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace Mediapipe.Unity
{
  public class VideoSource : ImageSource
  {
    [SerializeField] private VideoClip[] _availableSources;

    private VideoClip _video;
    private VideoClip video
    {
      get => _video;
      set
      {
        _video = value;
        resolution = new ResolutionStruct((int)_video.width, (int)_video.height, _video.frameRate);
      }
    }

    private VideoPlayer _videoPlayer;

    public override SourceType type => SourceType.Video;

    public override string sourceName => video != null ? video.name : null;

    public override string[] sourceCandidateNames => _availableSources?.Select(source => source.name).ToArray();

    public override ResolutionStruct[] availableResolutions => video == null ? null : new ResolutionStruct[] { new ResolutionStruct((int)video.width, (int)video.height, video.frameRate) };

    public override bool isPlaying => _videoPlayer != null && _videoPlayer.isPlaying;
    public override bool isPrepared => _videoPlayer != null && _videoPlayer.isPrepared;

    private void Start()
    {
      if (_availableSources != null && _availableSources.Length > 0)
      {
        video = _availableSources[0];
      }
    }

    public override void SelectSource(int sourceId)
    {
      if (sourceId < 0 || sourceId >= _availableSources.Length)
      {
        throw new ArgumentException($"Invalid source ID: {sourceId}");
      }

      video = _availableSources[sourceId];
      if (_videoPlayer != null)
      {
        _videoPlayer.clip = video;
      }
    }

    public override IEnumerator Play()
    {
      if (video == null)
      {
        throw new InvalidOperationException("Video is not selected");
      }
      _videoPlayer = gameObject.AddComponent<VideoPlayer>();
      _videoPlayer.renderMode = VideoRenderMode.APIOnly;
      _videoPlayer.isLooping = true;
      _videoPlayer.clip = video;
      _videoPlayer.Prepare();

      yield return new WaitUntil(() => _videoPlayer.isPrepared);
      _videoPlayer.Play();
    }

    public override IEnumerator Resume()
    {
      if (!isPrepared)
      {
        throw new InvalidOperationException("VideoPlayer is not prepared");
      }
      if (!isPlaying)
      {
        _videoPlayer.Play();
      }
      yield return null;
    }

    public override void Pause()
    {
      if (!isPlaying)
      {
        return;
      }
      _videoPlayer.Pause();
    }

    public override void Stop()
    {
      if (_videoPlayer == null)
      {
        return;
      }
      _videoPlayer.Stop();
      Destroy(gameObject.GetComponent<VideoPlayer>());
      _videoPlayer = null;
    }

    public override Texture GetCurrentTexture()
    {
      return _videoPlayer != null ? _videoPlayer.texture : null;
    }
  }
}
