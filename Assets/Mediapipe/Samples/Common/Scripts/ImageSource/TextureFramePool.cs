using Mediapipe;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity {

  public class TextureFramePool : MonoBehaviour {
    [SerializeField] readonly int poolSize = 10;

    readonly object formatLock = new object();
    int textureWidth = 0;
    int textureHeight = 0;
    TextureFormat format = TextureFormat.RGBA32;

    Queue<TextureFrame> availableTextureFrames;
    /// <remarks>
    ///   key: TextureFrame's instance ID
    /// </remarks>
    Dictionary<Guid, TextureFrame> textureFramesInUse;

    /// <returns>
    ///   The total number of texture frames in the pool.
    /// </returns>
    public int frameCount {
      get {
        var availableTextureFramesCount = availableTextureFrames == null ? 0 : availableTextureFrames.Count;
        var textureFramesInUseCount = textureFramesInUse == null ? 0 : textureFramesInUse.Count;

        return availableTextureFramesCount + textureFramesInUseCount;
      }
    }

    void Start() {
      availableTextureFrames = new Queue<TextureFrame>(poolSize);
      textureFramesInUse = new Dictionary<Guid, TextureFrame>();
    }

    void OnDestroy() {
      lock (((ICollection)availableTextureFrames).SyncRoot) {
        availableTextureFrames.Clear();
        availableTextureFrames = null;
      }

      lock (((ICollection)textureFramesInUse).SyncRoot) {
        foreach (var textureFrame in textureFramesInUse.Values) {
          textureFrame.OnRelease.RemoveListener(OnTextureFrameRelease);
        }
        textureFramesInUse.Clear();
        textureFramesInUse = null;
      }
    }

    public void ResizeTexture(int textureWidth, int textureHeight, TextureFormat format) {
      lock (formatLock) {
        this.textureWidth = textureWidth;
        this.textureHeight = textureHeight;
        this.format = format;
      }
    }

    public void ResizeTexture(int textureWidth, int textureHeight) {
      ResizeTexture(textureWidth, textureHeight, format);
    }

    public WaitForResult<TextureFrame> WaitForNextTextureFrame(Action<TextureFrame> callback) {
      return new WaitForResult<TextureFrame>(this, YieldTextureFrame(callback));
    }

    void OnTextureFrameRelease(TextureFrame textureFrame) {
      lock (((ICollection)textureFramesInUse).SyncRoot) {
        if (!textureFramesInUse.Remove(textureFrame.GetInstanceId())) {
          // won't be run
          Debug.LogWarning("The released texture does not belong to the pool");
          return;
        }

        if (frameCount > poolSize || IsStale(textureFrame)) {
          return;
        }
        availableTextureFrames.Enqueue(textureFrame);
      }
    }

    bool IsStale(TextureFrame textureFrame) {
      lock(formatLock) {
        return textureFrame.width != textureWidth || textureFrame.height != textureHeight;
      }
    }

    TextureFrame CreateNewTextureFrame() {
      var textureFrame = new TextureFrame(textureWidth, textureHeight, format);
      textureFrame.OnRelease.AddListener(OnTextureFrameRelease);

      return textureFrame;
    }

    IEnumerator YieldTextureFrame(Action<TextureFrame> callback) {
      TextureFrame nextFrame = null;

      lock (((ICollection)availableTextureFrames).SyncRoot) {
        yield return new WaitUntil(() => {
          return poolSize > frameCount || availableTextureFrames.Count > 0;
        });

        if (poolSize <= frameCount) {
          while (availableTextureFrames.Count > 0) {
            var textureFrame = availableTextureFrames.Dequeue();

            if (!IsStale(textureFrame)) {
              nextFrame = textureFrame;
              break;
            }
          }
        }

        if (nextFrame == null) {
          nextFrame = CreateNewTextureFrame();
        }
      }

      lock(((ICollection)textureFramesInUse).SyncRoot) {
        textureFramesInUse.Add(nextFrame.GetInstanceId(), nextFrame);
      }

      nextFrame.WaitUntilReleased();
      callback(nextFrame);

      yield return nextFrame;
    }
  }
}
