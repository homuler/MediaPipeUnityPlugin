using System;

namespace Mediapipe {
  public class ResourceUtil {
    public static void InitializeResourceManager(ResourceManager resourceManager) {
      UnsafeNativeMethods.MpResourceManagerInitialize(resourceManager.GetCacheFilePathResolverPtr(), resourceManager.GetReadFileHandlerPtr());
    }

    public static void CopyBytes(IntPtr dst, byte[] src) {
      UnsafeNativeMethods.MpStringCopy(dst, src, src.Length);
    }
  }
}
