using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  [StructLayout(LayoutKind.Sequential)]
  public unsafe struct ClassificationInner {
    public int index;
    public float score;
    public char* label;
  }

  public class Classification {
    public int index;
    public float score;
    public string label;

    public Classification(IntPtr ptr) {
      var classificationInner = Marshal.PtrToStructure<ClassificationInner>(ptr);

      this.index = classificationInner.index;
      this.score = classificationInner.score;

      unsafe {
        var label = Marshal.PtrToStringAnsi((IntPtr)(classificationInner.label));
        this.label = label;
      }
    }
  }
}
