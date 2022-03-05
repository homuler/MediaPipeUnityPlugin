// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe;
using Mediapipe.Unity;
using NUnit.Framework;
using System.Linq;

namespace Tests
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

    private const string _ObjectronConfigText = @"
input_stream: ""input_video""

node {
  calculator: ""ObjectronGpuSubgraph""
  input_stream: ""IMAGE_GPU:input_video""
  input_side_packet: ""LABELS_CSV:allowed_labels""
  input_side_packet: ""MAX_NUM_OBJECTS:max_num_objects""
  output_stream: ""FRAME_ANNOTATION:lifted_objects""
  output_stream: ""NORM_RECTS:multi_box_rects""
  output_stream: ""MULTI_LANDMARKS:multi_box_landmarks""
}  
";
    private const string _PoseLandmarkConfigText = @"
input_stream: ""input_video""

node {
  calculator: ""PoseLandmarkGpu""
  input_stream: ""IMAGE:input_video""
  input_side_packet: ""MODEL_COMPLEXITY:model_complexity""
  input_side_packet: ""SMOOTH_LANDMARKS:smooth_landmarks""
  output_stream: ""LANDMARKS:pose_landmarks""
  output_stream: ""WORLD_LANDMARKS:pose_world_landmarks""
  output_stream: ""DETECTION:pose_detection""
  output_stream: ""ROI_FROM_LANDMARKS:roi_from_landmarks""
}";

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
    public void Initialize_ShouldReturnOk_When_CalledWithConfig()
    {
      using (var config = new ValidatedGraphConfig())
      {
        using (var status = config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)))
        {
          Assert.True(status.Ok());
        }
        Assert.True(config.Initialized());
      }
    }

    [Test]
    public void Initialize_ShouldReturnOk_When_CalledWithValidGraphType()
    {
      using (var config = new ValidatedGraphConfig())
      {
        using (var status = config.Initialize("SwitchContainer"))
        {
          Assert.True(status.Ok());
        }
        Assert.True(config.Initialized());
      }
    }

    [Test]
    public void Initialize_ShouldReturnInternalError_When_CalledWithInvalidGraphType()
    {
      using (var config = new ValidatedGraphConfig())
      {
        using (var status = config.Initialize("InvalidSubgraph"))
        {
          Assert.AreEqual(status.Code(), Status.StatusCode.NotFound);
        }
        Assert.False(config.Initialized());
      }
    }
    #endregion

    #region #ValidateRequiredSidePackets
    [Test]
    public void ValidateRequiredSidePackets_ShouldReturnOk_When_TheConfigDoesNotRequireSidePackets_And_SidePacketIsEmpty()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        using (var sidePacket = new SidePacket())
        {
          using (var status = config.ValidateRequiredSidePackets(sidePacket))
          {
            Assert.True(status.Ok());
          }
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldReturnOk_When_TheConfigDoesNotRequireSidePackets_And_SidePacketIsNotEmpty()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        using (var sidePacket = new SidePacket())
        {
          sidePacket.Emplace("in", new IntPacket(0));
          using (var status = config.ValidateRequiredSidePackets(sidePacket))
          {
            Assert.True(status.Ok());
          }
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldReturnOk_When_AllTheSidePacketsAreOptional_And_SidePacketIsEmpty()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PoseLandmarkConfigText)).AssertOk();
        using (var sidePacket = new SidePacket())
        {
          using (var status = config.ValidateRequiredSidePackets(sidePacket))
          {
            Assert.True(status.Ok());
          }
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldReturnInvalidArgumentError_When_TheConfigRequiresSidePackets_And_SidePacketIsEmpty()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ObjectronConfigText)).AssertOk();
        using (var sidePacket = new SidePacket())
        {
          using (var status = config.ValidateRequiredSidePackets(sidePacket))
          {
            Assert.AreEqual(status.Code(), Status.StatusCode.InvalidArgument);
          }
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldReturnInvalidArgumentError_When_AllTheRequiredSidePacketsAreNotGiven()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ObjectronConfigText)).AssertOk();
        using (var sidePacket = new SidePacket())
        {
          sidePacket.Emplace("max_num_objects", new IntPacket(3));
          using (var status = config.ValidateRequiredSidePackets(sidePacket))
          {
            Assert.AreEqual(status.Code(), Status.StatusCode.InvalidArgument);
          }
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldReturnInvalidArgumentError_When_TheSidePacketValuesAreWrong()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ObjectronConfigText)).AssertOk();
        using (var sidePacket = new SidePacket())
        {
          sidePacket.Emplace("allowed_labels", new StringPacket("cup"));
          sidePacket.Emplace("max_num_objects", new StringPacket("3"));
          using (var status = config.ValidateRequiredSidePackets(sidePacket))
          {
            Assert.AreEqual(status.Code(), Status.StatusCode.InvalidArgument);
          }
        }
      }
    }

    [Test]
    public void ValidateRequiredSidePackets_ShouldReturnOk_When_AllTheRequiredSidePacketsAreGiven()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ObjectronConfigText)).AssertOk();
        using (var sidePacket = new SidePacket())
        {
          sidePacket.Emplace("allowed_labels", new StringPacket("cup"));
          sidePacket.Emplace("max_num_objects", new IntPacket(3));
          using (var status = config.ValidateRequiredSidePackets(sidePacket))
          {
            Assert.True(status.Ok());
          }
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
        config.Initialize(originalConfig).AssertOk();
        var canonicalizedConfig = config.Config();

        Assert.AreEqual(originalConfig.Node, canonicalizedConfig.Node);
        Assert.AreEqual(originalConfig.InputStream, canonicalizedConfig.InputStream);
        Assert.AreEqual(originalConfig.OutputStream, canonicalizedConfig.OutputStream);
        Assert.IsEmpty(originalConfig.Executor);
        Assert.AreEqual(canonicalizedConfig.Executor.Count, 1);
        Assert.AreEqual(canonicalizedConfig.Executor[0].CalculateSize(), 0);

        Assert.AreEqual(originalConfig.CalculateSize(), 80);
        Assert.AreEqual(canonicalizedConfig.CalculateSize(), 82);
      }
    }

    [Test]
    public void Config_ShouldReturnTheCanonicalizedConfig_When_TheConfigIsPoseLandmarkConfig()
    {
      using (var config = new ValidatedGraphConfig())
      {
        var originalConfig = CalculatorGraphConfig.Parser.ParseFromTextFormat(_PoseLandmarkConfigText);
        config.Initialize(originalConfig).AssertOk();
        var canonicalizedConfig = config.Config();

        Assert.AreEqual(originalConfig.CalculateSize(), 251);
        Assert.AreEqual(canonicalizedConfig.CalculateSize(), 26514);
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
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ConstantSidePacketConfigText)).AssertOk();
        Assert.IsEmpty(config.InputStreamInfos());
      }
    }

    [Test]
    public void InputStreamInfos_ShouldReturnEdgeInfoList_When_InputStreamsExist()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        var inputStreamInfos = config.InputStreamInfos();

        Assert.AreEqual(inputStreamInfos.Count, 2);

        var inStream = inputStreamInfos.First((edgeInfo) => edgeInfo.name == "in");
        Assert.AreEqual(inStream.upstream, 0);
        Assert.AreEqual(inStream.parentNode.type, NodeType.Calculator);
        Assert.AreEqual(inStream.parentNode.index, 0);
        Assert.False(inStream.backEdge);

        var out1Stream = inputStreamInfos.First((edgeInfo) => edgeInfo.name == "out1");
        Assert.AreEqual(out1Stream.upstream, 1);
        Assert.AreEqual(out1Stream.parentNode.type, NodeType.Calculator);
        Assert.AreEqual(out1Stream.parentNode.index, 1);
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
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        var outputStreamInfos = config.OutputStreamInfos();

        Assert.AreEqual(outputStreamInfos.Count, 3);

        var inStream = outputStreamInfos.First((edgeInfo) => edgeInfo.name == "in");
        Assert.AreEqual(inStream.upstream, -1);
        Assert.AreEqual(inStream.parentNode.type, NodeType.GraphInputStream);
        Assert.AreEqual(inStream.parentNode.index, 2);
        Assert.False(inStream.backEdge);

        var out1Stream = outputStreamInfos.First((edgeInfo) => edgeInfo.name == "out1");
        Assert.AreEqual(out1Stream.upstream, -1);
        Assert.AreEqual(out1Stream.parentNode.type, NodeType.Calculator);
        Assert.AreEqual(out1Stream.parentNode.index, 0);
        Assert.False(out1Stream.backEdge);

        var outStream = outputStreamInfos.First((edgeInfo) => edgeInfo.name == "out");
        Assert.AreEqual(outStream.upstream, -1);
        Assert.AreEqual(outStream.parentNode.type, NodeType.Calculator);
        Assert.AreEqual(outStream.parentNode.index, 1);
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
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        Assert.IsEmpty(config.InputSidePacketInfos());
      }
    }

    [Test]
    public void InputSidePacketInfos_ShouldReturnEdgeInfoList_When_InputSidePacketsExist()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PoseLandmarkConfigText)).AssertOk();
        var inputSidePacketInfos = config.InputSidePacketInfos();

        Assert.True(inputSidePacketInfos.Count >= 2);

        var modelComplexityPacket = inputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "model_complexity");
        Assert.AreEqual(modelComplexityPacket.upstream, -1);
        Assert.AreEqual(modelComplexityPacket.parentNode.type, NodeType.Calculator);
        Assert.False(modelComplexityPacket.backEdge);

        var smoothLandmarksPacket = inputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "smooth_landmarks");
        Assert.AreEqual(smoothLandmarksPacket.upstream, -1);
        Assert.AreEqual(smoothLandmarksPacket.parentNode.type, NodeType.Calculator);
        Assert.False(smoothLandmarksPacket.backEdge);
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
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        Assert.IsEmpty(config.OutputSidePacketInfos());
      }
    }

    [Test]
    public void OutputSidePacketInfos_ShouldReturnEdgeInfoList_When_OutputSidePacketsExist()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ConstantSidePacketConfigText)).AssertOk();
        var outputSidePacketInfos = config.OutputSidePacketInfos();

        Assert.AreEqual(outputSidePacketInfos.Count, 4);

        var intPacket = outputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "int_packet");
        Assert.AreEqual(intPacket.upstream, -1);
        Assert.AreEqual(intPacket.parentNode.type, NodeType.Calculator);
        Assert.False(intPacket.backEdge);

        var floatPacket = outputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "float_packet");
        Assert.AreEqual(floatPacket.upstream, -1);
        Assert.AreEqual(floatPacket.parentNode.type, NodeType.Calculator);
        Assert.False(floatPacket.backEdge);

        var boolPacket = outputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "bool_packet");
        Assert.AreEqual(boolPacket.upstream, -1);
        Assert.AreEqual(boolPacket.parentNode.type, NodeType.Calculator);
        Assert.False(boolPacket.backEdge);

        var stringPacket = outputSidePacketInfos.First((edgeInfo) => edgeInfo.name == "string_packet");
        Assert.AreEqual(stringPacket.upstream, -1);
        Assert.AreEqual(stringPacket.parentNode.type, NodeType.Calculator);
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
        Assert.AreEqual(config.OutputStreamIndex(""), -1);
      }
    }

    [Test]
    public void OutputStreamIndex_ShouldReturnNegativeValue_When_TheNameIsInvalid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        Assert.AreEqual(config.OutputStreamIndex("unknown"), -1);
      }
    }

    [Test]
    public void OutputStreamIndex_ShouldReturnIndex_When_TheNameIsValid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        Assert.AreEqual(config.OutputStreamIndex("out"), 2);
      }
    }

    [Test]
    public void OutputStreamIndex_ShouldReturnIndex_When_TheStreamIsNotPublic()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        Assert.AreEqual(config.OutputStreamIndex("out1"), 1);
      }
    }
    #endregion

    #region OutputSidePacketIndex
    [Test]
    public void OutputSidePacketIndex_ShouldReturnNegativeValue_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.AreEqual(config.OutputSidePacketIndex(""), -1);
      }
    }

    [Test]
    public void OutputSidePacketIndex_ShouldReturnNegativeValue_When_TheNameIsInvalid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ConstantSidePacketConfigText)).AssertOk();
        Assert.AreEqual(config.OutputSidePacketIndex("unknown"), -1);
      }
    }

    [Test]
    public void OutputSidePacketIndex_ShouldReturnIndex_When_TheNameIsValid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ConstantSidePacketConfigText)).AssertOk();
        Assert.AreEqual(config.OutputSidePacketIndex("int_packet"), 0);
      }
    }
    #endregion


    #region OutputStreamToNode
    [Test]
    public void OutputStreamToNode_ShouldReturnNegativeValue_When_NotInitialized()
    {
      using (var config = new ValidatedGraphConfig())
      {
        Assert.AreEqual(config.OutputStreamToNode(""), -1);
      }
    }

    [Test]
    public void OutputStreamToNode_ShouldReturnNegativeValue_When_TheNameIsInvalid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        Assert.AreEqual(config.OutputStreamToNode("unknown"), -1);
      }
    }

    [Test]
    public void OutputStreamToNode_ShouldReturnIndex_When_TheNameIsValid()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        Assert.AreEqual(config.OutputStreamToNode("out1"), 0);
      }
    }
    #endregion

    #region RegisteredSidePacketTypeName
    [Test]
    public void RegisteredSidePacketTypeName_ShouldReturnInvalidArgumentError_When_TheSidePacketDoesNotExist()
    {
      using (var config = new ValidatedGraphConfig())
      {
        using (var statusOrString = config.RegisteredSidePacketTypeName("model_complexity"))
        {
          Assert.AreEqual(statusOrString.status.Code(), Status.StatusCode.InvalidArgument);
        }
      }
    }

    [Test]
    public void RegisteredSidePacketTypeName_ShouldReturnUnknownError_When_TheSidePacketTypeCannotBeDetermined()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PoseLandmarkConfigText)).AssertOk();
        using (var statusOrString = config.RegisteredSidePacketTypeName("model_complexity"))
        {
          Assert.AreEqual(statusOrString.status.Code(), Status.StatusCode.Unknown);
        }
      }
    }
    #endregion

    #region RegisteredStreamTypeName
    [Test]
    public void RegisteredStreamTypeName_ShouldReturnInvalidArgumentError_When_TheStreamDoesNotExist()
    {
      using (var config = new ValidatedGraphConfig())
      {
        using (var statusOrString = config.RegisteredStreamTypeName("in"))
        {
          Assert.AreEqual(statusOrString.status.Code(), Status.StatusCode.InvalidArgument);
        }
      }
    }

    [Test]
    public void RegisteredStreamTypeName_ShouldReturnUnknownError_When_TheStreamTypeCannotBeDetermined()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
        using (var statusOrString = config.RegisteredStreamTypeName("in"))
        {
          Assert.AreEqual(statusOrString.status.Code(), Status.StatusCode.Unknown);
        }
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
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText)).AssertOk();
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
        Assert.False(config.IsExternalSidePacket("model_complexity"));
      }
    }


    [Test]
    public void IsExternalSidePacket_ShouldReturnFalse_When_TheSidePacketIsInternal()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ConstantSidePacketConfigText)).AssertOk();
        Assert.False(config.IsExternalSidePacket("int_packet"));
      }
    }

    [Test]
    public void IsExternalSidePacket_ShouldReturnTrue_When_TheSidePacketIsExternal()
    {
      using (var config = new ValidatedGraphConfig())
      {
        config.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_PoseLandmarkConfigText)).AssertOk();
        Assert.True(config.IsExternalSidePacket("model_complexity"));
      }
    }
    #endregion
  }
}
