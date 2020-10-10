using System;

namespace Mediapipe {
  public class ResourceUtil {
    public static void InitializeResourceManager(AssetManager assetManager) {
      UnsafeNativeMethods.MpResourceManagerInitialize(assetManager.GetCacheFilePathResolverPtr(), assetManager.GetReadFileHandlerPtr());
    }

    public static void CopyBytes(IntPtr dst, byte[] src) {
      UnsafeNativeMethods.MpStringCopy(dst, src, src.Length);
    }
  }
}
