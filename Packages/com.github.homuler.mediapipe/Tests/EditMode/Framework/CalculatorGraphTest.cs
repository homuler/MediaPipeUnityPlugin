// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using Mediapipe;
using NUnit.Framework;

namespace Tests
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
        Assert.AreEqual(config.InputStream[0], "in");
        Assert.AreEqual(config.OutputStream[0], "out");
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
    public void Initialize_ShouldReturnOk_When_CalledWithConfig_And_ConfigIsNotSet()
    {
      using (var graph = new CalculatorGraph())
      {
        using (var status = graph.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ValidConfigText)))
        {
          Assert.True(status.Ok());
        }

        var config = graph.Config();
        Assert.AreEqual(config.InputStream[0], "in");
        Assert.AreEqual(config.OutputStream[0], "out");
      }
    }

    [Test]
    public void Initialize_ShouldReturnInternalError_When_CalledWithConfig_And_ConfigIsSet()
    {
      using (var graph = new CalculatorGraph(_ValidConfigText))
      {
        using (var status = graph.Initialize(CalculatorGraphConfig.Parser.ParseFromTextFormat(_ValidConfigText)))
        {
          Assert.AreEqual(status.Code(), Status.StatusCode.Internal);
        }
      }
    }

    [Test]
    public void Initialize_ShouldReturnOk_When_CalledWithConfigAndSidePacket_And_ConfigIsNotSet()
    {
      using (var sidePacket = new SidePacket())
      {
        sidePacket.Emplace("flag", new BoolPacket(true));

        using (var graph = new CalculatorGraph())
        {
          var config = CalculatorGraphConfig.Parser.ParseFromTextFormat(_ValidConfigText);

          using (var status = graph.Initialize(config, sidePacket))
          {
            Assert.True(status.Ok());
          }
        }
      }
    }

    [Test]
    public void Initialize_ShouldReturnInternalError_When_CalledWithConfigAndSidePacket_And_ConfigIsSet()
    {
      using (var sidePacket = new SidePacket())
      {
        sidePacket.Emplace("flag", new BoolPacket(true));

        using (var graph = new CalculatorGraph(_ValidConfigText))
        {
          var config = CalculatorGraphConfig.Parser.ParseFromTextFormat(_ValidConfigText);

          using (var status = graph.Initialize(config, sidePacket))
          {
            Assert.AreEqual(status.Code(), Status.StatusCode.Internal);
          }
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
        Assert.True(graph.StartRun().Ok());
        Assert.False(graph.GraphInputStreamsClosed());

        Assert.True(graph.WaitUntilIdle().Ok());
        Assert.True(graph.CloseAllPacketSources().Ok());
        Assert.True(graph.GraphInputStreamsClosed());
        Assert.True(graph.WaitUntilDone().Ok());
        Assert.False(graph.HasError());
      }
    }

    [Test]
    public void Cancel_ShouldCancelGraph()
    {
      using (var graph = new CalculatorGraph(_ValidConfigText))
      {
        Assert.True(graph.StartRun().Ok());
        graph.Cancel();
        Assert.AreEqual(graph.WaitUntilDone().Code(), Status.StatusCode.Cancelled);
      }
    }
    #endregion
  }
}
