using pb = global::Google.Protobuf;
namespace Mediapipe {
  public static class CalculatorGraphConfigExtension {
    /// <summary>
    /// </summary>
    public static CalculatorGraphConfig ParseFromTextFormat(this pb::MessageParser<CalculatorGraphConfig> parser, string configText) {
      UnsafeNativeMethods.mp_api__ConvertFromCalculatorGraphConfigTextFormat(configText, out var serializedProtoPtr).Assert();

      var config = Protobuf.DeserializeProto(serializedProtoPtr, CalculatorGraphConfig.Parser);
      UnsafeNativeMethods.mp_api_SerializedProto__delete(serializedProtoPtr);

      return config;
    }
  }
}
