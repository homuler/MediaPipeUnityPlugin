using System;

namespace Mediapipe {
  public class CalculatorGraphConfig : MpResourceHandle {
    public CalculatorGraphConfig(IntPtr ptr) : base(ptr) {}

    public static CalculatorGraphConfig ParseFromString(string configText) {
      return Protobuf.ParseFromStringAsCalculatorGraphConfig(configText);
    }

    protected override void DeleteMpPtr() {
      UnsafeNativeMethods.mp_CalculatorGraphConfig__delete(ptr);
    }

    public int byteSizeLong {
      get { return UnsafeNativeMethods.mp_CalculatorGraphConfig__ByteSizeLong(mpPtr); }
    }

    /// <exception cref="InvalidOperationException">Thrown when some required fields are not set</exception>
    public string SerializeAsString() {
      var str = MarshalStringFromNative(UnsafeNativeMethods.mp_CalculatorGraphConfig__SerializeAsString);

      if (str == null) {
        throw new InvalidOperationException("All the required fields must be set");
      }

      return str;
    }

    public string DebugString() {
      return MarshalStringFromNative(UnsafeNativeMethods.mp_CalculatorGraphConfig__DebugString);
    }
  }
}
