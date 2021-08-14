using System.IO;
using UnityEngine;

namespace Mediapipe.Unity {
  public class StreamingAssetsResourceManager : FileSystemResourceManager {
    public StreamingAssetsResourceManager() : base(Application.streamingAssetsPath) {}
    public StreamingAssetsResourceManager(string path) : base(Path.Combine(Application.streamingAssetsPath, path)) {}
  }
}
