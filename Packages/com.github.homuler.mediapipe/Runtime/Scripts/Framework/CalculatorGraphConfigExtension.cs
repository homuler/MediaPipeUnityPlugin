// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using pb = Google.Protobuf;

namespace Mediapipe
{
  public static class CalculatorGraphConfigExtension
  {
    public static CalculatorGraphConfig ParseFromTextFormat(this pb::MessageParser<CalculatorGraphConfig> _, string configText)
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
