// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mediapipe.Tasks.Core
{
  internal class TaskInfo<T> where T : ITaskOptions
  {
    public string taskGraph { get; }
    public List<string> inputStreams { get; }
    public List<string> outputStreams { get; }
    public T taskOptions { get; }

    public TaskInfo(string taskGraph, List<string> inputStreams, List<string> outputStreams, T taskOptions)
    {
      this.taskGraph = taskGraph;
      this.inputStreams = inputStreams;
      this.outputStreams = outputStreams;
      this.taskOptions = taskOptions;
    }

    public CalculatorGraphConfig GenerateGraphConfig(bool enableFlowLimiting = false)
    {
      if (string.IsNullOrEmpty(taskGraph) || taskOptions == null)
      {
        throw new InvalidOperationException("Please provide both `task_graph` and `task_options`.");
      }
      if (inputStreams?.Count <= 0 || outputStreams?.Count <= 0)
      {
        throw new InvalidOperationException("Both `input_streams` and `output_streams` must be non-empty.");
      }

      if (!enableFlowLimiting)
      {
        return new CalculatorGraphConfig()
        {
          Node = {
            BuildConfigNode(taskGraph, taskOptions, inputStreams, outputStreams),
          },
          InputStream = { inputStreams },
          OutputStream = { outputStreams },
        };
      }

      var throttledInputStreams = inputStreams.Select(AddStreamNamePrefix);
      var finishedStream = $"FINISHED:{Tool.ParseNameFromStream(outputStreams.First())}";
      var flowLimiterOptions = new CalculatorOptions();
      flowLimiterOptions.SetExtension(FlowLimiterCalculatorOptions.Extensions.Ext, new FlowLimiterCalculatorOptions()
      {
        MaxInFlight = 1,
        MaxInQueue = 1,
      });

      return new CalculatorGraphConfig()
      {
        Node = {
          new CalculatorGraphConfig.Types.Node()
          {
            Calculator = "FlowLimiterCalculator",
            InputStreamInfo = {
              new InputStreamInfo()
              {
                TagIndex = "FINISHED",
                BackEdge = true,
              },
            },
            InputStream = { inputStreams.Select(Tool.ParseNameFromStream).Append(finishedStream) },
            OutputStream = { throttledInputStreams.Select(Tool.ParseNameFromStream) },
            Options = flowLimiterOptions,
          },
          BuildConfigNode(taskGraph, taskOptions, throttledInputStreams, outputStreams),
        },
        InputStream = { inputStreams },
        OutputStream = { outputStreams },
      };
    }

    private static string AddStreamNamePrefix(string tagIndexName)
    {
      Tool.ParseTagAndName(tagIndexName, out var tag, out var name);
      return $"{tag}:throttled_{name}";
    }

    private static CalculatorGraphConfig.Types.Node BuildConfigNode(string calculator, T taskOptions, IEnumerable<string> inputStreams, IEnumerable<string> outputStreams)
    {
      var node = new CalculatorGraphConfig.Types.Node()
      {
        Calculator = calculator,
        InputStream = { inputStreams },
        OutputStream = { outputStreams },
      };

      var calculatorOptions = taskOptions.ToCalculatorOptions();
      if (calculatorOptions != null)
      {
        node.Options = calculatorOptions;
        return node;
      }
      var anyOptions = taskOptions.ToAnyOptions();
      if (anyOptions != null)
      {
        node.NodeOptions.Add(anyOptions);
        return node;
      }

      throw new NotSupportedException($"{typeof(T)} cannot be converted to Calculator's options");
    }
  }
}
