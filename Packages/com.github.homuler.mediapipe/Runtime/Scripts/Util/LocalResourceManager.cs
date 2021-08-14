using System.IO;
using UnityEngine;

namespace Mediapipe.Unity {
  public class LocalResourceManager : FileSystemResourceManager {
    public LocalResourceManager() : base(Path.Combine(Application.dataPath, "..", "Packages", "com.github.homuler.mediapipe", "Runtime", "Resources")) {}
  }
}
