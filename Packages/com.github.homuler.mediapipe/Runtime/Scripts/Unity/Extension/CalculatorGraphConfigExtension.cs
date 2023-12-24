// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Unity
{
  public static class CalculatorGraphConfigExtension
  {
    public static string AddPacketPresenceCalculator(this CalculatorGraphConfig config, string outputStreamName)
    {
      var presenceStreamName = Tool.GetUnusedStreamName(config, $"{outputStreamName}_presence");
      var packetPresenceCalculatorNode = new CalculatorGraphConfig.Types.Node()
      {
        Calculator = "PacketPresenceCalculator"
      };
      packetPresenceCalculatorNode.InputStream.Add($"PACKET:{outputStreamName}");
      packetPresenceCalculatorNode.OutputStream.Add($"PRESENCE:{presenceStreamName}");

      config.Node.Add(packetPresenceCalculatorNode);
      return presenceStreamName;
    }
  }
}
