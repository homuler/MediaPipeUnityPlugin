namespace Mediapipe {
  public class Glog {
    public static void Initialize(string name, string dir) {
      UnsafeNativeMethods.InitGoogleLogging(name, dir);
    }

    public static void Shutdown() {
      UnsafeNativeMethods.ShutdownGoogleLogging();
    }
  }
}
