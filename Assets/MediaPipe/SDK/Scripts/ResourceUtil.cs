namespace Mediapipe {
  public class ResourceUtil {
    public static void SetResourceRootPath(string path) {
      UnsafeNativeMethods.MpSetResourceRootPath(path);
    }
  }
}
