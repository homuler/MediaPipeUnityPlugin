using UnityEngine;

namespace Mediapipe.Unity {
  public class MaskAnnotationController : AnnotationController<MaskAnnotation> {
    [SerializeField] int maskWidth = 512;
    [SerializeField] int maskHeight = 512;
    [SerializeField, Range(0, 1)] float minAlpha = 0.9f;
    [SerializeField, Range(0, 1)] float maxAlpha = 1.0f;

    ImageFrame currentTarget;
    byte[] maskArray;

    public void InitScreen() {
      maskArray = new byte[maskWidth * maskHeight];
      annotation.InitScreen();
    }

    public void DrawNow(ImageFrame target) {
      currentTarget = target;
      UpdateMaskArray(currentTarget);
      SyncNow();
    }

    public void DrawLater(ImageFrame target) {
      UpdateCurrentTarget(target, ref currentTarget);
      UpdateMaskArray(currentTarget);
    }

    void UpdateMaskArray(ImageFrame imageFrame) {
      if (imageFrame != null) {
        imageFrame.GetChannel(0, isMirrored, maskArray);
      }
    }

    protected override void SyncNow() {
      isStale = false;
      annotation.Draw(maskArray, maskWidth, maskHeight, minAlpha, maxAlpha);
    }
  }
}
