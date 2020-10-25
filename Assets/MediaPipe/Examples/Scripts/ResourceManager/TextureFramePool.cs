using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureFramePool : MonoBehaviour {
  [SerializeField] int poolSize = 20;
  public int frameCount { get; private set; }
  private readonly object frameCountLock = new object();

  private int textureWidth = 0;
  private int textureHeight = 0;
  private readonly object dimensionLock = new object();
  private Queue<TextureFrame> availableTextureFrames;

  void Start() {
    frameCount = 0;
    availableTextureFrames = new Queue<TextureFrame>(poolSize);
  }

  public void SetDimension(int textureWidth, int textureHeight) {
    lock(dimensionLock) {
      this.textureWidth = textureWidth;
      this.textureHeight = textureHeight;
    }
  }

  public TextureFrameRequest RequestNextTextureFrame(Action<TextureFrame> callback) {
    return new TextureFrameRequest(this, callback);
  }

  public void OnTextureFrameReleased(TextureFrame textureFrame) {
    lock(frameCountLock) {
      if (frameCount > poolSize || IsStale(textureFrame)) {
        frameCount--;
        return;
      }

      availableTextureFrames.Enqueue(textureFrame);
    }
  }

  private bool IsStale(TextureFrame textureFrame) {
    lock(dimensionLock) {
      return textureFrame.width != textureWidth || textureFrame.height != textureHeight;
    }
  }

  private TextureFrame CreateNewTextureFrame() {
    lock(dimensionLock) {
      return new TextureFrame(textureWidth, textureHeight);
    }
  }

  private IEnumerator WaitForTextureFrame(Action<TextureFrame> callback) {
    TextureFrame nextFrame = null;

    lock(((ICollection)availableTextureFrames).SyncRoot) {
      yield return new WaitUntil(() => {
        return poolSize > frameCount || availableTextureFrames.Count > 0;
      });

      lock(frameCountLock) {
        while (availableTextureFrames.Count > 0) {
          var textureFrame = availableTextureFrames.Dequeue();

          if (!IsStale(textureFrame)) {
            nextFrame = textureFrame;
            break;
          }

          frameCount--;
        }

        if (nextFrame == null) {
          nextFrame = CreateNewTextureFrame();
          frameCount++;
        }
      }
    }

    callback(nextFrame);
  }

  public class TextureFrameRequest : CustomYieldInstruction {
    public TextureFrame textureFrame { get; private set; }
    private TextureFramePool textureFramePool;

    public override bool keepWaiting {
      get { return textureFrame == null; }
    }

    public TextureFrameRequest(TextureFramePool textureFramePool, Action<TextureFrame> callback) : base() {
      textureFramePool.StartCoroutine(textureFramePool.WaitForTextureFrame((TextureFrame textureFrame) => {
        callback(textureFrame);

        this.textureFrame = textureFrame;
      }));
    }
  }
}
