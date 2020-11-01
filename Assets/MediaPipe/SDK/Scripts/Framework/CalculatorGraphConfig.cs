using System;

namespace Mediapipe {
  public class CalculatorGraphConfig : MpResourceHandle {
    public CalculatorGraphConfig(IntPtr ptr) : base(ptr) {}

    public static CalculatorGraphConfig ParseFromString(string configText) {
      return Protobuf.ParseFromStringAsCalculatorGraphConfig(configText);
    }

    protected override void DisposeUnmanaged() {
      if (OwnsResource()) {
        UnsafeNativeMethods.mp_CalculatorGraphConfig__delete(ptr);
      }
      base.DisposeUnmanaged();
    }
  }
}
