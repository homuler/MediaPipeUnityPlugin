using Mediapipe;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureFramePool : MonoSingleton<TextureFramePool> {
  [SerializeField] readonly int poolSize = 10;

  private readonly object dimensionLock = new object();
  private int textureWidth = 0;
  private int textureHeight = 0;

  private Queue<TextureFrame> availableTextureFrames;
  /// <remarks>
  ///   key: texture's native pointer (e.g. OpenGL texture name)
  /// </remarks>
  private Dictionary<UInt64, TextureFrame> textureFramesInUse;

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
    textureFramesInUse = new Dictionary<UInt64, TextureFrame>();
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

  [AOT.MonoPInvokeCallback(typeof(GlTextureBuffer.DeletionCallback))]
  private static void OnTextureFrameRelease(UInt64 textureName, IntPtr syncTokenPtr) {
    lock(((ICollection)Instance.textureFramesInUse).SyncRoot) {
      if (!Instance.textureFramesInUse.TryGetValue(textureName, out var textureFrame)) {
        Debug.LogWarning("The released texture does not belong to the pool");
        return;
      }

      Instance.textureFramesInUse.Remove(textureName);

      if (Instance.frameCount > Instance.poolSize || IsStale(textureFrame)) {
        return;
      }

      if (syncTokenPtr != IntPtr.Zero) {
        using (var glSyncToken = new GlSyncPoint(syncTokenPtr)) {
          glSyncToken.Wait();
        }
      }
      Instance.availableTextureFrames.Enqueue(textureFrame);
    }
  }

  private static bool IsStale(TextureFrame textureFrame) {
    lock(Instance.dimensionLock) {
      return textureFrame.width != Instance.textureWidth || textureFrame.height != Instance.textureHeight;
    }
  }

  private TextureFrame CreateNewTextureFrame() {
    lock(dimensionLock) {
      return new TextureFrame(textureWidth, textureHeight, (GlTextureBuffer.DeletionCallback)OnTextureFrameRelease);
    }
  }

  private IEnumerator WaitForTextureFrame(Action<TextureFrame> callback) {
    TextureFrame nextFrame = null;

    lock(((ICollection)availableTextureFrames).SyncRoot) {
      yield return new WaitUntil(() => {
        return poolSize > frameCount || availableTextureFrames.Count > 0;
      });

      while (availableTextureFrames.Count > 0) {
        var textureFrame = availableTextureFrames.Dequeue();

        if (!IsStale(textureFrame)) {
          nextFrame = textureFrame;
          break;
        }
      }

      if (nextFrame == null) {
        nextFrame = CreateNewTextureFrame();
      }
    }

    callback(nextFrame);

    lock(((ICollection)textureFramesInUse).SyncRoot) {
      textureFramesInUse.Add((UInt64)nextFrame.GetNativeTexturePtr(false), nextFrame);
    }
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
