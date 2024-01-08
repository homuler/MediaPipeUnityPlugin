// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;

namespace Mediapipe.Tests
{
  public class CalculatorGraphTest
  {
    private const string _ValidConfigText = @"node {
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

    #region Constructor
    [Test]
    public void Ctor_ShouldInstantiateCalculatorGraph_When_CalledWithNoArguments()
    {
      Assert.DoesNotThrow(() =>
      {
        var graph = new CalculatorGraph();
        graph.Dispose();
      });
    }

    [Test]
    public void Ctor_ShouldInstantiateCalculatorGraph_When_CalledWithConfigText()
    {
      using (var graph = new CalculatorGraph(_ValidConfigText))
      {
        var config = graph.Config();
        Assert.AreEqual("in", config.InputStream[0]);
        Assert.AreEqual("out", config.OutputStream[0]);
      }
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var graph = new CalculatorGraph())
      {
        Assert.False(graph.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var graph = new CalculatorGraph();
      graph.Dispose();

      Assert.True(graph.isDisposed);
    }
    #endregion

    #region #Initialize
    [Test]
    public void Initialize_ShouldNotThrow_When_CalledWithConfig_And_ConfigIsNotSet()
    {
      using (var graph = new CalculatorGraph())
      {
        graph.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ValidConfigText));
        var config = graph.Config();

        Assert.AreEqual("in", config.InputStream[0]);
        Assert.AreEqual("out", config.OutputStream[0]);
      }
    }

    [Test]
    public void Initialize_ShouldReturnInternalError_When_CalledWithConfig_And_ConfigIsSet()
    {
      using (var graph = new CalculatorGraph(_ValidConfigText))
      {
        var exception = Assert.Throws<BadStatusException>(() =>
        {
          graph.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ValidConfigText));
        });
        Assert.AreEqual(StatusCode.Internal, exception.statusCode);
      }
    }

    [Test]
    public void Initialize_ShouldNotThrow_When_CalledWithConfigAndSidePacket_And_ConfigIsNotSet()
    {
      using (var sidePacket = new PacketMap())
      {
        sidePacket.Emplace("flag", Packet.CreateBool(true));

        using (var graph = new CalculatorGraph())
        {
          var config = CalculatorGraphConfig.Parser.ParseFromTextFormat(_ValidConfigText);

          Assert.DoesNotThrow(() => graph.Initialize(config, sidePacket));
        }
      }
    }

    [Test]
    public void Initialize_ShouldReturnInternalError_When_CalledWithConfigAndSidePacket_And_ConfigIsSet()
    {
      using (var sidePacket = new PacketMap())
      {
        sidePacket.Emplace("flag", Packet.CreateBool(true));

        using (var graph = new CalculatorGraph(_ValidConfigText))
        {
          var config = CalculatorGraphConfig.Parser.ParseFromTextFormat(_ValidConfigText);

          var exception = Assert.Throws<BadStatusException>(() => graph.Initialize(config, sidePacket));
          Assert.AreEqual(StatusCode.Internal, exception.statusCode);
        }
      }
    }
    #endregion

    #region lifecycle
    [Test]
    public void LifecycleMethods_ShouldControlGraphLifeCycle()
    {
      using (var graph = new CalculatorGraph(_ValidConfigText))
      {
        graph.StartRun();
        Assert.False(graph.GraphInputStreamsClosed());

        graph.WaitUntilIdle();
        graph.CloseAllPacketSources();
        Assert.True(graph.GraphInputStreamsClosed());
        graph.WaitUntilDone();
        Assert.False(graph.HasError());
      }
    }

    [Test]
    public void Cancel_ShouldCancelGraph()
    {
      using (var graph = new CalculatorGraph(_ValidConfigText))
      {
        graph.StartRun();
        graph.Cancel();
        var exception = Assert.Throws<BadStatusException>(() => graph.WaitUntilDone());
        Assert.AreEqual(StatusCode.Cancelled, exception.statusCode);
      }
    }
    #endregion
  }
}
