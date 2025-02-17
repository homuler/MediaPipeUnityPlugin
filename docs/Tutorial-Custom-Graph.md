# Tutorial: Custom Graph

> :warning: This is now referred to as a [Legacy Solution](https://ai.google.dev/edge/mediapipe/solutions/guide#legacy), and it is generally recommended to use the Task API.
> However, running a custom CalculatorGraph is still supported, and this document will explain how to do it.

In this tutorial, we will demonstrate how to run the Face Mesh Legacy Solution.
If you have not read the [Hello World tutorial](./Tutorial-Hello-World.md), please try it first.

> :skull_and_crossbones: On Windows, some of the code below might cause UnityEditor to crash. Check [Technical Limitations](../README.md#warning-technical-limitations) for more information.

> :bell: A scene for this tutorial is available under `Tutorial/Custom Graph`.

## Display Webcam Images

Let's start by displaying the camera image using [`WebCamTexture`](https://docs.unity3d.com/ScriptReference/WebCamTexture.html).
The following script is already provided in `Tutorial/Custom Graph/FaceMeshLegacy.cs`.

```cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.Tutorial
{
  public class FaceMeshLegacy : MonoBehaviour
  {
    [SerializeField] private TextAsset configAsset;
    [SerializeField] private RawImage screen;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int fps;

    private WebCamTexture webCamTexture;

    private IEnumerator Start()
    {
      if (WebCamTexture.devices.Length == 0)
      {
        throw new System.Exception("Web Camera devices are not found");
      }
      var webCamDevice = WebCamTexture.devices[0];
      webCamTexture = new WebCamTexture(webCamDevice.name, width, height, fps);
      webCamTexture.Play();

      // NOTE: On macOS, the contents of webCamTexture may not be readable immediately, so wait until it is readable
      yield return new WaitUntil(() => webCamTexture.width > 16);

      screen.rectTransform.sizeDelta = new Vector2(width, height);
      screen.texture = webCamTexture;
    }

    private void OnDestroy()
    {
      if (webCamTexture != null)
      {
        webCamTexture.Stop();
      }
    }
  }
}
```

If everything is set up correctly, your screen should look like this.

![face-mesh-webcam](https://github.com/user-attachments/assets/abbf556b-4d23-4bba-a829-d865e20e0bc1)

By default, it operates at 1280x720 (30fps), but if it does not work with this setting, change it to a setting value supported by your webcam.

Now let's try [`face_mesh_desktop_live.pbtxt`](https://github.com/google/mediapipe/blob/c6c80c37452d0938b1577bd1ad44ad096ca918e0/mediapipe/graphs/face_mesh/face_mesh_desktop_live.pbtxt), the official Face Mesh Legacy Solution!

> :bell: `face_mesh_desktop_live.pbtxt` is saved as `Tutorial/Custom Graph/face_mesh_desktop_live.txt`.

## Initialize CalculatorGraph

First, initialize a [`CalculatorGraph`](https://ai.google.dev/edge/mediapipe/framework/framework_concepts/graphs) similar to the [Hello World tutorial](./Tutorial-Hello-World.md).

```cs
TextAsset configAsset;

var graph = new CalculatorGraph(configAsset.text);
graph.StartRun();
```

## Prepare Data

In MediaPipe, image data on the CPU is stored in a class called `ImageFrame`.\
Let's initialize an `ImageFrame` instance from the `WebCamTexture` image.

> :bell: On the other hand, image data on the GPU is stored in a class called `GpuBuffer`.

Copy the data from `WebCamTexture` to a `Texture2D`, and then create an `ImageFrame` from the `Texture2D`.

Here, [as with the Task API](./Tutorial-Task-API.md#flip-the-input-image), we use `TextureFrame`. Note that it is flipped vertically to account for differences in coordinate systems.

```cs
using var textureFrame = new Experimental.TextureFrame(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32);
textureFrame.ReadTextureOnCPU(webCamTexture, flipHorizontally: false, flipVertically: true);
using var imageFrame = new ImageFrame(tmpTexture);
```

> :warning: In theory, you can build `ImageFrame` instances using various formats, but not all `Calculator`s necessarily support all formats. As for legacy solutions, they often work only with RGBA32 format.

As usual, initialize a [`Packet`](https://ai.google.dev/edge/mediapipe/framework/framework_concepts/packets) and send it to the `CalculatorGraph`.\
Note that the input stream name is `"input_video"` and the input type is `ImageFrame` this time.

```cs
graph.AddPacketToInputStream("input_video", Packet.CreateImageFrame(imageFrame));
```

> :warning: Make sure the stream name matches the one defined in your graph configuration.

We should stop the `CalculatorGraph` on the `OnDestroy` event.\
With a little refactoring, the code now looks like this.

```cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.Tutorial
{
  public class FaceMeshLegacy : MonoBehaviour
  {
    [SerializeField] private TextAsset configAsset;
    [SerializeField] private RawImage screen;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int fps;

    private CalculatorGraph graph;

    private WebCamTexture webCamTexture;

    private IEnumerator Start()
    {
      if (WebCamTexture.devices.Length == 0)
      {
        throw new System.Exception("Web Camera devices are not found");
      }
      var webCamDevice = WebCamTexture.devices[0];
      webCamTexture = new WebCamTexture(webCamDevice.name, width, height, fps);
      webCamTexture.Play();

      yield return new WaitUntil(() => webCamTexture.width > 16);

      screen.rectTransform.sizeDelta = new Vector2(width, height);
      screen.texture = webCamTexture;

      graph = new CalculatorGraph(configAsset.text);
      graph.StartRun();

      using var textureFrame = new Experimental.TextureFrame(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32);

      while (true)
      {
        textureFrame.ReadTextureOnCPU(webCamTexture, flipHorizontally: false, flipVertically: true);
        using var imageFrame = textureFrame.BuildImageFrame();
        graph.AddPacketToInputStream("input_video", Packet.CreateImageFrame(imageFrame));

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnDestroy()
    {
      if (webCamTexture != null)
      {
        webCamTexture.Stop();
      }

      if (graph != null)
      {
        try
        {
          graph.CloseInputStream("input_video");
          graph.WaitUntilDone();
        }
        finally
        {
          graph.Dispose();
          graph = null;
        }
      }
    }
  }
}
```

Let's play the scene!

![face-mesh-load-fail](https://github.com/user-attachments/assets/7c7649c1-6249-4b80-b5b0-5d6ebc97c6fd)

Well, it's not so easy, is it?

```txt
BadStatusException: NOT_FOUND: Graph has errors: 
Calculator::Open() for node "facelandmarkfrontgpu__facelandmarkgpu__facelandmarksmodelloader__ResourceProviderCalculator" failed: Failed to load resource: mediapipe/modules/face_landmark/face_landmark_with_attention.tflite
Can't find file: mediapipe/modules/face_landmark/face_landmark_with_attention.tflite
Can't find file: mediapipe/modules/face_landmark/face_landmark_with_attention.tflite
Calculator::Open() for node "facelandmarkfrontgpu__facedetectionshortrangegpu__facedetectionshortrange__facedetection__inferencecalculator__facelandmarkfrontgpu__facedetectionshortrangegpu__facedetectionshortrange__facedetection__InferenceCalculator" failed: Failed to load resource: mediapipe/modules/face_detection/face_detection_short_range.tflite
Can't find file: mediapipe/modules/face_detection/face_detection_short_range.tflite
Can't find file: mediapipe/modules/face_detection/face_detection_short_range.tflite
Mediapipe.Status.AssertOk () (at ./Packages/com.github.homuler.mediapipe/Runtime/Scripts/Framework/Port/Status.cs:168)
Mediapipe.MpResourceHandle.AssertStatusOk (System.IntPtr statusPtr) (at ./Packages/com.github.homuler.mediapipe/Runtime/Scripts/Core/MpResourceHandle.cs:115)
Mediapipe.CalculatorGraph.AddPacketToInputStream[T] (System.String streamName, Mediapipe.Packet`1[TValue] packet) (at ./Packages/com.github.homuler.mediapipe/Runtime/Scripts/Framework/CalculatorGraph.cs:167)
Mediapipe.Unity.Tutorial.FaceMesh+<Start>d__7.MoveNext () (at Assets/MediaPipeUnity/Tutorial/Custom Graph/FaceMesh.cs:45)
UnityEngine.SetupCoroutine.InvokeMoveNext (System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at /home/bokken/build/output/unity/unity/Runtime/Export/Scripting/Coroutines.cs:17)
```

It appears that `LocalFileContentsCalculator` failed to load `face_landmark_with_attention.tflite`.\
In the next section, we will resolve this error.

### :warning: MediaPipe Aborted
If you get error messages like the following, read the Editor.log.
```txt
MediaPipeException: MediaPipe Aborted, refer glog files for more details
```

If it says something like the following, try `face_mesh_desktop_live_gpu.pbtxt` instead of `face_mesh_desktop_live.pbtxt`.

```txt
F0000 00:00:1739782582.369034  548617 calculator_graph.cc:157] Non-OK-status: Initialize(std::move(config)) status: NOT_FOUND: ValidatedGraphConfig Initialization failed.
No registered object with name: FaceLandmarkFrontCpu; Unable to find Calculator "FaceLandmarkFrontCpu"
No registered object with name: FaceRendererCpu; Unable to find Calculator "FaceRendererCpu"
```

If the error persists, it means the necessary calculators are not included in the library, and you will need to [build the native library yourself](./Build.md).

If you use `face_mesh_desktop_live_gpu.pbtxt`, additional GPU-specific settings are required. Please refer to the [GPU support section](#gpu-support) as you proceed.

### Load model files

To load model files on Unity, we need to resolve their paths because they are usually hardcoded.\
Additionally, we need to _save_ the file to a specific path because some calculators read required resources directly from the file system.

> :bulb: The path to save is not fixed since we can translate each model path into an arbitrary path.

But don't worry. In most cases, all you need to do is initialize an `IResourceManager` class and call the `PrepareAssetAsync` method in advance.

> :bulb: `PrepareAssetAsync` method will save the specified file under `Application.persistentDataPath`.

For testing purposes, the `LocalResourceManager` class is sufficient.

```cs
var resourceManager = new LocalResourceManager();
yield return resourceManager.PrepareAssetAsync("dependent_asset_name");
```

In development / production, you can choose either `StreamingAssetResourceManager` or `AssetBundleResourceManager`.\
For example, `StreamingAssetResourceManager` will load model files from [`Application.streamingAssetsPath`](https://docs.unity3d.com/Manual/StreamingAssets.html).

> :warning: To use `StreamingAssetResourceManager`, you need to place dependent assets under `Assets/StreamingAssets`. You can usually copy those assets from `Packages/com.github.homuler.mediapipe/PackageResources`.

```cs
IResourceManager resourceManager = new StreamingAssetsResourceManager();
yield return resourceManager.PrepareAssetAsync("dependent_asset_name");
```

Now, let's get back to the code.

After trial and error, we find that we need to prepare files `face_detection_short_range.tflite` and `face_landmark_with_attention.tflite`.\
Unity does not support [`.tflite` extension](https://docs.unity3d.com/Manual/class-TextAsset.html), so this plugin adopts the `.bytes` extension instead.

> :bell: A `.bytes` file has the same contents as the corresponding `.tflite` file, just with a different extension.

Now the entire code will look like this.

```cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.Tutorial
{
  public class FaceMeshLegacy : MonoBehaviour
  {
    [SerializeField] private TextAsset configAsset;
    [SerializeField] private RawImage screen;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int fps;

    private CalculatorGraph graph;

    private WebCamTexture webCamTexture;

    private IEnumerator Start()
    {
      if (WebCamTexture.devices.Length == 0)
      {
        throw new System.Exception("Web Camera devices are not found");
      }
      var webCamDevice = WebCamTexture.devices[0];
      webCamTexture = new WebCamTexture(webCamDevice.name, width, height, fps);
      webCamTexture.Play();

      yield return new WaitUntil(() => webCamTexture.width > 16);

      screen.rectTransform.sizeDelta = new Vector2(width, height);
      screen.texture = webCamTexture;

      IResourceManager resourceManager = new LocalResourceManager();
      yield return resourceManager.PrepareAssetAsync("face_detection_short_range.bytes");
      yield return resourceManager.PrepareAssetAsync("face_landmark_with_attention.bytes");

      graph = new CalculatorGraph(configAsset.text);
      graph.StartRun();

      using var textureFrame = new Experimental.TextureFrame(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32);

      while (true)
      {
        textureFrame.ReadTextureOnCPU(webCamTexture, flipHorizontally: false, flipVertically: true);
        using var imageFrame = textureFrame.BuildImageFrame();
        graph.AddPacketToInputStream("input_video", Packet.CreateImageFrame(imageFrame));

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnDestroy()
    {
      if (webCamTexture != null)
      {
        webCamTexture.Stop();
      }

      if (graph != null)
      {
        try
        {
          graph.CloseInputStream("input_video");
          graph.WaitUntilDone();
        }
        finally
        {
          graph.Dispose();
          graph = null;
        }
      }
    }
  }
}
```

What will be the result this time...?

![face-mesh-no-timestamp](https://github.com/user-attachments/assets/d7fc9468-0daf-42b6-8ebc-65c720096112)

Oops, we forgot to set the timestamp.\
But what value should be set for the timestamp?

### Set the correct timestamp

In the [Hello World tutorial](./Tutorial-Hello-World.md), the loop variable `i` was set to the value of timestamp.\
In practice, however, MediaPipe assumes that the value of timestamp is in microseconds (cf.[mediapipe/framework/timestamp.h](https://github.com/google/mediapipe/blob/c6c80c37452d0938b1577bd1ad44ad096ca918e0/mediapipe/framework/timestamp.h#L85-L87)).

> :bell: There are calculators that care about the absolute value of the timestamp. Such calculators will behave unintentionally if the timestamp value is not in microseconds.

Let's use the elapsed time in microseconds since startup as a timestamp.

```cs
using Stopwatch = System.Diagnostics.Stopwatch;

var stopwatch = new Stopwatch();
stopwatch.Start();

var currentTimestamp = stopwatch.ElapsedTicks / ((double)System.TimeSpan.TicksPerMillisecond / 1000);
```

And the entire code:

```cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.Tutorial
{
  public class FaceMeshLegacy : MonoBehaviour
  {
    [SerializeField] private TextAsset configAsset;
    [SerializeField] private RawImage screen;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int fps;

    private CalculatorGraph graph;

    private WebCamTexture webCamTexture;

    private IEnumerator Start()
    {
      if (WebCamTexture.devices.Length == 0)
      {
        throw new System.Exception("Web Camera devices are not found");
      }
      var webCamDevice = WebCamTexture.devices[0];
      webCamTexture = new WebCamTexture(webCamDevice.name, width, height, fps);
      webCamTexture.Play();

      yield return new WaitUntil(() => webCamTexture.width > 16);

      screen.rectTransform.sizeDelta = new Vector2(width, height);
      screen.texture = webCamTexture;

      IResourceManager resourceManager = new LocalResourceManager();
      yield return resourceManager.PrepareAssetAsync("face_detection_short_range.bytes");
      yield return resourceManager.PrepareAssetAsync("face_landmark_with_attention.bytes");

      graph = new CalculatorGraph(configAsset.text);
      graph.StartRun();

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      using var textureFrame = new Experimental.TextureFrame(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32);

      while (true)
      {
        textureFrame.ReadTextureOnCPU(webCamTexture, flipHorizontally: false, flipVertically: true);
        using var imageFrame = textureFrame.BuildImageFrame();

        var currentTimestamp = stopwatch.ElapsedTicks / ((double)System.TimeSpan.TicksPerMillisecond / 1000);
        graph.AddPacketToInputStream("input_video", Packet.CreateImageFrameAt(imageFrame, (long)currentTimestamp));

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnDestroy()
    {
      if (webCamTexture != null)
      {
        webCamTexture.Stop();
      }

      if (graph != null)
      {
        try
        {
          graph.CloseInputStream("input_video");
          graph.WaitUntilDone();
        }
        finally
        {
          graph.Dispose();
          graph = null;
        }
      }
    }
  }
}
```

![face-mesh-success](https://github.com/user-attachments/assets/1da44b8d-427c-47d5-8899-8e9b2c6b8c93)

Now, it seems to be working.\
But of course, we want to receive output next.

### Get `ImageFrame`

In the Hello World example, we initialized `OutputStreamPoller` using `CalculatorGraph#AddOutputStreamPoller`.\
This time, to handle output more easily, let's use the **OutputStream API** provided by the plugin instead!

```cs
var graph = new CalculatorGraph(_configAsset.text);
var outputVideoStream = new OutputStreasm<ImageFrame>(graph, "output_video");
```

Before running the `CalculatorGraph`, call `StartPolling`.

```cs
outputVideoStream.StartPolling();
graph.StartRun();
```

To get the next output, call `WaitNextAsync`.\
It returns when the next output is retrieved or some error occurred.

```cs
var task = outputVideoStream.WaitNextAsync();
yield return new WaitUntil(() => task.IsCompleted);

if (task.Result.ok)
{
  // ...
}
```

Now, let's display the output image directly on the screen.\
We can read the pixel data using `ImageFrame#TryReadPixelData`.

```cs
// NOTE: `TryReadPixelData` is implemented in `Mediapipe.Unity.ImageFrameExtension`.
// using Mediapipe.Unity;

var outputTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
var outputPixelData = new Color32[width * height];
screen.texture = outputTexture;

var task = outputVideoStream.WaitNextAsync();
yield return new WaitUntil(() => task.IsCompleted);

if (!task.Result.ok)
{
  throw new System.Exception("Something went wrong");
}

var outputPacket = task.Result.packet;

if (outputPacket != null)
{
  var outputVideo = outputPacket.Get();

  if (outputVideo.TryReadPixelData(outputPixelData))
  {
    outputTexture.SetPixels32(outputPixelData);
    outputTexture.Apply();
  }
}
```

Now our code should look something like this.

```cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.Tutorial
{
  public class FaceMeshLegacy : MonoBehaviour
  {
    [SerializeField] private TextAsset configAsset;
    [SerializeField] private RawImage screen;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int fps;

    private CalculatorGraph graph;
    private OutputStream<ImageFrame> outputVideoStream;

    private WebCamTexture webCamTexture;

    private IEnumerator Start()
    {
      if (WebCamTexture.devices.Length == 0)
      {
        throw new System.Exception("Web Camera devices are not found");
      }
      var webCamDevice = WebCamTexture.devices[0];
      webCamTexture = new WebCamTexture(webCamDevice.name, width, height, fps);
      webCamTexture.Play();

      yield return new WaitUntil(() => webCamTexture.width > 16);

      var outputTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
      var outputPixelData = new Color32[width * height];

      screen.rectTransform.sizeDelta = new Vector2(width, height);
      screen.texture = outputTexture;

      IResourceManager resourceManager = new LocalResourceManager();
      yield return resourceManager.PrepareAssetAsync("face_detection_short_range.bytes");
      yield return resourceManager.PrepareAssetAsync("face_landmark_with_attention.bytes");

      graph = new CalculatorGraph(configAsset.text);
      outputVideoStream = new OutputStream<ImageFrame>(graph, "output_video");
      outputVideoStream.StartPolling();
      graph.StartRun();

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      using var textureFrame = new Experimental.TextureFrame(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32);

      while (true)
      {
        textureFrame.ReadTextureOnCPU(webCamTexture, flipHorizontally: false, flipVertically: true);
        using var imageFrame = textureFrame.BuildImageFrame();

        var currentTimestamp = stopwatch.ElapsedTicks / ((double)System.TimeSpan.TicksPerMillisecond / 1000);
        graph.AddPacketToInputStream("input_video", Packet.CreateImageFrameAt(imageFrame, (long)currentTimestamp));

        var task = outputVideoStream.WaitNextAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (!task.Result.ok)
        {
          throw new System.Exception("Something went wrong");
        }

        var outputPacket = task.Result.packet;
        if (outputPacket != null)
        {
          var outputVideo = outputPacket.Get();

          if (outputVideo.TryReadPixelData(outputPixelData))
          {
            outputTexture.SetPixels32(outputPixelData);
            outputTexture.Apply();
          }
        }
      }
    }

    private void OnDestroy()
    {
      if (webCamTexture != null)
      {
        webCamTexture.Stop();
      }

      outputVideoStream?.Dispose();
      outputVideoStream = null;

      if (graph != null)
      {
        try
        {
          graph.CloseInputStream("input_video");
          graph.WaitUntilDone();
        }
        finally
        {
          graph.Dispose();
          graph = null;
        }
      }
    }
  }
}
```

Let's try running!

![face-mesh-cpu](https://github.com/user-attachments/assets/1cef311b-bfda-4318-aaa5-17f108c08e64)

If you see the annotated image, it's successful!

> :bell: `ImageFrame#TryReadPixelData` automatically reads pixels upside down, so the output image is received correctly.

#### Load `ImageFrame` fast

We used `ImageFrame#TryReadPixelData` and `Texture2D#SetPixels32` to render the output image.

However, using `Texture2D#LoadRawTextureData`, we can do it a little faster.

```cs
outputTexture.LoadRawTextureData(outputVideo.MutablePixelData(), outputVideo.PixelDataSize());
outputTexture.Apply();
```

This time, the orientation of the image must be adjusted by yourself.

### Get landmarks

This time, let's try to get landmark positions from the `"multi_face_landmarks"` stream.

The output type is `List<NormalizedLandmarkList>` (`std::vector<NormalizedLandmarkList>` in C++), so we can initialize the `OutputStream` like this.

```cs
var multiFaceLandmarksStream = new OutputStream<List<NormalizedLandmarkList>>(graph, "multi_face_landmarks");
multiFaceLandmarksStream.StartPolling();
```

As with the `"output_video"` stream, we can receive the result using `OutputStream#WaitNextAsync`.

```cs
var task = multiFaceLandmarksStream.WaitNextAsync();
yield return new WaitUntil(() => task.IsCompleted);
```

Note that the output values are in the Image Coordinate System.\
To convert them to the Unity local coordinates, you can use methods defined in `ImageCoordinateSystem`.

Please also refer to the [Task API tutorial](./Tutorial-Task-API.md#get-landmark-coordinates) for more details.

> :bell: Which coordinate system the output is based on depends on the solution.

```cs
// using Mediapipe.Unity.CoordinateSystem;

var screenRect = screen.rectTransform.rect;
var position = screenRect.GetPoint(normalizedLandmark);
```

## GPU Support

> :warning: To test the code in this section, you need to build native libraries with GPU enabled.

When you built native libraries with GPU enabled, you need to initialize GPU resources before running the `CalculatorGraph`.

```cs
yield return GpuManager.Initialize();

if (!GpuManager.IsInitialized)
{
  throw new System.Exception("Failed to initialize GPU resources");
}

graph = new CalculatorGraph(configAsset.text);
graph.SetGpuResources(GpuManager.GpuResources);
```

Note that you need to dispose of GPU resources when the program exits.

```cs
private void OnDestroy()
{
  GpuManager.Shutdown();
}
```

The remaining steps are the same as those for the CPU.

Here is a sample code.
Before running, don't forget to set `face_mesh_desktop_live_gpu.txt` to `configAsset`.

> :bell: `face_mesh_desktop_live_gpu.pbtxt` is saved as `Tutorial/Custom Graph/face_mesh_desktop_live_gpu.txt`.

```cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.Tutorial
{
  public class FaceMeshLegacy : MonoBehaviour
  {
    [SerializeField] private TextAsset configAsset;
    [SerializeField] private RawImage screen;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int fps;

    private CalculatorGraph graph;
    private OutputStream<ImageFrame> outputVideoStream;

    private WebCamTexture webCamTexture;

    private IEnumerator Start()
    {
      if (WebCamTexture.devices.Length == 0)
      {
        throw new System.Exception("Web Camera devices are not found");
      }
      var webCamDevice = WebCamTexture.devices[0];
      webCamTexture = new WebCamTexture(webCamDevice.name, width, height, fps);
      webCamTexture.Play();

      yield return new WaitUntil(() => webCamTexture.width > 16);

      yield return GpuManager.Initialize();
      if (!GpuManager.IsInitialized)
      {
        throw new System.Exception("Failed to initialize GPU resources");
      }

      var outputTexture = new Texture2D(width, height, TextureFormat.RGBA32, false);
      var outputPixelData = new Color32[width * height];

      screen.rectTransform.sizeDelta = new Vector2(width, height);
      screen.texture = outputTexture;

      IResourceManager resourceManager = new LocalResourceManager();
      yield return resourceManager.PrepareAssetAsync("face_detection_short_range.bytes");
      yield return resourceManager.PrepareAssetAsync("face_landmark_with_attention.bytes");

      graph = new CalculatorGraph(configAsset.text);
      graph.SetGpuResources(GpuManager.GpuResources);
      outputVideoStream = new OutputStream<ImageFrame>(graph, "output_video");
      outputVideoStream.StartPolling();
      graph.StartRun();

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      using var textureFrame = new Experimental.TextureFrame(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32);

      while (true)
      {
        textureFrame.ReadTextureOnCPU(webCamTexture, flipHorizontally: false, flipVertically: true);
        using var imageFrame = textureFrame.BuildImageFrame();

        var currentTimestamp = stopwatch.ElapsedTicks / ((double)System.TimeSpan.TicksPerMillisecond / 1000);
        graph.AddPacketToInputStream("input_video", Packet.CreateImageFrameAt(imageFrame, (long)currentTimestamp));

        var task = outputVideoStream.WaitNextAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (!task.Result.ok)
        {
          throw new System.Exception("Something went wrong");
        }

        var outputPacket = task.Result.packet;
        if (outputPacket != null)
        {
          var outputVideo = outputPacket.Get();

          if (outputVideo.TryReadPixelData(outputPixelData))
          {
            outputTexture.SetPixels32(outputPixelData);
            outputTexture.Apply();
          }
        }
      }
    }

    private void OnDestroy()
    {
      if (webCamTexture != null)
      {
        webCamTexture.Stop();
      }

      outputVideoStream?.Dispose();
      outputVideoStream = null;

      if (graph != null)
      {
        try
        {
          graph.CloseInputStream("input_video");
          graph.WaitUntilDone();
        }
        finally
        {
          graph.Dispose();
          graph = null;
        }
      }

      GpuManager.Shutdown();
    }
  }
}
```

Let's try to run the sample scene.

```txt
BadStatusException: INVALID_ARGUMENT: Graph has errors: 
Packet type mismatch on calculator outputting to stream "input_video": The Packet stores "mediapipe::ImageFrame", but "mediapipe::GpuBuffer" was requested.
Mediapipe.Status.AssertOk () (at ./Packages/com.github.homuler.mediapipe/Runtime/Scripts/Framework/Port/Status.cs:168)
Mediapipe.MpResourceHandle.AssertStatusOk (System.IntPtr statusPtr) (at ./Packages/com.github.homuler.mediapipe/Runtime/Scripts/Core/MpResourceHandle.cs:115)
Mediapipe.CalculatorGraph.AddPacketToInputStream[T] (System.String streamName, Mediapipe.Packet`1[TValue] packet) (at ./Packages/com.github.homuler.mediapipe/Runtime/Scripts/Framework/CalculatorGraph.cs:167)
Mediapipe.Unity.Tutorial.FaceMesh+<Start>d__8.MoveNext () (at Assets/MediaPipeUnity/Tutorial/Custom Graph/FaceMesh.cs:67)
UnityEngine.SetupCoroutine.InvokeMoveNext (System.Collections.IEnumerator enumerator, System.IntPtr returnValueAddress) (at /home/bokken/build/output/unity/unity/Runtime/Export/Scripting/Coroutines.cs:17)
```

It seems that the official solution graph expects `mediapipe::GpuBuffer`, but we're putting `mediapipe::ImageFrame` to the input stream.\
So let's edit the `face_mesh_desktop_live_gpu.pbtxt` and convert the input using `ImageFrameToGpuBufferCalculator`.

Add the following node to the graph.

```txt
node: {
  calculator: "ImageFrameToGpuBufferCalculator"
  input_stream: "throttled_input_video"
  output_stream: "throttled_input_video_gpu"
}
```

We also need to convert the output from `mediapipe::GpuBuffer` to `mediapipe::ImageFrame`.

```txt
node: {
  calculator: "GpuBufferToImageFrameCalculator"
  input_stream: "output_video_gpu"
  output_stream: "output_video"
}
```

> :bell: You need to change some `Calculator` inputs and outputs as well, but this is left as an exercise.

If everything is fine, the result would be like this.

![face-mesh-gpu](https://github.com/user-attachments/assets/559f009c-2e75-443e-8724-55f2b1b073d2)
