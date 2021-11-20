# MediaPipe Unity Plugin

This is a Unity (2020.3.23f1) Plugin to use MediaPipe (0.8.8).

## Prerequisites

To use this plugin, you need to build native libraries for the target platforms (Desktop/UnityEditor, Android, iOS).
If you'd like to build them on your machine, below commands/tools/libraries are required (not required if you use Docker).

- Python >= 3.9.0
- Bazel >= 3.7.2, (< 4.0.0 for iOS)
- GCC/G++ >= 8.0.0 (Linux, macOS), < 11.0
- [NuGet](https://docs.microsoft.com/en-us/nuget/reference/nuget-exe-cli-reference)

## Platforms

- [x] Linux Desktop (tested on ArchLinux)
- [x] Android
- [x] iOS
- [x] macOS (CPU only)
- [x] Windows 10 (CPU only, experimental)

## Example Graphs

|                         | Android | iOS | Linux (GPU) | Linux (CPU) | macOS | Windows |
| :---------------------: | :-----: | :-: | :---------: | :---------: | :---: | :-----: |
|     Face Detection      |   ✅    | ✅  |     ✅      |     ✅      |  ✅   |   ✅    |
|        Face Mesh        |   ✅    | ✅  |     ✅      |     ✅      |  ✅   |   ✅    |
|          Iris           |   ✅    | ✅  |     ✅      |     ✅      |  ✅   |   ✅    |
|          Hands          |   ✅    | ✅  |     ✅      |     ✅      |  ✅   |   ✅    |
|          Pose           |   ✅    | ✅  |     ✅      |     ✅      |  ✅   |   ✅    |
|  Holistic (with iris)   |   ✅    | ✅  |     ✅      |     ✅      |  ✅   |   ✅    |
|    Hair Segmentation    |   ✅    | ✅  |     ✅      |     ✅      |  ✅   |   ✅    |
|    Object Detection     |   ✅    | ✅  |     ✅      |     ✅      |  ✅   |   ✅    |
|      Box Tracking       |   ✅    | ✅  |     ✅      |     ✅      |  ✅   |   ✅    |
| Instant Motion Tracking |   ✅    | ✅  |     ✅      |     ✅      |  ✅   |   ✅    |
|        Objectron        |   ✅    | ✅  |     ✅      |     ✅      |  ✅   |   ✅    |
|          KNIFT          |         |     |             |             |       |

## Installation Guide

Run commands at the project root if not specified otherwise.\
Also note that you need to build native libraries for Desktop CPU or GPU to run this plugin on UnityEditor.

As long as Docker is available on your environment, Docker is always preferable.\
Docker is only used to build native libraries, not at runtime, so there is no impact on performance.

- [Docker for Linux](#docker-for-linux)
- [Linux](#linux)
- [Docker for Windows](#docker-for-windows)
- [Windows](#windows)
- [macOS](#macOS)

### Docker for Linux

**ATTENTION!**: If your GNU libc version does not match the version of it in the container, built libraries don't work on your machine probably.

1. Build a Docker image

   ```sh
   docker build -t mediapipe_unity:latest . -f docker/linux/x86_64/Dockerfile

   # Above command may fail depending on glibc version installed to your host machine.
   # cf. https://serverfault.com/questions/1052963/pacman-doesnt-work-in-docker-image
   #
   # In that case, apply a patch.
   #
   #   git apply docker/linux/x86_64/glibc.patch

   # You can specify MIRROR_COUNTRY to increase download speed
   docker build --build-arg RANKMIRROS=true --build-arg MIRROR_COUNTRY=FR,GB -t mediapipe_unity:latest . -f docker/linux/x86_64/Dockerfile
   ```

1. Run a Docker container

   ```sh
   # Run with `Packages` directory mounted to the container
   docker run \
       --mount type=bind,src=$PWD/Packages,dst=/home/mediapipe/Packages \
       --mount type=bind,src=$PWD/Assets,dst=/home/mediapipe/Assets \
       -it mediapipe_unity:latest
   ```

1. Run [build command](#build-command) inside the container

   ```sh
   # Build native libraries for Desktop CPU.
   # Note that you need to specify `--opencv=cmake`, because OpenCV is not installed to the container.
   python build.py build --desktop cpu --opencv=cmake -v

   # Build native libraries for Desktop GPU and Android
   python build.py build --desktop gpu --android arm64 --opencv=cmake -v
   ```

If the command finishes successfully, required files will be installed to your host machine.

### Linux

1. (Optional) Install OpenCV and FFmpeg\
   You can skip this if you plan to build OpenCV with MediaPipe (see [Build Command](https://github.com/homuler/MediaPipeUnityPlugin#build-command) for more details).

   By default, it is assumed that OpenCV 3 is installed under `/usr` (e.g. `/usr/lib/libopencv_core.so`).\
   If your version or path is different, please edit [third_party/opencv_linux.BUILD](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/third_party/opencv_linux.BUILD) and [WORKSPACE](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/WORKSPACE).

   For example, if you use ArchLinux and [opencv3-opt](https://aur.archlinux.org/packages/opencv3-opt/), OpenCV 3 is installed under `/opt/opencv3`.\
   In this case, your `WORKSPACE` will look like this.

   ```starlark
   new_local_repository(
       name = "linux_opencv",
       build_file = "@//third_party:opencv_linux.BUILD",
       path = "/opt/opencv3",
   )
   ```

   On the other hand, if you use Ubuntu, probably OpenCV's shared libraries is installed under `/usr/lib/x86_64-linux-gnu/`.\
   In that case, your `opencv_linux.BUILD` would be like this.

   ```starlark
   cc_library(
       name = "opencv",
       srcs = glob(
           [
               "lib/x86_64-linux-gnu/libopencv_core.so",
               "lib/x86_64-linux-gnu/libopencv_calib3d.so",
               "lib/x86_64-linux-gnu/libopencv_features2d.so",
               "lib/x86_64-linux-gnu/libopencv_highgui.so",
               "lib/x86_64-linux-gnu/libopencv_imgcodecs.so",
               "lib/x86_64-linux-gnu/libopencv_imgproc.so",
               "lib/x86_64-linux-gnu/libopencv_video.so",
               "lib/x86_64-linux-gnu/libopencv_videoio.so",
           ],
       ),
       ...
   )
   ```

1. Install Bazelisk and NuGet, and ensure you can run them

   ```sh
   bazel --version
   nuget
   ```

1. Install numpy

   ```sh
   pip install numpy --user

   # or
   # pip3 install numpy --user
   ```

1. (Optional) Install Android SDK and Android NDK, and set environment variables

   ```sh
   # bash
   export ANDROID_HOME=/path/to/SDK
   # ATTENTION!: Currently Bazel does not support NDK r22, so use NDK r21 instead.
   export ANDROID_NDK_HOME=/path/to/ndk/21.4.7075529
   ```

1. Run [build command](#build-command)

### Docker for Windows

#### Desktop/UnityEditor

1. Switch to windows containers

   Note that Hyper-V backend is required (that is, Windows 10 Home is not supported).

1. Build a Docker image

   ```bat
   docker build -t mediapipe_unity:windows . -f docker/windows/x86_64/Dockerfile
   ```

   This process will hang when MSYS2 is being installed.\
   If this issue occurs, remove `C:\ProgramData\Docker\tmp\hcs*\Files\$Recycle.Bin\` manually (hcs\* is random name).\
   cf. https://github.com/docker/for-win/issues/8910

1. Run a Docker container

   ```bat
   Rem Run with `Packages` directory mounted to the container
   Rem Specify `--cpus` and `--memory` options according to your machine.
   docker run --cpus=16 --memory=12288m ^
       --mount type=bind,src=%CD%\Packages,dst=C:\mediapipe\Packages ^
       --mount type=bind,src=%CD%\Assets,dst=C:\mediapipe\Assets ^
       -it mediapipe_unity:windows
   ```

1. Run [build command](#build-command) inside the container
   ```bat
   python build.py build --desktop cpu --opencv=cmake -v
   ```

If the command finishes successfully, required files will be installed to your host machine.

#### Android

1. Switch to Linux containers

   Note that you cannot build native libraries for Desktop with Linux containers.

1. Build a Docker image

   ```bat
   Rem You may skip applying a patch depending on your machine settings.
   Rem See https://serverfault.com/questions/1052963/pacman-doesnt-work-in-docker-image for more details.
   git apply docker/linux/x86_64/pacman.patch

   docker build -t mediapipe_unity:linux . -f docker/linux/x86_64/Dockerfile
   ```

1. Run a Docker container

   ```bat
   Rem Run with `Packages` directory mounted to the container
   Rem Specify `--cpus` and `--memory` options according to your machine.
   docker run --cpus=16 --memory=8192m ^
       --mount type=bind,src=%CD%\Packages,dst=/home/mediapipe/Packages ^
       --mount type=bind,src=%CD%\Assets,dst=/home/mediapipe/Assets ^
       -it mediapipe_unity:linux
   ```

1. Run [build command](#build-command) inside the container
   ```sh
   python build.py build --android arm64 -v
   ```

If the command finishes successfully, required files will be installed to your host machine.

### Windows

#### Desktop/UnityEditor

1. Follow [mediapipe's installation guide](https://google.github.io/mediapipe/getting_started/install.html#installing-on-windows) and
   install MSYS2, Python, Visual C++ Build Tools 2019, WinSDK and Bazel (step1 ~ step6).

1. (Optional) Install Opencv

   By default, it is assumed that OpenCV 3.4.10 is installed under `C:\opencv`.\
   If your version or path is different, please edit [third_party/opencv_windows.BUILD](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/third_party/opencv_windows.BUILD) and [WORKSPACE](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/WORKSPACE).

1. Install NuGet, and ensure you can run them

   ```sh
   bazel --version
   nuget
   ```

1. Install numpy

   ```bat
   pip install numpy --user
   ```

1. Set `PYTHON_BIN_PATH`

   ```bat
   set PYTHON_BIN_PATH=C:\path\to\python.exe
   ```

   When the path includes space characters (e.g. `C:\Program Files\Python39\python.exe`), it's reported that build command will fail.\
   In that case, install python to another directory as a workaround (it's unnecessary to set the path to `%PATH%`, but don't forget to install numpy for the new Python).

1. Run [build command](#build-command)
   ```bat
   python build.py build --desktop cpu --opencv=cmake -v
   Rem or if you'd like to use local OpenCV
   python build.py build --desktop cpu --include_opencv_libs -v
   ```

#### Android

You cannot build native libraries for Android on Windows 10, so use [Docker for Windows](https://github.com/homuler/MediaPipeUnityPlugin#android) instead.

### macOS

1. Install [Homebrew](https://brew.sh)

1. (Optional) Install OpenCV 3 and FFmpeg
   You can skip this if you plan to build OpenCV with MediaPipe (see [Build Command](https://github.com/homuler/MediaPipeUnityPlugin#build-command) for more details).
   Note that you need to install Xcode in this case.

   ```sh
   brew install opencv@3
   brew uninstall --ignore-dependencies glog
   ```

1. Install Python

   ```sh
   brew install python

   # Python version must be >= 3.9.0
   sudo ln -s -f /usr/local/bin/python3.9 /usr/local/bin/python
   pip3 install --user six numpy
   ```

1. Install Bazelisk and NuGet

   ```sh
   brew install bazelisk
   # Note that you need to specify bazel version if you'd like to build for iOS
   # See https://github.com/bazelbuild/bazelisk for more details.
   #
   #  e.g. export USE_BAZEL_VERSION=3.7.2

   brew install nuget
   ```

1. (Optional) Install Xcode

1. (Optional) Install Android SDK and Android NDK, and set environment variables

   ```sh
   # bash
   export ANDROID_HOME=/path/to/SDK
   # ATTENTION!: Currently Bazel does not support NDK r22, so use NDK r21 instead.
   export ANDROID_NDK_HOME=/path/to/ndk/21.4.7075529
   ```

1. Run [build command](#build-command)

### Build command

```sh
# Required files (native libraries, model files, C# scripts) will be built and installed.

# Build for Desktop with GPU enabled.
python build.py build --desktop gpu -v

# If you've not installed OpenCV locally, you need to build OpenCV from sources for Desktop.
python build.py build --desktop gpu --opencv=cmake -v

# Build for Desktop with GPU disabled, and copy OpenCV shared libraries to `Packages`.
python build.py build --desktop cpu --include_opencv_libs -v

# Build for Desktop, Android, and iOS
python build.py build --desktop cpu --android arm64 --ios arm64 -v
```

Run `python build.py --help` and `python build.py build --help` for more details.

## Run example scenes

### UnityEditor

Select `Mediapipe/Samples/Scenes/Start Scene` and play.

### Desktop

If you've built native libraries for CPU (i.e. `--desktop cpu`), select `CPU` for inference mode from the Inspector Window.
![preferable-inference-mode](https://user-images.githubusercontent.com/4690128/134795568-156f3d41-b46e-477f-a487-d04c99300c33.png)

### Android, iOS

Make sure that you select `GPU` for inference mode before building the app, because `CPU` inference mode is not supported currently.

## FAQ

### DllNotFoundException: mediapipe_c

This error can occur for a variety of reasons, so it is necessary to isolate the cause.

#### 1. Native libraries are not built yet

If native libraries (`libmediapipe_c.{so,dylib,dll}` / `mediapipe_android.aar` / `MediaPipeUnity.Framework`) are not built yet,
this error can occur because Unity cannot load them.

If they don't exist under `Packages/com.github.homuler.mediapipe/Runtime/Plugins`, run [build command](#build-command) first, and make sure that this command finishes successfully.

#### 2. Native libraries are incompatible with your machine

Libraries built on Linux machines won't work on your Windows machine.
If you'd like to run the plugin on Windows, you need to build `libmediapipe_c.dll` on Windows.

#### 3. Dependent libraries are not linked

This error typically also occurs when OpenCV is incorrectly configured.
See `opencv_linux.BUILD` / `opencv_windows.BUILD` and check if the path is correct (if not, edit the BUILD file).\
You can also build and link OpenCV statically with `--opencv=cmake` option instead.

Tips:\
In this case, when you check on [Load on startup](https://docs.unity3d.com/Manual/PluginInspector.html) and click `Apply` button,
error logs like the following will be output.

```txt
Plugins: Couldn't open Packages/com.github.homuler.mediapipe/Runtime/Plugins/libmediapipe_c.so, error:  Packages/com.github.homuler.mediapipe/Runtime/Plugins/libmediapipe_c.so: undefined symbol: _ZN2cv8fastFreeEPv
```

#### 4. Dependent libraries do not exist

When you build an app and copy to another machine, you need to bundle dependent libraries with it.

For example, when you build `libmediapipe_c.so` with `--opencv=local`, OpenCV is dynamically linked to `libmediapipe_c.so`.\
To use this on another machine, OpenCV must be installed to the machine too.\
In this case, the recommended way is to build `libmediapipe_c.so` with `--opencv=cmake`.

If you are unsure of the cause, try checking the dependent libraries using `ldd` command, etc.

#### 5. Dependent libraries are not loaded

On Windows, when you're using local OpenCV (i.e. `--opencv=local`), `opencv_3410world.dll` must be loaded **before** `libmediapipe_c.so`.\
Unfortunately, currently no easy way to do this is known.\
As a workaround, try loading both `opencv_3410world.dll` and `libmediapipe_c.dll` on startup.

![load-on-startup](https://user-images.githubusercontent.com/4690128/135591282-8a2011b1-9ae8-4a6a-a5fb-cc3a21f1125f.png)

`DllNotFoundException` will be thrown even after restarting UnityEditor, but you can ignore it safely if everything else is going well.\
If you still cannot run sample scenes, the cause is probably something else.

### InternalException: INTERNAL: ; eglMakeCurrent() returned error 0x3000

If you encounter an error like below and you use OpenGL Core as the Unity's graphics APIs, please try Vulkan.

```txt
InternalException: INTERNAL: ; eglMakeCurrent() returned error 0x3000_mediapipe/mediapipe/gpu/gl_context_egl.cc:261)
```

### How to debug?

When debugging, you may want to read the MediaPipe log.
If you set `true` to `Glog.Logtostderr` before calling `Glog.Initialize`, MediaPipe will output logs to standard error, so you can check them from `Editor.log` or `Player.log`.

You can set various Glog flags as well. See https://github.com/google/glog#setting-flags for available options.

```cs
void OnEnable() {
    Glog.Logtosdderr = true;
    Glog.Initialize("MediaPipeUnityPlugin");
}

void OnDisable() {
    Glog.Shutdown();
}
```

## TODO

- [ ] Prepare API Documents
- [ ] Implement cross-platform APIs to send images to MediaPipe
- [ ] use CVPixelBuffer on iOS
- [ ] KNIFT

## LICENSE

MIT

Note that some files are distributed under other licenses.

- MediaPipe ([Apache Licence 2.0](https://github.com/google/mediapipe/blob/master/LICENSE))
- FontAwesome ([LICENSE](https://github.com/FortAwesome/Font-Awesome/blob/master/LICENSE.txt))
