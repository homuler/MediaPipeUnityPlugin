# Hello World!

> :bell: If you're new to MediaPipe, consider reading the [Framework Concepts](https://ai.google.dev/edge/mediapipe/framework/framework_concepts/overview) article first.

> :skull_and_crossbones: On Windows, some of the code below might cause UnityEditor to crash. Check [Technical Limitations](../README.md#warning-technical-limitations) for more information.

Let's write our first program!

> :bell: The following code is based on [mediapipe/examples/desktop/examples/hello_world/hello_world.cc](https://github.com/google/mediapipe/blob/cf101e62a9d49a51be76836b2b8e5ba5c06b5da0/mediapipe/examples/desktop/hello_world/hello_world.cc).

> :bell: You can find the complete code at [Tutorial/Hello World](https://github.com/homuler/MediaPipeUnityPlugin/tree/master/Assets/MediaPipeUnity/Tutorial/Hello%20World).

## Send the input

To use the Calculators provided by MediaPipe, we typically need to set up a [`CalculatorGraph`](https://ai.google.dev/edge/mediapipe/framework/framework_concepts/graphs). Let's start with that!

> :bell: Each `CalculatorGraph` requires its own configuration ([`CalculatorGraphConfig`](https://ai.google.dev/edge/mediapipe/framework/framework_concepts/graphs#graph_config)).

```cs
var configText = @"
input_stream: ""in""
output_stream: ""out""
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
";

var graph = new CalculatorGraph(configText);
```

To run a `CalculatorGraph`, call the `StartRun` method.

```cs
graph.StartRun();
```

Note that the `StartRun` method will throw an exception if there is an error.

Now that we've started the graph, let's provide some input to the `CalculatorGraph`.

Let's say we want to give a sequence of 10 strings (`"Hello World!"`) as input.

```cs
for (var i = 0; i < 10; i++)
{
  // Send input to running graph
}
```

In MediaPipe, inputs are passed through a class called [`Packet`](https://ai.google.dev/edge/mediapipe/framework/framework_concepts/packets).

```cs
var input = Packet.CreateString("Hello World!");
```

To pass an input `Packet` to the `CalculatorGraph`, we use the `AddPacketToInputStream` method.\
In this example, our `CalculatorGraph` has a single input stream named `in`.

> :bell: It depends on the `CalculatorGraphConfig`. `CalculatorGraph` can have multiple input streams

```cs
for (var i = 0; i < 10; i++)
{
  var input = Packet.CreateString("Hello World!");
  graph.AddPacketToInputStream("in", input); // NOTE: Packet is disposed automatically
}
```

`CalculatorGraph#AddPacketToInputStream` may also throw an exception, for instance, when the input type is invalid.

When we're finished, we need to:

1. Close all input streams
2. Dispose of the `CalculatorGraph`

Here's how we do that:

```cs
using var graph = new CalculatorGraph(configText);
// ...
graph.CloseInputStream("in");
graph.WaitUntilDone();
```

For now, let's just run the code we've written so far.

Save the following code as `HelloWorld.cs`, attach it to an empty `GameObject` and play the scene.

```cs
using UnityEngine;

namespace Mediapipe.Unity.Tutorial
{
  public class HelloWorld : MonoBehaviour
  {
    private void Start()
    {
      var configText = @"
input_stream: ""in""
output_stream: ""out""
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
";
      using var graph = new CalculatorGraph(configText);
      graph.StartRun();

      for (var i = 0; i < 10; i++)
      {
        var input = Packet.CreateString("Hello World!");
        graph.AddPacketToInputStream("in", input);
      }

      graph.CloseInputStream("in");
      graph.WaitUntilDone();

      Debug.Log("Done");
    }
  }
}
```

![hello-world-timestamp-error](https://github.com/user-attachments/assets/4fc4416f-b254-4612-aa1b-b973a0dc7073)

Oops, an error occurred.

```txt
BadStatusException: INVALID_ARGUMENT: Graph has errors: 
; In stream "in", timestamp not specified or set to illegal value: Timestamp::Unset()
Mediapipe.Status.AssertOk () (at ./Packages/com.github.homuler.mediapipe/Runtime/Scripts/Framework/Port/Status.cs:168)
Mediapipe.MpResourceHandle.AssertStatusOk (System.IntPtr statusPtr) (at ./Packages/com.github.homuler.mediapipe/Runtime/Scripts/Core/MpResourceHandle.cs:115)
Mediapipe.CalculatorGraph.AddPacketToInputStream[T] (System.String streamName, Mediapipe.Packet`1[TValue] packet) (at ./Packages/com.github.homuler.mediapipe/Runtime/Scripts/Framework/CalculatorGraph.cs:167)
Mediapipe.Unity.Tutorial.HelloWorld.Start () (at Assets/MediaPipeUnity/Tutorial/Hello World/HelloWorld.cs:35)
```

Each input packet [should have a timestamp](https://ai.google.dev/edge/mediapipe/framework/framework_concepts/packets), but it does not appear to be set.

Let's fix the code that initializes a `Packet` as follows:

```cs
// var input = Packet.CreateString("Hello World!");
var input = Packet.CreateStringAt("Hello World!", i);
```

![hello-world-no-output](https://github.com/user-attachments/assets/d87247ef-2b13-4f0b-8c5d-4a652ec05f01)

This time it seems to be working.\
But wait, we are not receiving the `CalculatorGraph` output!

## Get the output

To receive output from the `CalculatorGraph`, we need to set up an output stream poller before running the graph.\
In this example, our graph has a single output stream named `out`.

> :bell: It depends on the `CalculatorGraphConfig`. `CalculatorGraph` can have multiple output streams.

```cs
using var graph = new CalculatorGraph(configText);

// Initialize an `OutputStreamPoller`. Note that the output type is string.
using var poller = graph.AddOutputStreamPoller<string>("out");

graph.StartRun();
```

Then, we can get output using the `OutputStreamPoller#Next`.\
Like inputs, outputs must be received through packets.

```cs
graph.CloseInputStream("in");

// Initialize an empty packet
using var output = new Packet<string>();

while (poller.Next(output))
{
  Debug.Log(output.Get());
}

graph.WaitUntilDone();
```

Now, our code would look as follows.
Note that `OutputStreamPoller` and `Packet` also need to be disposed of.

```cs
using UnityEngine;

namespace Mediapipe.Unity.Tutorial
{
  public class HelloWorld : MonoBehaviour
  {
    private void Start()
    {
      var configText = @"
input_stream: ""in""
output_stream: ""out""
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
";
      using var graph = new CalculatorGraph(configText);
      using var poller = graph.AddOutputStreamPoller<string>("out");
      graph.StartRun();

      for (var i = 0; i < 10; i++)
      {
        var input = Packet.CreateStringAt("Hello World!", i);
        graph.AddPacketToInputStream("in", input);
      }

      graph.CloseInputStream("in");

      using var output = new Packet<string>();
      while (poller.Next(output))
      {
        Debug.Log(output.Get());
      }

      graph.WaitUntilDone();

      Debug.Log("Done");
    }
  }
}
```

![hello-world-output](https://github.com/user-attachments/assets/60bddb98-ef95-4806-b2f6-ec6ba22be0e4)

## Validate the config format

What happens if the config format is invalid?

```cs
using var graph = new CalculatorGraph("invalid format");
```

![hello-world-invalid-config](https://github.com/user-attachments/assets/75220066-0532-4aef-94dc-eb40a094bcbd)

Hmm, the constructor fails, which is probably the behavior it should be.\
Let's check [`Editor.log`](https://docs.unity3d.com/Manual/LogFiles.html).

```txt
[libprotobuf ERROR external/com_google_protobuf/src/google/protobuf/text_format.cc:335] Error parsing text-format mediapipe.CalculatorGraphConfig: 1:9: Message type "mediapipe.CalculatorGraphConfig" has no field named "invalid".
MediaPipeException: Failed to parse config text. See error logs for more details
  at Mediapipe.CalculatorGraphConfigExtension.ParseFromTextFormat (Google.Protobuf.MessageParser`1[T] _, System.String configText) [0x0001e] in ./Packages/com.github.homuler.mediapipe/Runtime/Scripts/Framework/CalculatorGraphConfigExtension.cs:21 
  at Mediapipe.CalculatorGraph..ctor (System.String textFormatConfig) [0x00000] in ./Packages/com.github.homuler.mediapipe/Runtime/Scripts/Framework/CalculatorGraph.cs:33 
  at Mediapipe.Unity.Tutorial.HelloWorld.Start () [0x00000] in /home/homuler/Development/unity/MediaPipeUnityPlugin/Assets/MediaPipeUnity/Tutorial/Hello World/HelloWorld.cs:29 
```

Not too bad, but it's inconvenient to check `Editor.log` every time.\
Let's fix it so that the logs are visible in the Console Window.

```cs
Protobuf.SetLogHandler(Protobuf.DefaultLogHandler);
var graph = new CalculatorGraph("invalid format");
```

![hello-world-protobuf-logger](https://github.com/user-attachments/assets/03956a07-ba77-45a0-9479-bb5f3dd8b2c1)

Great!\
However, there's an important detail to handle: we need to reset the log handler when the application exits to prevent potential crashes (SIGSEGV).\
Add this code to restore the default `LogHandler`:

```cs
void OnApplicationQuit()
{
  Protobuf.ResetLogHandler();
}
```
