using System;

namespace Mediapipe {
  public interface IMpResourceHandle {
    IntPtr mpPtr { get; }
    IntPtr ReleaseMpPtr();
    void TransferOwnership();
  }
}
