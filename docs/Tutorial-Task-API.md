# Tutorial: Task API

MediaPipe implements Task APIs that allow you to easily execute several predefined tasks. This plugin also implements the Task API with a similar interface.

In this article, we will use the FaceLandmarker API as an example to explain how to use the Task API. In this sample, we aim to use a `WebCamTexture` as the input image for the FaceLandmarker API and overlay the output landmarks on it.

> :skull_and_crossbones: On Windows, some of the code below might cause UnityEditor to crash. Check [Technical Limitations](../README.md#warning-technical-limitations) for more information.

> :bell: A scene for this tutorial is available under `Tutorial/Tasks`.

> :bell: When you use the Task API, it's recommended to read the official documentation(e.g. [Face landmark detection](https://ai.google.dev/edge/mediapipe/solutions/vision/face_landmarker)).

## Display Webcam Images

Let's start by displaying the camera image using [`WebCamTexture`](https://docs.unity3d.com/ScriptReference/WebCamTexture.html).
The following script is already provided in `Tutorial/Tasks/FaceLandmarkerRunner.cs`.

```cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.Tutorial
{
  public class FaceLandmarkerRunner : MonoBehaviour
  {
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

If you open the scene in an environment where a webcam is available and run it, the webcam image should be displayed as shown below.

![face-landmarker-webcam](https://github.com/user-attachments/assets/f06ac301-a045-46ed-81e7-60e069c48515)

By default, it operates at 1280x720 (30fps), but if it does not work with this setting, change it to a setting value supported by your webcam.

## Create the Task

First, we will create a Task.

> :bell: For options not introduced in this article, please refer to the documentation comments.

> :bell: For the meaning of the options, refer to the documentation comments or the [official documentation](https://ai.google.dev/edge/mediapipe/solutions/vision/face_landmarker).

Typically, you generate a Task by setting options.
```cs
using Mediapipe.Tasks.Vision.FaceLandmarker;

TextAsset modelAsset;

var options = new FaceLandmarkerOptions(
  baseOptions: new Tasks.Core.BaseOptions(
    Tasks.Core.BaseOptions.Delegate.CPU,
    modelAssetBuffer: modelAsset.bytes
  ),
  runningMode: Tasks.Vision.Core.RunningMode.VIDEO
);

using var faceLandmarker = FaceLandmarker.CreateFromOptions(options);
```

There are other options available, but at a minimum, you need to set the model and delegate in `BaseOptions`. Additionally, you should specify the `RunningMode` according to your use case (the default is `RunningMode.IMAGE`).

In this sample, we will execute in `RunningMode.VIDEO`.

Please also refer to the [Task API Options](#task-api-options) section for more details.

## Prepare Data

The FaceLandmarker API accepts an `Image` as input, so let's create an `Image` instance corresponding to the `WebCamTexture` image data.

Copy the data from `WebCamTexture` to a `Texture2D`, and then create an `Image` from it.

```cs
var tmpTexture = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);
tmpTexture.SetPixels32(webCamTexture.GetPixels32());
tmpTexture.Apply();
using var image = new Image(tmpTexture);
```

> :warning: This code reads pixel data on the CPU. To avoid data copying between CPU and GPU (especially on Android), refer to [Share OpenGL Context](#share-opengl-context).

> :warning: In general, you should avoid calling `SetPixels32` since it's so slow. See the [Flip the Input Image](#flip-the-input-image) section for an alternative.

## Run the Task

Now, let's run the task.

The execution method varies depending on the RunningMode, but in VIDEO mode, you use the `DetectForVideo` method[^1][^2].

[^1]: Which method to use depends on the RunningMode. Please refer to the [Running Mode](#running-mode) section for more details.
[^2]: The `DetectForVideo` method causes allocation each time it is executed. It is recommended to use the `TryDetectForVideo` method in production environments. For details, refer to [Avoid Allocations](#avoid-allocations).

```cs
// VIDEO mode
var result = faceLandmarker.DetectForVideo(image, timestampMillisec);
```

You need to provide the timestamp of the input frame as the second argument, `timestampMillisec`.

```cs
using Stopwatch = System.Diagnostics.Stopwatch;

var stopwatch = new Stopwatch();
stopwatch.Start();
// ...
var result = faceLandmarker.DetectForVideo(image, stopwatch.ElapsedMilliseconds);
```

Now, we can run the task and get the results.

```cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Tasks.Vision.FaceLandmarker;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.Tutorial
{
  public class FaceLandmarkerRunner : MonoBehaviour
  {
    [SerializeField] private RawImage screen;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int fps;

    [SerializeField] private TextAsset modelAsset;

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

      var options = new FaceLandmarkerOptions(
        baseOptions: new Tasks.Core.BaseOptions(
          Tasks.Core.BaseOptions.Delegate.CPU,
          modelAssetBuffer: modelAsset.bytes
        ),
        runningMode: Tasks.Vision.Core.RunningMode.VIDEO
      );

      using var faceLandmarker = FaceLandmarker.CreateFromOptions(options);

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var waitForEndOfFrame = new WaitForEndOfFrame();
      var tmpTexture = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32, false);

      while (true)
      {
        tmpTexture.SetPixels32(webCamTexture.GetPixels32());
        tmpTexture.Apply();
        using var image = new Image(tmpTexture);

        var result = faceLandmarker.DetectForVideo(image, stopwatch.ElapsedMilliseconds);
        Debug.Log(result);

        yield return waitForEndOfFrame;
      }
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

Since the `SerializedField` `modelAsset` is empty, you need to set the model.\
The file `face_landmarker_v2_with_blendshapes.bytes` is available in the plugin's `PackageResources/MediaPipe` folder, so set this file.

![face-landmarker-set-model-asset](https://github.com/user-attachments/assets/51432330-bdec-4e8d-8c32-503e5006a993)

When you run the scene, the inference results should be output to the console.

> :warning: Outputting results as strings is a heavy process and may cause lag.

![face-landmarker-empty-result](https://github.com/user-attachments/assets/0cc45e2a-10dc-443d-a7fd-a598f9920128)

But why are most of the results `null`?

## Flip the Input Image

MediaPipe and Unity handle pixel data differently. Unity treats the bottom-left corner as the origin, while MediaPipe uses the top-left corner as the origin (= Image Coordinate System).

Therefore, if you pass the data directly to MediaPipe, it will interpret the image as flipped vertically. To avoid this, you should flip the image vertically beforehand.

Although experimental, you can use a class called `TextureFrame` to do this relatively easily.

> :warning: This method is experimental and may change in the future.

```cs
using var textureFrame = new Experimental.TextureFrame(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32);

textureFrame.ReadTextureOnCPU(webCamTexture, flipHorizontally: false, flipVertically: true);
using var image = textureFrame.BuildImage();
```

Overall, it looks like this:

```cs
using var faceLandmarker = FaceLandmarker.CreateFromOptions(options);

var stopwatch = new Stopwatch();
stopwatch.Start();

var waitForEndOfFrame = new WaitForEndOfFrame();
using var textureFrame = new Experimental.TextureFrame(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32);

while (true)
{
  textureFrame.ReadTextureOnCPU(webCamTexture, flipHorizontally: false, flipVertically: true);
  using var image = textureFrame.BuildCPUImage();

  var result = faceLandmarker.DetectForVideo(image, stopwatch.ElapsedMilliseconds);
  Debug.Log(result);

  yield return waitForEndOfFrame;
}
```

Now, the landmarks are correctly detected!

![face-landmarker-results](https://github.com/user-attachments/assets/1a8b6222-12da-4cb9-83ec-898217a23e9b)

## Get Landmark Coordinates

The return value of `DetectForVideo` includes the position information of landmarks, so let's use it.

For example, let's obtain the position of the top of the head. This is located in the 11th element of the landmarks list[^3].

[^3]: Refer to the [official image](https://storage.googleapis.com/mediapipe-assets/documentation/mediapipe_face_landmark_fullsize.png) to see which element corresponds to which position.

It's important to note that this position information also uses MediaPipe's coordinate system, which differs from Unity's.

The position information in `FaceLandmarkerResult.faceLandmarks` is of type [`NormalizedLandmark`](https://ai.google.dev/edge/api/mediapipe/java/com/google/mediapipe/tasks/components/containers/NormalizedLandmark).

If you want to overlay this on the screen, it's useful to convert it to the screen's local coordinate system, and there's a helper for that.

```cs
using Mediapipe.Unity.CoordinateSystem;

var screenRect = screen.rectTransform.rect;
var topOfHead = result.faceLandmarks[0].landmarks[10];
var position = screenRect.GetPoint(in topOfHead);

var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
sphere.transform.SetParent(screen.transform);
sphere.transform.localScale = new Vector3(10f, 10f, 10f);
sphere.transform.localPosition = position;
```

The above code creates a `Sphere` object and draws it at the `topOfHead` position.

Format the code as follows and try running it.

```cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Tasks.Vision.FaceLandmarker;
using Mediapipe.Unity.CoordinateSystem;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.Tutorial
{
  public class FaceLandmarkerRunner : MonoBehaviour
  {
    [SerializeField] private RawImage screen;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int fps;

    [SerializeField] private TextAsset modelAsset;

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

      var options = new FaceLandmarkerOptions(
        baseOptions: new Tasks.Core.BaseOptions(
          Tasks.Core.BaseOptions.Delegate.CPU,
          modelAssetBuffer: modelAsset.bytes
        ),
        runningMode: Tasks.Vision.Core.RunningMode.VIDEO
      );

      using var faceLandmarker = FaceLandmarker.CreateFromOptions(options);

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      var waitForEndOfFrame = new WaitForEndOfFrame();
      using var textureFrame = new Experimental.TextureFrame(webCamTexture.width, webCamTexture.height, TextureFormat.RGBA32);
      var screenRect = screen.rectTransform.rect;

      var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      sphere.transform.SetParent(screen.transform);
      sphere.transform.localPosition = new Vector3(0, 0, 0);
      sphere.transform.localScale = new Vector3(10f, 10f, 10f);
      sphere.SetActive(false);

      while (true)
      {
        textureFrame.ReadTextureOnCPU(webCamTexture, flipHorizontally: false, flipVertically: true);
        using var image = textureFrame.BuildCPUImage();

        var result = faceLandmarker.DetectForVideo(image, stopwatch.ElapsedMilliseconds);
        if (result.faceLandmarks?.Count > 0)
        {
          var landmarks = result.faceLandmarks[0].landmarks;
          var topOfHead = landmarks[10];
          var position = screenRect.GetPoint(in topOfHead);
          position.z = 0; // ignore Z
          sphere.transform.localPosition = position;
          sphere.SetActive(true);
        }
        else
        {
          sphere.SetActive(false);
        }

        yield return waitForEndOfFrame;
      }
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

If it is drawn as shown below, it is successful!

![face-landmarker-top-of-head](https://github.com/user-attachments/assets/a3e1d1f8-e96d-4a8a-b6ee-e336da7d21ac)

## Task API Options

Let's delve deeper into the options introduced in [Create the Task](#create-the-task).

### `Delegate`

On supported platforms, inference can be performed on the GPU or TPU. In such cases, set the `Delegate` to `Delegate.GPU` or `Delegate.EDGETPU_NNAPI`.

#### Share OpenGL Context

> :bell: This is an advanced topic.

Especially when the Graphics API is set to OpenGL ES on Android, you can share the OpenGL Context between MediaPipe and Unity to avoid copying input images on the CPU. To do this, pass `GpuResources` as the second argument to `CreateFromOptions`.

`Initializing `GpuResources` can be complex, so use the `GpuManager` helper.

```cs
// ATTENTION!: It will fail if the Graphics API is set to OpenGL Core.
yield return GpuManager.Initialize();

using var gpuResources = GpuManager.IsInitialized ? GpuManager.GpuResources : null;
using var faceLandmarker = FaceLandmarker.CreateFromOptions(options, gpuResources);
```

### Running Mode

Refer to the [official documentation](https://ai.google.dev/edge/mediapipe/solutions/vision/face_landmarker) for the meaning of each mode.

While not the official view of MediaPipe, the modes are generally used as follows:

- `RunningMode.IMAGE`
  - When the input image is a still image and you want to perform inference once.
  - When processing multiple still images independently.
- `RunningMode.VIDEO`
  - When you want to process video frames continuously.
- `RunningMode.LIVE_STREAM`
  - Officially intended for camera input, but the use case is almost the same as VIDEO mode.
  - VIDEO mode receives inference results synchronously, while LIVE_STREAM mode receives results asynchronously via a callback function.
  - Useful when you want to avoid blocking the main thread during inference.

Additionally, the method used to execute the Task differs for each mode. Use `Detect` for IMAGE mode and `DetectAsync` for LIVE_STREAM mode instead of `DetectForVideo`.

### Specify the Model Path

For simplicity, this tutorial uses `ModelAssetBuffer` to pass byte arrays, but if the model file is available on the device, you can specify the file path.

```cs
var options = new FaceLandmarkerOptions(
  baseOptions: new Tasks.Core.BaseOptions(
    Tasks.Core.BaseOptions.Delegate.CPU,
    modelAssetPath: "face_landmarker_v2_with_blendshapes.bytes"
  ),
  runningMode: Tasks.Vision.Core.RunningMode.VIDEO
);
```

If using `StreamingAssets` and the model does not exist on the local file system, you need to write the model to a file in advance. A helper is provided for this.

```cs
var resourceManager = new StreamingAssetsResourceManager();

var modelPath = "face_landmarker_v2_with_blendshapes.bytes"; // Assuming the model is placed in the StreamingAssets folder.
yield return resourceManager.PrepareAssetAsync(modelPath, overwrite: true);
```

## Handle Rotated Input Images

When using a camera on mobile devices, the input image (`WebCamTexture`) may be rotated depending on the device's orientation. In this case, you can pass `ImageProcessingOptions` to methods like `Detect` to rotate the image before MediaPipe starts processing.

```cs
var imageProcessingOptions = new ImageProcessingOptions(
  rotation: 270 // Current rotation angle of the image
);

var result = faceLandmarker.Detect(image, imageProcessingOptions);
```

However, due to the different coordinate systems used by MediaPipe and Unity, simply passing `webCamTexture.videoRotationAngle` to convey the rotation won't work correctly.

For example, consider a camera capturing an image oriented as follows:

```txt
w-----x
|     |
|   ^ |
y-----z
```

If the camera is rotated 270 degrees, the `WebCamTexture` will have an image like this:

```txt
y-------w
|       |
| >     |
z-------x
```

Because MediaPipe and Unity have different coordinate systems, by specifying `ImageProcessingOptions.rotation` alone, the image will be interpreted as:

```txt
z-------x
| >     |
|       |
y-------w
```

and then as a 270-degree rotated image, resulting in:

```txt
x-----w
|     |
| ^   |
z-----y
```

In the above case, to execute the Task on the correctly oriented image, you can:

1. Vertically flip the `Image`.
2. Specify 270 in `ImageProcessingOptions.rotation`[^4].

[^4]: Assuming `webCamTexture.videoVerticallyMirrored` is `false`.

You can use `Experimental.ImageTransformationOptions` to determine the parameters for executing the Task on the correctly oriented image.

```cs
var imageTransformationOptions = Experimental.ImageTransformationOptions.Build(
  shouldFlipHorizontally: webCamDevice.isFrontFacing,
  isVerticallyFlipped: webCamTexture.videoVerticallyMirrored,
  rotation: (RotationAngle)webCamTexture.videoRotationAngle
);
var flipHorizontally = imageTransformationOptions.flipHorizontally;
var flipVertically = imageTransformationOptions.flipVertically;
var imageProcessingOptions = new Tasks.Vision.Core.ImageProcessingOptions(rotationDegrees: (int)imageTransformationOptions.rotationAngle);

textureFrame.ReadTextureOnCPU(webCamTexture, flipHorizontally, flipVertically);

var result = faceLandmarker.Detect(image, imageProcessingOptions);
```

## About LIVE_STREAM Mode

In LIVE_STREAM mode, you need to specify a callback function to receive the results as an option.

```cs
var options = new FaceLandmarkerOptions(
  baseOptions: new Tasks.Core.BaseOptions(
    Tasks.Core.BaseOptions.Delegate.GPU,
    modelAssetPath: "face_landmarker_v2_with_blendshapes.bytes"
  ),
  runningMode: Tasks.Vision.Core.RunningMode.LIVE_STREAM,
  resultCallback: (FaceLandmarkerResult result, Image image, long timestamp) =>
  {
    // NOTE: This is called from a thread other than the main thread
    Debug.Log(result);
  }
);
```

Additionally, the `DetectAsync` method returns immediately without waiting for inference to complete.

```cs
// Does not block the main thread during inference
faceLandmarker.DetectAsync(image, timestampMillisec);
```

Note that the callback function is called from a thread other than the main thread, so you cannot directly call Unity's API from it.

## Avoid Allocations

Each `Detect` / `DetectForVideo` call causes allocations because it initializes lists to hold the results.

You can avoid allocations by preparing the return value in advance and passing its reference to `TryDetect` / `TryDetectForVideo`.

```cs
var result = default(FaceLandmarkerResult);

if (faceLandmarker.TryDetectForVideo(image, timestampMillisec, imageProcessingOptions, ref result))
{
  // ...
}
```

The `DetectAsync` method internally performs similar operations to avoid allocations.
Note that the reference to `result` in the callback arguments is shared between calls.
