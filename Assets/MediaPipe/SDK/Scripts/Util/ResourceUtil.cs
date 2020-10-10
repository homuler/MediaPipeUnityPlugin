using System;

namespace Mediapipe {
  public class ResourceUtil {
    public static void InitializeAssetManager(AssetManager assetManager) {
      UnsafeNativeMethods.MpAssetManagerInitialize(assetManager.GetCacheFilePathResolverPtr(), assetManager.GetReadFileHandlerPtr());
    }

    public static void CopyBytes(IntPtr dst, byte[] src) {
      UnsafeNativeMethods.MpStringCopy(dst, src, src.Length);
    }
  }
}
