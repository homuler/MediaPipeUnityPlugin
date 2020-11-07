namespace Mediapipe {
  public class Glog {
    public static void Initialize(string name, string dir) {
      UnsafeNativeMethods.google_InitGoogleLogging__PKc(name, dir).Assert();
    }

    public static void Shutdown() {
      UnsafeNativeMethods.google_ShutdownGoogleLogging().Assert();
    }
  }
}
