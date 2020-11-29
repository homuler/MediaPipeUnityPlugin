namespace Mediapipe {
  public class ResourceUtil {
    public static void InitializeResourceManager(ResourceManager resourceManager) {
      SafeNativeMethods.mp_api__PrepareResourceManager(resourceManager.cacheFilePathResolver, resourceManager.readFileHandler);
    }
  }
}
