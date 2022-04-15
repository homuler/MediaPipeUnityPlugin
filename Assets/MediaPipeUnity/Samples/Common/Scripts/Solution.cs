// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity
{
  public abstract class Solution : MonoBehaviour
  {
#pragma warning disable IDE1006
    // TODO: make it static
    protected virtual string TAG => GetType().Name;
#pragma warning restore IDE1006

    public Bootstrap bootstrap;
    protected bool isPaused;

    protected virtual IEnumerator Start()
    {
      bootstrap = FindBootstrap();
      yield return new WaitUntil(() => bootstrap.isFinished);

      Play();
    }

    /// <summary>
    ///   Start the main program from the beginning.
    /// </summary>
    public virtual void Play()
    {
      isPaused = false;
    }

    /// <summary>
    ///   Pause the main program.
    /// <summary>
    public virtual void Pause()
    {
      isPaused = true;
    }

    /// <summary>
    ///    Resume the main program.
    ///    If the main program has not begun, it'll do nothing.
    /// </summary>
    public virtual void Resume()
    {
      isPaused = false;
    }

    /// <summary>
    ///   Stops the main program.
    /// </summary>
    public virtual void Stop()
    {
      isPaused = true;
    }

    protected static void SetupAnnotationController<T>(AnnotationController<T> annotationController, ImageSource imageSource, bool expectedToBeMirrored = false) where T : HierarchicalAnnotation
    {
      annotationController.isMirrored = expectedToBeMirrored ^ imageSource.isHorizontallyFlipped ^ imageSource.isFrontFacing;
      annotationController.rotationAngle = imageSource.rotation.Reverse();
    }

    protected static void ReadFromImageSource(ImageSource imageSource, TextureFrame textureFrame)
    {
      var sourceTexture = imageSource.GetCurrentTexture();

      // For some reason, when the image is coiped on GPU, latency tends to be high.
      // So even when OpenGL ES is available, use CPU to copy images.
      var textureType = sourceTexture.GetType();

      if (textureType == typeof(WebCamTexture))
      {
        textureFrame.ReadTextureFromOnCPU((WebCamTexture)sourceTexture);
      }
      else if (textureType == typeof(Texture2D))
      {
        textureFrame.ReadTextureFromOnCPU((Texture2D)sourceTexture);
      }
      else
      {
        textureFrame.ReadTextureFromOnCPU(sourceTexture);
      }
    }

    protected Bootstrap FindBootstrap()
    {
      var bootstrapObj = GameObject.Find("Bootstrap");

      if (bootstrapObj != null)
      {
        return bootstrapObj.GetComponent<Bootstrap>();
      }

      Logger.LogWarning(TAG, "Global Bootstrap instance is not found (maybe running a sample scene directly), "
                            + "so activating a fallback Bootstrap instance attached to each Solution object");

      var bootstrap = GetComponent<Bootstrap>();
      bootstrap.enabled = true;

      // hide menu button when trying a single scene.
      DisableMenuButton();
      return bootstrap;
    }

    private void DisableMenuButton()
    {
      var menuButton = GameObject.Find("MenuButton");
      menuButton.SetActive(false);
    }
  }
}
