// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System.Linq;

namespace Mediapipe.Tests
{
  public class ValidatedGraphConfigTest
  {
    private const string _PassThroughConfigText = @"
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""in""
  output_stream: ""out1""
}
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""out1""
  output_stream: ""out""
}
input_stream: ""in""
output_stream: ""out""
";

    private const string _FlowLimiterConfigText = @"
input_stream: ""input_video""
input_stream: ""output""

node {
  calculator: ""FlowLimiterCalculator""
  input_stream: ""input_video""
  input_stream: ""FINISHED:output""
  input_stream_info: {
    tag_index: ""FINISHED""
    back_edge: true
  }
  input_side_packet: ""MAX_IN_FLIGHT:max_in_flight""
  input_side_packet: ""OPTIONS:flow_limiter_calculator_options""
  output_stream: ""throttled_input_video""
}
";

    private const string _ImageTransformationConfigText = @"
input_stream: ""input_video""

node: {
  calculator: ""ImageTransformationCalculator""
  input_stream: ""IMAGE:input_video""
  input_side_packet: ""ROTATION_DEGREES:input_rotation""
  input_side_packet: ""FLIP_HORIZONTALLY:input_horizontally_flipped""
  input_side_packet: ""FLIP_VERTICALLY:input_vertically_flipped""
  output_stream: ""IMAGE:transformed_input_video""
}
";

    private const string _ConstantSidePacketConfigText = @"
node {
  calculator: ""ConstantSidePacketCalculator""
  output_side_packet: ""PACKET:0:int_packet""
  output_side_packet: ""PACKET:1:float_packet""
  output_side_packet: ""PACKET:2:bool_packet""
  output_side_packet: ""PACKET:3:string_packet""
  options: {
    [mediapipe.ConstantSidePacketCalculatorOptions.ext]: {
      packet { int_value: 256 }
      packet { float_value: 0.5f }
      packet { bool_value: false }
      packet { string_value: ""string"" }
    }
  }
}
";

    private const string _FaceDetectionShortRangeConfigText = @"
input_stream: ""image""
input_stream: ""roi""

node {
  calculator: ""FaceDetectionShortRange""
  input_stream: ""IMAGE:image""
  input_stream: ""ROI:roi""
  output_stream: ""DETECTIONS:detections""
}
";

    #region Constructor
    [Test]
    public void Ctor_ShouldInstantiateValidatedGraphConfig()
    {
      Assert.DoesNotThrow(() =>
      {
        var config = new ValidatedGraphConfig();
        config.Dispose();
      });
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.False(config.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var config = new ValidatedGraphConfig();
      config.Dispose();

      Assert.True(config.isDisposed);
    }
    #endregion

    #region #Initialize
    [Test]
    public void Initialize_ShouldInitialize_When_CalledWithConfig()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        Assert.True(config.Initialized());
      }
    }

    [Test]
    public void Initialize_ShouldInitialize_When_CalledWithValidGraphType()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize("SwitchContainer");
        Assert.True(config.Initialized());
      }
    }

    [Test]
    public void Initialize_ShouldReturnInternalError_When_CalledWithInvalidGraphType()
    {
      using (var config = new ValidatedGraphConfig())
      {
        var exception = Assert.Throws<BadStatusException>(() => config.Initialize("InvalidSubgraph"));
        Assert.AreEqual(StatusCode.NotFound, exception.statusCode);
        Assert.False(config.Initialized());
      }
    }
    #endregion

    #region #ValidateRequiredSidePackets
    [Test]
    public void ValidateRequiredSidePackets_ShouldNotThrow_When_TheConfigDoesNotRequireSidePackets_And_SidePacketIsEmpty()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        using (var sidePacket = new PacketMap())
        {
          Assert.DoesNotThrow(() => config.ValidateRequiredSidePackets(sidePacket));
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldNotThrow_When_TheConfigDoesNotRequireSidePackets_And_SidePacketIsNotEmpty()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        using (var sidePacket = new PacketMap())
        {
          sidePacket.Emplace("in", Packet.CreateInt(0));
          Assert.DoesNotThrow(() => config.ValidateRequiredSidePackets(sidePacket));
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldNotThrow_When_AllTheSidePacketsAreOptional_And_SidePacketIsEmpty()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_FlowLimiterConfigText));
        using (var sidePacket = new PacketMap())
        {
          Assert.DoesNotThrow(() => config.ValidateRequiredSidePackets(sidePacket));
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldThrowInvalidArgumentError_When_TheConfigRequiresSidePackets_And_SidePacketIsEmpty()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ImageTransformationConfigText));
        using (var sidePacket = new PacketMap())
        {
          var exception = Assert.Throws<BadStatusException>(() => config.ValidateRequiredSidePackets(sidePacket));
          Assert.AreEqual(StatusCode.InvalidArgument, exception.statusCode);
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldThrowInvalidArgumentError_When_AllTheRequiredSidePacketsAreNotGiven()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ImageTransformationConfigText));
        using (var sidePacket = new PacketMap())
        {
          sidePacket.Emplace("input_horizontally_flipped", Packet.CreateBool(false));
          sidePacket.Emplace("input_vertically_flipped", Packet.CreateBool(true));
          var exception = Assert.Throws<BadStatusException>(() => config.ValidateRequiredSidePackets(sidePacket));
          Assert.AreEqual(StatusCode.InvalidArgument, exception.statusCode);
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldReturnInvalidArgumentError_When_TheSidePacketValuesAreWrong()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ImageTransformationConfigText));
        using (var sidePacket = new PacketMap())
        {
          sidePacket.Emplace("input_horizontally_flipped", Packet.CreateBool(false));
          sidePacket.Emplace("input_vertically_flipped", Packet.CreateBool(true));
          sidePacket.Emplace("input_rotation", Packet.CreateString("0"));
          var exception = Assert.Throws<BadStatusException>(() => config.ValidateRequiredSidePackets(sidePacket));
          Assert.AreEqual(StatusCode.InvalidArgument, exception.statusCode);
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldNotThrow_When_AllTheRequiredSidePacketsAreGiven()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ImageTransformationConfigText));
        using (var sidePacket = new PacketMap())
        {
          sidePacket.Emplace("input_horizontally_flipped", Packet.CreateBool(false));
          sidePacket.Emplace("input_vertically_flipped", Packet.CreateBool(true));
          sidePacket.Emplace("input_rotation", Packet.CreateInt(0));
          Assert.DoesNotThrow(() => config.ValidateRequiredSidePackets(sidePacket));
        }
      }
    }
    #endregion

    #region Config
    [Test]
    public void Config_ShouldReturnAnEmptyConfig_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        var canonicalizedConfig = config.Config();
        Assert.AreEqual(canonicalizedConfig.CalculateSize(), 0);
      }
    }

    [Test]
    public void Config_ShouldReturnTheCanonicalizedConfig_When_TheConfigIsPassThroughConfig()
    {
      using (var config = new ValidatedGraphConfig())
      {
        var originalConfig = CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText);
        config.Initialize(originalConfig);
        var canonicalizedConfig = config.Config();

        Assert.AreEqual(originalConfig.Node, canonicalizedConfig.Node);
        Assert.AreEqual(originalConfig.InputStream, canonicalizedConfig.InputStream);
        Assert.AreEqual(originalConfig.OutputStream, canonicalizedConfig.OutputStream);
        Assert.IsEmpty(originalConfig.Executor);
        Assert.AreEqual(1, canonicalizedConfig.Executor.Count);
        Assert.AreEqual(0, canonicalizedConfig.Executor[0].CalculateSize());

        Assert.AreEqual(80, originalConfig.CalculateSize());
        Assert.AreEqual(82, canonicalizedConfig.CalculateSize());
      }
    }

    [Test]
    public void Config_ShouldReturnTheCanonicalizedConfig_When_TheConfigIsFaceDetectionShortRangeCommonConfig()
    {
      using (var config = new ValidatedGraphConfig())
      {
        var originalConfig = CalculatorGraphConfig.Parser.ParseFromTextFormat(_FaceDetectionShortRangeConfigText);
        config.Initialize(originalConfig);
        var canonicalizedConfig = config.Config();

        Assert.AreEqual(84, originalConfig.CalculateSize());
        // 2167 on CPU, 2166 on GPU
        Assert.AreEqual(2166, canonicalizedConfig.CalculateSize(), 1);
      }
    }
    #endregion

    #region InputStreamInfos
    [Test]
    public void InputStreamInfos_ShouldReturnEmptyList_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.IsEmpty(config.InputStreamInfos());
      }
    }

    [Test]
    public void InputStreamInfos_ShouldReturnEmptyList_When_NoInputStreamExists()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ConstantSidePacketConfigText));
        Assert.IsEmpty(config.InputStreamInfos());
      }
    }

    [Test]
    public void InputStreamInfos_ShouldReturnEdgeInfoList_When_InputStreamsExist()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        var inputStreamInfos = config.InputStreamInfos();

        Assert.AreEqual(inputStreamInfos.Count, 2);

        var inStream = inputStreamInfos.First((edgeInfo) => edgeInfo.name == "in");
        Assert.AreEqual(0, inStream.upstream);
        Assert.AreEqual(NodeType.Calculator, inStream.parentNode.type);
        Assert.AreEqual(0, inStream.parentNode.index);
        Assert.False(inStream.backEdge);

        var out1Stream = inputStreamInfos.First((edgeInfo) => edgeInfo.name == "out1");
        Assert.AreEqual(1, out1Stream.upstream);
        Assert.AreEqual(NodeType.Calculator, out1Stream.parentNode.type);
        Assert.AreEqual(1, out1Stream.parentNode.index);
        Assert.False(out1Stream.backEdge);
      }
    }
    #endregion

    #region OutputStreamInfos
    [Test]
    public void OutputStreamInfos_ShouldReturnEmptyList_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.IsEmpty(config.OutputStreamInfos());
      }
    }

    [Test]
    public void OutputStreamInfos_ShouldReturnEdgeInfoList_When_OutputStreamsExist()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        var outputStreamInfos = config.OutputStreamInfos();

        Assert.AreEqual(3, outputStreamInfos.Count);

        var inStream = outputStreamInfos.First((edgeInfo) => edgeInfo.name == "in");
        Assert.AreEqual(-1, inStream.upstream);
        Assert.AreEqual(NodeType.GraphInputStream, inStream.parentNode.type);
        Assert.AreEqual(2, inStream.parentNode.index, 2);
        Assert.False(inStream.backEdge);

        var out1Stream = outputStreamInfos.First((edgeInfo) => edgeInfo.name == "out1");
        Assert.AreEqual(-1, out1Stream.upstream);
        Assert.AreEqual(NodeType.Calculator, out1Stream.parentNode.type);
        Assert.AreEqual(0, out1Stream.parentNode.index);
        Assert.False(out1Stream.backEdge);

        var outStream = outputStreamInfos.First((edgeInfo) => edgeInfo.name == "out");
        Assert.AreEqual(-1, outStream.upstream);
        Assert.AreEqual(NodeType.Calculator, outStream.parentNode.type);
        Assert.AreEqual(1, outStream.parentNode.index);
        Assert.False(outStream.backEdge);
      }
    }
    #endregion

    #region InputSidePacketInfos
    [Test]
    public void InputSidePacketInfos_ShouldReturnEmptyList_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.IsEmpty(config.InputSidePacketInfos());
      }
    }

    [Test]
    public void InputSidePacketInfos_ShouldReturnEmptyList_When_NoInputSidePacketExists()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        Assert.IsEmpty(config.InputSidePacketInfos());
      }
    }

    [Test]
    public void InputSidePacketInfos_ShouldReturnEdgeInfoList_When_InputSidePacketsExist()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_FlowLimiterConfigText));
        var inputSidePacketInfos = config.InputSidePacketInfos();

        Assert.True(inputSidePacketInfos.Count >= 2);

        var maxInFlightPacket = inputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "max_in_flight");
        Assert.AreEqual(-1, maxInFlightPacket.upstream);
        Assert.AreEqual(NodeType.Calculator, maxInFlightPacket.parentNode.type);
        Assert.False(maxInFlightPacket.backEdge);

        var flowLimiterCalculatorOptionsPacket = inputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "flow_limiter_calculator_options");
        Assert.AreEqual(-1, flowLimiterCalculatorOptionsPacket.upstream);
        Assert.AreEqual(NodeType.Calculator, flowLimiterCalculatorOptionsPacket.parentNode.type);
        Assert.False(flowLimiterCalculatorOptionsPacket.backEdge);
      }
    }
    #endregion

    #region OutputSidePacketInfos
    [Test]
    public void OutputSidePacketInfos_ShouldReturnEmptyList_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.IsEmpty(config.OutputSidePacketInfos());
      }
    }

    [Test]
    public void OutputSidePacketInfos_ShouldReturnEmptyList_When_NoOutputSidePacketExists()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        Assert.IsEmpty(config.OutputSidePacketInfos());
      }
    }

    [Test]
    public void OutputSidePacketInfos_ShouldReturnEdgeInfoList_When_OutputSidePacketsExist()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ConstantSidePacketConfigText));
        var outputSidePacketInfos = config.OutputSidePacketInfos();

        Assert.AreEqual(4, outputSidePacketInfos.Count);

        var intPacket = outputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "int_packet");
        Assert.AreEqual(-1, intPacket.upstream);
        Assert.AreEqual(NodeType.Calculator, intPacket.parentNode.type);
        Assert.False(intPacket.backEdge);

        var floatPacket = outputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "float_packet");
        Assert.AreEqual(-1, floatPacket.upstream);
        Assert.AreEqual(NodeType.Calculator, floatPacket.parentNode.type);
        Assert.False(floatPacket.backEdge);

        var boolPacket = outputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "bool_packet");
        Assert.AreEqual(-1, boolPacket.upstream);
        Assert.AreEqual(NodeType.Calculator, boolPacket.parentNode.type);
        Assert.False(boolPacket.backEdge);

        var stringPacket = outputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "string_packet");
        Assert.AreEqual(-1, stringPacket.upstream);
        Assert.AreEqual(NodeType.Calculator, stringPacket.parentNode.type);
        Assert.False(stringPacket.backEdge);
      }
    }
    #endregion

    #region OutputStreamIndex
    [Test]
    public void OutputStreamIndex_ShouldReturnNegativeValue_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.AreEqual(-1, config.OutputStreamIndex(""));
      }
    }

    [Test]
    public void OutputStreamIndex_ShouldReturnNegativeValue_When_TheNameIsInvalid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        Assert.AreEqual(-1, config.OutputStreamIndex("unknown"));
      }
    }

    [Test]
    public void OutputStreamIndex_ShouldReturnIndex_When_TheNameIsValid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        Assert.AreEqual(2, config.OutputStreamIndex("out"));
      }
    }

    [Test]
    public void OutputStreamIndex_ShouldReturnIndex_When_TheStreamIsNotPublic()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        Assert.AreEqual(1, config.OutputStreamIndex("out1"));
      }
    }
    #endregion

    #region OutputSidePacketIndex
    [Test]
    public void OutputSidePacketIndex_ShouldReturnNegativeValue_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.AreEqual(-1, config.OutputSidePacketIndex(""));
      }
    }

    [Test]
    public void OutputSidePacketIndex_ShouldReturnNegativeValue_When_TheNameIsInvalid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ConstantSidePacketConfigText));
        Assert.AreEqual(-1, config.OutputSidePacketIndex("unknown"));
      }
    }

    [Test]
    public void OutputSidePacketIndex_ShouldReturnIndex_When_TheNameIsValid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ConstantSidePacketConfigText));
        Assert.AreEqual(0, config.OutputSidePacketIndex("int_packet"));
      }
    }
    #endregion


    #region OutputStreamToNode
    [Test]
    public void OutputStreamToNode_ShouldReturnNegativeValue_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.AreEqual(-1, config.OutputStreamToNode(""));
      }
    }

    [Test]
    public void OutputStreamToNode_ShouldReturnNegativeValue_When_TheNameIsInvalid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        Assert.AreEqual(-1, config.OutputStreamToNode("unknown"));
      }
    }

    [Test]
    public void OutputStreamToNode_ShouldReturnIndex_When_TheNameIsValid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        Assert.AreEqual(0, config.OutputStreamToNode("out1"));
      }
    }
    #endregion

    #region RegisteredSidePacketTypeName
    [Test]
    public void RegisteredSidePacketTypeName_ShouldReturnInvalidArgumentError_When_TheSidePacketDoesNotExist()
    {
      using (var config = new ValidatedGraphConfig())
      {
        var exception = Assert.Throws<BadStatusException>(() => { _ = config.RegisteredSidePacketTypeName("max_in_flight"); });
        Assert.AreEqual(StatusCode.InvalidArgument, exception.statusCode);
      }
    }

    [Test]
    public void RegisteredSidePacketTypeName_ShouldReturnUnknownError_When_TheSidePacketTypeCannotBeDetermined()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_FlowLimiterConfigText));
        var exception = Assert.Throws<BadStatusException>(() => { _ = config.RegisteredSidePacketTypeName("max_in_flight"); });
        Assert.AreEqual(StatusCode.Unknown, exception.statusCode);
      }
    }
    #endregion

    #region RegisteredStreamTypeName
    [Test]
    public void RegisteredStreamTypeName_ShouldReturnInvalidArgumentError_When_TheStreamDoesNotExist()
    {
      using (var config = new ValidatedGraphConfig())
      {
        var exception = Assert.Throws<BadStatusException>(() => { _ = config.RegisteredStreamTypeName("in"); });
        Assert.AreEqual(StatusCode.InvalidArgument, exception.statusCode);
      }
    }

    [Test]
    public void RegisteredStreamTypeName_ShouldReturnUnknownError_When_TheStreamTypeCannotBeDetermined()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        var exception = Assert.Throws<BadStatusException>(() => { _ = config.RegisteredStreamTypeName("in"); });
        Assert.AreEqual(StatusCode.Unknown, exception.statusCode);
      }
    }
    #endregion

    #region Package
    [Test]
    public void Package_ShouldReturnNull_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.IsNull(config.Package());
      }
    }

    [Test]
    public void Package_ShouldReturnNull_When_TheNamespaceIsNotSet()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText));
        Assert.IsNull(config.Package());
      }
    }
    #endregion

    #region IsReservedExecutorName
    [Test]
    public void IsReservedExecutorName_ShouldReturnFalse_When_TheNameIsNotReserved()
    {
      Assert.False(ValidatedGraphConfig.IsReservedExecutorName("unknown"));
    }

    [Test]
    public void IsReservedExecutorName_ShouldReturnFalse_When_TheNameIsReserved()
    {
      Assert.True(ValidatedGraphConfig.IsReservedExecutorName("default"));
      Assert.True(ValidatedGraphConfig.IsReservedExecutorName("gpu"));
      Assert.True(ValidatedGraphConfig.IsReservedExecutorName("__gpu"));
    }
    #endregion

    #region IsExternalSidePacket
    [Test]
    public void IsExternalSidePacket_ShouldReturnFalse_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.False(config.IsExternalSidePacket("max_in_flight"));
      }
    }


    [Test]
    public void IsExternalSidePacket_ShouldReturnFalse_When_TheSidePacketIsInternal()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ConstantSidePacketConfigText));
        Assert.False(config.IsExternalSidePacket("int_packet"));
      }
    }

    [Test]
    public void IsExternalSidePacket_ShouldReturnTrue_When_TheSidePacketIsExternal()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_FlowLimiterConfigText));
        Assert.True(config.IsExternalSidePacket("max_in_flight"));
      }
    }
    #endregion
  }
}
