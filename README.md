# MediaPipe Unity Plugin
This is a Unity (2020.3.8f1) Plugin to use MediaPipe (0.8.6).

## Prerequisites
To use this plugin, you need to build native libraries for the target platforms (Desktop/UnityEditor, Android, iOS).
If you'd like to build them on your machine, below commands/tools/libraries are required (not required if you use Docker).

- Python >= 3.9.0
- Bazel >= 3.7.2, (< 4.0.0 for iOS)
- GCC/G++ >= 8.0.0 (Linux, macOS)
- [NuGet](https://docs.microsoft.com/en-us/nuget/reference/nuget-exe-cli-reference)

## Platforms
- [x] Linux Desktop (tested on ArchLinux)
- [x] Android
- [x] iOS
- [x] macOS (CPU only)
- [x] Windows 10 (CPU only, experimental)


## Example Graphs
|                       | Android | iOS | Linux (GPU) | Linux (CPU) | macOS | Windows
:---------------------: | :-----: | :-: | :---------: | :---------: | :---: | :------:
Face Detection          | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Face Mesh               | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Iris                    | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Hands                   | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Pose                    | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Holistic (with iris)    | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Hair Segmentation       | ✅       |     | ✅           |             |       |
Object Detection        | ✅       | ✅   | ✅           | ✅           | ✅     | ✅
Box Tracking            | ✅       | ✅   | ✅           | ✅           | ✅     | 🔺*1
Instant Motion Tracking | ✅       | 🔺   | ✅           |             |       |
Objectron               | ✅       | 🔺   | ✅           |             |       |
KNIFT                   |         |     |             |             |       |

*1: crashes sometimes when the graph exits.


## Installation Guide
Run commands at the project root if not specified otherwise.\
Also note that you need to build native libraries for Desktop CPU or GPU to run this plugin on UnityEditor.

- [Docker for Linux (experimental)](#docker-for-linux-experimental)
- [Linux](#linux)
- [Docker for Windows (experimental)](#docker-for-windows-experimental)
- [Windows](#windows)
- [macOS](#macOS)


### Docker for Linux (experimental)
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
    docker run --mount type=bind,src=$PWD/Packages,dst=/home/mediapipe/Packages \
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
1. Install OpenCV and FFmpeg (optional)

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

### Docker for Windows (experimental)
#### Desktop/UnityEditor
1. Switch to windows containers

    Note that Hyper-V backend is required (that is, Windows 10 Home is not supported).

1. Build a Docker image
    ```bat
    docker build -t mediapipe_unity:windows . -f docker/windows/x86_64/Dockerfile
    ```

    This process will hang when MSYS2 is being installed.\
    If this issue occurs, remove `C:\ProgramData\Docker\tmp\hcs*\Files\$Recycle.Bin\` manually (hcs* is random name).\
    cf. https://github.com/docker/for-win/issues/8910

1. Run a Docker container
    ```bat
    Rem Run with `Packages` directory mounted to the container
    Rem Specify `--cpus` and `--memory` options according to your machine.
    docker run --cpus=16 --memory=8192m ^
        --mount type=bind,src=%CD%\Packages,dst=C:\mediapipe\Packages ^
        -it mediapipe_unity:windows
    ```

1. Run [build command](#build-command) inside the container
    ```bat
    python build.py build --desktop cpu --include_opencv_libs -v
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

1. Install Opencv

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
    ```sh
    set PYTHON_BIN_PATH=C:\path\to\python.exe
    ```

    When the path includes space characters (e.g. `C:\Program Files\Python39\python.exe`), it's reported that build command will fail.\
    In that case, install python to another directory as a workaround (it's unnecessary to set the path to `%PATH%`, but don't forget to install numpy for the new Python).

1. Run [build command](#build-command)
    ```bat
    python build.py build --desktop cpu --include_opencv_libs -v
    ```


#### Android
You cannot build native libraries for Android on Windows 10, so use [Docker for Windows](https://github.com/homuler/MediaPipeUnityPlugin#android) instead.


### macOS
1. Install [Homebrew](https://brew.sh)

1. Install OpenCV 3 and FFmpeg (optional)
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

# If FFmpeg is installed, you can build OpenCV with FFmpeg.
python build.py build --desktop gpu --opencv=cmake --opencv_deps=ffmpeg -v

# Build for Desktop with GPU disabled, and copy OpenCV shared libraries to `Packages`.
# If you use Windows 10, you would run this command.
python build.py build --desktop cpu --include_opencv_libs -v

# Build for Desktop, Android, and iOS
python build.py build --desktop cpu --android arm64 --ios arm64 -v
```

Run `python build.py --help` and `python build.py build --help` for more details.


## Run example scenes
### UnityEditor
Select `Mediapipe/Samples/Scenes/DesktopDemo` and play.

### Desktop
If you'd like to run graphs on CPU, uncheck `Use GPU` from the inspector window.
![scene-director-use-gpu](https://user-images.githubusercontent.com/4690128/107133987-4f51b180-6931-11eb-8a75-4993a5c70cc1.png)

## Troubleshooting
### DllNotFoundException: mediapipe_c
OpenCV's path may not be configured properly.

If you're sure the path is correct, please check on **Load on startup** in the plugin inspector, click **Apply** button, and restart Unity Editor.\
Some helpful logs will be output in the console.

### InternalException: INTERNAL: ; eglMakeCurrent() returned error 0x3000
If you encounter an error like below and you use OpenGL Core as the Unity's graphics APIs, please try Vulkan.

```txt
InternalException: INTERNAL: ; eglMakeCurrent() returned error 0x3000_mediapipe/mediapipe/gpu/gl_context_egl.cc:261)
```

### Debug MediaPipe
If you set an environment variable `GLOG_v` before loading native libraries (e.g. `libmediapipe_c.so`),
MediaPipe will output verbose logs to log files (e.g. `Editor.log`, `Player.log`).

```cs

void OnEnable() {
    // see https://github.com/google/glog#setting-flags
    System.Environment.SetEnvironmentVariable("GLOG_v", "2");
}
```

You can also setup Glog so that it writes logs to files.

```cs
using System.IO;

void OnEnable() {
    var logDir = Path.Combine(Application.persistentDataPath, "Logs");

    if (!Directory.Exists(logDir)) {
      Directory.CreateDirectory(logDir);
    }

    Glog.Initialize("MediaPipeUnityPlugin", logDir);
}

void OnDisable() {
    Glog.Shutdown();
}
```


## TODO
- [ ] Prepare API Documents
- [ ] Implement cross-platform APIs to send images to MediaPipe
- [ ] use CVPixelBuffer on iOS
- [ ] Box Tracking (on Windows)
- [ ] Objectron
- [ ] KNIFT

## LICENSE
MIT

Note that some files are distributed under other licenses.
- MediaPipe ([Apache Licence 2.0](https://github.com/google/mediapipe/blob/master/LICENSE))
