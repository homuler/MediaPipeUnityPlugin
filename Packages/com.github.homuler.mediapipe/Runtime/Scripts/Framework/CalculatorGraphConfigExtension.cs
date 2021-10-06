using pb = global::Google.Protobuf;

namespace Mediapipe
{
  public static class CalculatorGraphConfigExtension
  {
    public static CalculatorGraphConfig ParseFromTextFormat(this pb::MessageParser<CalculatorGraphConfig> parser, string configText)
    {
      if (UnsafeNativeMethods.mp_api__ConvertFromCalculatorGraphConfigTextFormat(configText, out var serializedProto))
      {
        var config = serializedProto.Deserialize(CalculatorGraphConfig.Parser);
        serializedProto.Dispose();
        return config;
      }
      throw new MediaPipeException("Failed to parse config text. See error logs for more details");
    }
  }
}
