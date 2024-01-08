// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using Mediapipe.Tasks.Core;
using NUnit.Framework;

namespace Mediapipe.Tests.Tasks.Core
{
  public class TaskRunnerTest
  {
    private const string _PassThroughConfigText = @"node {
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

    private CalculatorGraphConfig passThroughConfig => CalculatorGraphConfig.Parser.ParseFromTextFormat(_PassThroughConfigText);

    #region Create
    [Test]
    public void Create_ShouldThrowException_When_CalledWithInvalidConfig()
    {
      var exception = Assert.Throws<BadStatusException>(() => TaskRunner.Create(new CalculatorGraphConfig()));
      Assert.AreEqual(StatusCode.InvalidArgument, exception.statusCode);
    }

    [Test]
    public void Create_ShouldInstantiateTaskRunner_When_CalledWithValidConfig()
    {
      Assert.DoesNotThrow(() =>
      {
        var taskRunner = TaskRunner.Create(passThroughConfig);
        taskRunner.Dispose();
      });
    }
    #endregion

    #region #isDisposed
    [Test]
    public void IsDisposed_ShouldReturnFalse_When_NotDisposedYet()
    {
      using (var taskRunner = TaskRunner.Create(passThroughConfig))
      {
        Assert.False(taskRunner.isDisposed);
      }
    }

    [Test]
    public void IsDisposed_ShouldReturnTrue_When_AlreadyDisposed()
    {
      var taskRunner = TaskRunner.Create(passThroughConfig);
      taskRunner.Dispose();

      Assert.True(taskRunner.isDisposed);
    }
    #endregion

    #region #Process
    [Test]
    public void Process_ShouldThrowException_When_InputIsInvalid()
    {
      using (var taskRunner = TaskRunner.Create(passThroughConfig))
      {
        var packetMap = new PacketMap();
        var exception = Assert.Throws<BadStatusException>(() => taskRunner.Process(packetMap));
        Assert.AreEqual(StatusCode.InvalidArgument, exception.statusCode);
        Assert.True(packetMap.isDisposed);
      }
    }

    [Test]
    public void Process_ShouldReturnOutput_When_InputIsValid()
    {
      using (var taskRunner = TaskRunner.Create(passThroughConfig))
      {
        var packetMap = new PacketMap();
        packetMap.Emplace("in", Packet.CreateInt(1));

        var outputMap = taskRunner.Process(packetMap);
        Assert.AreEqual(1, outputMap.At<int>("out").Get());
        Assert.True(packetMap.isDisposed);
      }
    }
    #endregion

    #region #Send
    [Test]
    public void Send_ShouldThrowException_When_CallbackIsNotSet()
    {
      using (var taskRunner = TaskRunner.Create(passThroughConfig))
      {
        var packetMap = new PacketMap();
        packetMap.Emplace("in", Packet.CreateIntAt(1, 1));

        var exception = Assert.Throws<BadStatusException>(() => taskRunner.Send(packetMap));
        Assert.AreEqual(StatusCode.InvalidArgument, exception.statusCode);
        Assert.True(packetMap.isDisposed);
      }
    }

    [Test]
    public void Send_ShouldThrowException_When_InputIsInvalid()
    {
      using (var taskRunner = TaskRunner.Create(passThroughConfig, 0, HandlePassThroughResult))
      {
        var packetMap = new PacketMap();
        var exception = Assert.Throws<BadStatusException>(() => taskRunner.Send(packetMap));
        Assert.AreEqual(StatusCode.InvalidArgument, exception.statusCode);
        Assert.True(packetMap.isDisposed);
      }
    }

    [Test]
    public void Send_ShouldNotThrowException_When_InputIsValid()
    {
      using (var taskRunner = TaskRunner.Create(passThroughConfig, 0, HandlePassThroughResult))
      {
        var packetMap = new PacketMap();
        packetMap.Emplace("in", Packet.CreateIntAt(1, 1));
        Assert.DoesNotThrow(() => taskRunner.Send(packetMap));
        Assert.True(packetMap.isDisposed);
      }
    }
    #endregion

    #region #Close
    [Test]
    public void Close_ShouldNotThrowException_When_NotClosed()
    {
      using (var taskRunner = TaskRunner.Create(passThroughConfig))
      {
        Assert.DoesNotThrow(() => taskRunner.Close());
      }
    }

    [Test]
    public void Close_ShouldThrowException_When_AlreadyClosed()
    {
      using (var taskRunner = TaskRunner.Create(passThroughConfig))
      {
        taskRunner.Close();
        var exception = Assert.Throws<BadStatusException>(() => taskRunner.Close());
        Assert.AreEqual(StatusCode.InvalidArgument, exception.statusCode);
      }
    }
    #endregion

    #region #Restart
    [Test]
    public void Restart_ShouldNotThrowException_When_Running()
    {
      using (var taskRunner = TaskRunner.Create(passThroughConfig))
      {
        Assert.DoesNotThrow(() => taskRunner.Restart());
      }
    }

    [Test]
    public void Restart_ShouldThrowException_When_Closed()
    {
      using (var taskRunner = TaskRunner.Create(passThroughConfig))
      {
        taskRunner.Close();
        var exception = Assert.Throws<BadStatusException>(() => taskRunner.Restart());
        Assert.AreEqual(StatusCode.InvalidArgument, exception.statusCode);
      }
    }
    #endregion

    #region #GetGraphConfig
    [Test]
    public void GetGraphConfig_ShouldReturnCanonicalizedConfig()
    {
      using (var taskRunner = TaskRunner.Create(passThroughConfig))
      {
        var config = taskRunner.GetGraphConfig();
        Assert.AreEqual(233, config.CalculateSize());
      }
    }
    #endregion

    [AOT.MonoPInvokeCallback(typeof(TaskRunner.NativePacketsCallback))]
    private static void HandlePassThroughResult(int callbackId, IntPtr statusPtr, IntPtr packetMapPtr)
    {
      // Do nothing
    }
  }
}
