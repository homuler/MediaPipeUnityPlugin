# MediaPipe Unity Plugin

This is a Unity (2020.3.23f1) [Native Plugin](https://docs.unity3d.com/Manual/NativePlugins.html) to use [MediaPipe](https://github.com/google/mediapipe) (0.8.8).

The goal of this project is to port the MediaPipe API (C++) _one by one_ to C# so that it can be called from Unity.\
This approach may sacrifice performance when you need to call multiple APIs in a loop, but it gives you the flexibility to use MediaPipe instead.

With this plugin, you can

- Write MediaPipe code in C#.
- Run MediaPipe's official solution on Unity.
- Run your custom `Calculator` and `CalculatorGraph` on Unity.
  - :warning: Depending on the type of input/output, you may need to write C++ code.

## :art: Example Solutions

Here is a list of [solutions](https://google.github.io/mediapipe/solutions/solutions.html) that you can try in the sample app.

> :bell: The graphs you can run are not limited to the ones in this list.

|                         |      Android       |        iOS         |    Linux (GPU)     |    Linux (CPU)     |    macOS (CPU)     |   Windows (CPU)    | WebGL |
| :---------------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | ----- |
|     Face Detection      | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|        Face Mesh        | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|          Iris           | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|          Hands          | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|          Pose           | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|        Holistic         | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|   Selfie Segmentation   |                    |                    |                    |                    |                    |                    |       |
|    Hair Segmentation    | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|    Object Detection     | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|      Box Tracking       | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
| Instant Motion Tracking | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|        Objectron        | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: | :heavy_check_mark: |       |
|          KNIFT          |                    |                    |                    |                    |                    |                    |       |

## :compass: Installation

This repository does not contain required libraries (e.g. `libmediapipe_c.so`, `Google.Protobuf.dll`, etc), so you need to build them first.

> :warning: libraries that can be built differ depending on your environment.

### Supported Platforms

> :warning: GPU mode is not supported on macOS and Windows.

|                             |       Editor       |   Linux (x86_64)   |   macOS (x86_64)   |   macOS (ARM64)    |  Windows (x86_64)  |      Android       |        iOS         | WebGL |
| :-------------------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :---: |
|     Linux (AMD64) [^1]      | :heavy_check_mark: | :heavy_check_mark: |                    |                    |                    | :heavy_check_mark: |                    |       |
|          Intel Mac          | :heavy_check_mark: |                    | :heavy_check_mark: |                    |                    | :heavy_check_mark: | :heavy_check_mark: |       |
|         M1 Mac [^2]         | :heavy_check_mark: |                    |                    | :heavy_check_mark: |                    | :heavy_check_mark: | :heavy_check_mark: |       |
| Windows 10 (AMD64) [^3][^4] | :heavy_check_mark: | :heavy_check_mark: |                    |                    | :heavy_check_mark: | :heavy_check_mark: |                    |       |

[^1]: Tested on Arch Linux.
[^2]: Experimental, because MediaPipe does not support M1 Mac.
[^3]: Windows 11 will be also OK.
[^4]: Running MediaPipe on Windows is [experimental](https://google.github.io/mediapipe/getting_started/install.html#installing-on-windows).

### Prerequisites

If Docker is not available, below commands/tools/libraries are required.

- Python >= 3.9.0, < 3.10.0
- Bazel >= 3.7.2 (tested against 4.2.1)
- GCC/G++ >= 8.0.0 (Linux, macOS)
- NuGet (tested against 5.10.0.7240)

Please go to the article for each OS for more details.

> :bell: Run commands at the project root if not specified otherwise.

<details>
<summary>Linux</summary>

### Linux

> :warning: If GNU libc version in the target machine is less than the version of it in the machine where `libmediapipe_c.so` (a native library for Linux) is built, `libmediapipe_c.so` won't work.\
> For the same reason, if your target machine's GNU libc version is less than 2.27, you cannot use Docker[^5].

[^5]: You can still use Docker, but you need to write `Dockerfile` by yourself.

- [Docker](#docker)
- [Arch Linux](#arch-linux)

#### Docker

1. Install Docker

   Make sure you can run `docker` command without using `sudo`.

1. Build a Docker image

   There are two `Dockerfile`s, based on Arch Linux and Ubuntu 18.04 images respectively, so use whichever you prefer.\
   Each of them uses a different version of GLIBC.

   - Arch Linux image

     ```sh
     docker build -t mediapipe_unity:latest . -f docker/linux/x86_64/Dockerfile

     # You can specify MIRROR_COUNTRY to increase download speed
     docker build --build-arg RANKMIRROS=true --build-arg MIRROR_COUNTRY=FR,GB -t mediapipe_unity:latest . -f docker/linux/x86_64/Dockerfile
     ```

   - Ubuntu 18.04 image

     ```
     docker build -t mediapipe_unity:latest . -f docker/linux/x86_64/Dockerfile.ubuntu
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
   # Note that you need to specify `--opencv cmake`, because OpenCV is not installed to the container.
   python build.py build --desktop cpu --opencv cmake -v

   # Build native libraries for Desktop GPU and Android
   python build.py build --desktop gpu --android arm64 --opencv cmake -v
   ```

If the command finishes successfully, required files will be installed to your host machine.

#### Arch Linux

If you are using another disribution, please replace some of the commands.

> :waning: If your GNU libc version in the target machine is compatible with it in the container, always prefer [Docker](#docker).

1. Install [yay](https://github.com/Jguer/yay)

   ```sh
   # Run under your favorite directory

   pacman -S --needed git base-devel
   git clone https://aur.archlinux.org/yay.git
   cd yay
   makepkg -si
   ```

1. Install required packages

   ```sh
   yay -Sy unzip mesa npm
   ```

   It is recommended to configure `npm` here (cf. https://docs.npmjs.com/cli/v8/configuring-npm/npmrc#files).

   ```txt
   # ~/.npmrc
   prefix = ${HOME}/.npm-packages
   ```

   ```txt
   # ~/.bash_profile
   export PATH=${HOME}/.npm-packages/bin:${PATH}
   ```

1. (Optional) If you'd like to link OpenCV dynamically, install OpenCV.

   Skip this step if you want to link OpenCV statically.

   ```sh
   yay -S opencv3-opt
   ```

   By default, it is assumed that OpenCV 3 is installed under `/usr` (e.g. `/usr/lib/libopencv_core.so`).\
   `opencv3-opt` will install OpenCV 3 to `/opt/opencv3`, so you need to edit [WORKSPACE](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/WORKSPACE).

   ```starlark
   # WORKSPACE
   new_local_repository(
       name = "linux_opencv",
       build_file = "@//third_party:opencv_linux.BUILD",
       path = "/opt/opencv3",
   )
   ```

   :bell: If you'd like to use OpenCV 4, you need to edit [third_party/opencv_linux.BUILD](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/third_party/opencv_linux.BUILD), too.

1. Install [Bazelisk](https://github.com/bazelbuild/bazelisk#installation) and [NuGet](https://docs.microsoft.com/en-us/nuget/install-nuget-client-tools#nugetexe-cli)

   ```sh
   npm install -g bazelisk
   yay -S nuget
   ```

1. Install numpy

   ```sh
   pip install numpy --user
   ```

1. (For Android) [Install Android SDK and Android NDK](#android-configuration)

1. Run [build command](#build-command)

</details>

<details>
<summary>macOS</summary>

### macOS

- [Intel Mac](#intel-mac)
- [M1 Mac (Experimental)](#m1-mac)

#### Intel Mac

1. Install [Homebrew](https://brew.sh)

1. Install OpenCV 3.

   > :bell: It's essentially an optional step, but if you'd like to build libraries for iOS, it's necessary because of a bug.

   ```sh
   brew install opencv@3
   brew uninstall --ignore-dependencies glog
   ```

1. Install Python and numpy

   If your Python version is **not 3.9.x**, install it here in your favorite way.

   ```sh
   brew install python

   # Python version must be >= 3.9.0 and < 3.10.0
   sudo ln -s -f /usr/local/bin/python3.9 /usr/local/bin/python
   pip3 install --user six numpy
   ```

1. Install [Bazelisk](https://github.com/bazelbuild/bazelisk#installation) and [NuGet](https://docs.microsoft.com/en-us/nuget/install-nuget-client-tools#nugetexe-cli)

   ```sh
   brew install bazelisk
   brew install nuget
   ```

1. (For iOS) Install Xcode using App Store

   After that, install the Command Line Tools, too.

   ```sh
   xcode-select --install
   ```

1. (For iOS) Open [`mediapipe_api/objc/BUILD`](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/mediapipe_api/objc/BUILD#L29) and modify `bundle_id`.

1. (For Android) [Install Android SDK and Android NDK](#android-configuration)

1. Run [build command](#build-command)

#### M1 Mac

> :warning: use UnityEditor (Apple silicon) (>= 2021.2.2f1) to open the project.

1. Install [Homebrew](https://brew.sh)

1. Install OpenCV 3.

   > :bell: It's essentially an optional step, but if you'd like to build libraries for iOS, it's necessary because of a bug.

   ```sh
   brew install opencv@3
   brew uninstall --ignore-dependencies glog
   ```

1. Install Python and numpy

   If your Python version is **not 3.9.x**, install it here in your favorite way.

   ```sh
   brew install python

   python3.9 --version
   # Python 3.9.x
   pip3 install --user six numpy
   ```

1. Install [Bazelisk](https://github.com/bazelbuild/bazelisk#installation)

   ```sh
   brew install bazelisk
   ```

1. Install [NuGet](https://docs.microsoft.com/en-us/nuget/install-nuget-client-tools#nugetexe-cli)

   ```sh
   /usr/sbin/softwareupdate --install-rosetta

   # Install Homebrew using Rosetta 2
   # Before running the command, please check https://brew.sh/ for the correct URL
   arch -x86_64 /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

   # Install NuGet using Rosetta 2
   arch -x86_64 /usr/local/homebrew/bin/brew install nuget
   ```

1. (For iOS) Install Xcode using App Store

   After that, install the Command Line Tools, too.

   ```sh
   xcode-select --install
   ```

1. (For iOS) Open [`mediapipe_api/objc/BUILD`](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/mediapipe_api/objc/BUILD#L29) and modify `bundle_id`.

1. (For Android) [Install Android SDK and Android NDK](#android-configuration)

1. Run [build command](#build-command)

</details>

<details>
<summary>Windows</summary>

### Windows

- [Docker Windows Container](#docker-windows-container)
- [Docker Linux Container](#docker-linux-container)
- [Windows 10](#windows-10)

#### Docker Windows Container

> :warning: Hyper-V backend is required (that is, Windows 10 Home is not supported).

1. Install [Docker Desktop](https://docs.docker.com/desktop/windows/install/)

1. Switch to Windows Containers

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
   docker run --cpus=8 --memory=12g ^
       --mount type=bind,src=%CD%\Packages,dst=C:\mediapipe\Packages ^
       --mount type=bind,src=%CD%\Assets,dst=C:\mediapipe\Assets ^
       -it mediapipe_unity:windows
   ```

1. (For Android, Experimental) Apply a patch to bazel.

   > :bell: You can also make use of [Linux containers](#docker-linux-container) to build libraries for Android.

   ```bat
   Rem Run inside the container

   git clone https://github.com/bazelbuild/bazel.git C:\bazel
   cd C:\bazel
   git checkout 4.2.1
   git apply ..\mediapipe\third_party\bazel_android_fixes.diff
   bazel --output_user_root=C:\_bzl build //src:bazel.exe
   cp bazel-bin\src\bazel.exe C:\bin
   cd C:\mediapipe
   ```

1. Run [build command](#build-command) inside the container

   ```bat
   python build.py build --desktop cpu --opencv cmake -vv
   Rem or if you'd like to link OpenCV dynamically
   python build.py build --desktop cpu --include_opencv_libs -vv
   ```

If the command finishes successfully, required files will be installed to your host machine.

#### Docker Linux Container

> :warning: This can be used only to build libraries for _Android_ and you cannot build libraries for _Windows_.

1. Install [Docker Desktop](https://docs.docker.com/desktop/windows/install/)

1. Switch to Linux containers

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
   docker run --cpus=16 --memory=8g ^
       --mount type=bind,src=%CD%\Packages,dst=/home/mediapipe/Packages ^
       --mount type=bind,src=%CD%\Assets,dst=/home/mediapipe/Assets ^
       -it mediapipe_unity:linux
   ```

1. Run [build command](#build-command) inside the container

   ```sh
   python build.py build --android arm64 -vv
   ```

If the command finishes successfully, required files will be installed to your host machine.

#### Windows 10

> :warning: You cannot build libraries for _Android_ with the following steps.
> If you use Window 10 Pro, go to [Docker Windows Container](#docker-windows-container).

> :warning: Run commands using 'cmd.exe'. It's known that some commands does not work properly with MSYS2.

1. Install [MSYS2](https://www.msys2.org/) and edit the `%PATH%` environment variable.

   If MSYS2 is installed to `C:\msys64`, add `C:\msys64\usr\bin` to your `%PATH%` environment variable.

1. Install necessary packages

   ```sh
   pacman -S git patch unzip
   ```

1. Install Python 3.9.x and allow the executable to edit the `%PATH%` environment variable.

   Download Python Windows executable from https://www.python.org/downloads/windows/ and install.

1. Install Visual C++ Build Tools 2019 and WinSDK

   1. Download Build Tools from https://visualstudio.microsoft.com/visual-cpp-build-tools/ and run Visual Studio Installer.

   1. Select `Desktop development with C++` and install it.

   ![Visual Studio Installer](https://user-images.githubusercontent.com/4690128/144782083-7320741b-3ca4-4442-95ae-40421c7725ae.png)

1. Install [Bazel](https://docs.bazel.build/versions/main/install-windows.html) or [Bazelisk](https://docs.bazel.build/versions/main/install-bazelisk.html) and add the location of the Bazel executable to the `%PATH%` environment variable.

   If you use Bazelisk, save the binary as `bazel.exe`.

1. (Optional) Set Bazel variables.

   If you have installed multiple Visual Studios or Win SDKs, set environment variables here.\
   Learn more details about [“Build on Windows”](https://docs.bazel.build/versions/main/windows.html#build-on-windows) in the Bazel official documentation.

   ```bat
   Rem Please find the exact paths and version numbers from your local version.

   set BAZEL_VS=C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools
   set BAZEL_VC=C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\VC
   set BAZEL_VC_FULL_VERSION=<Your local VC version>
   set BAZEL_WINSDK_FULL_VERSION=<Your local WinSDK version>
   ```

1. (Optional) If you'd like to link OpenCV dynamically, install OpenCV.

   You can skip this step if you want to link OpenCV statically.

   By default, it is assumed that OpenCV 3.4.16 is installed under `C:\opencv`.\
   If your version or path is different, please edit [third_party/opencv_windows.BUILD](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/third_party/opencv_windows.BUILD) and [WORKSPACE](https://github.com/homuler/MediaPipeUnityPlugin/blob/master/WORKSPACE).

1. Install [NuGet](https://docs.microsoft.com/en-us/nuget/install-nuget-client-tools#nugetexe-cli), and add the location of the NuGet executable to the `%PATH%` environment variable.

   ```bat
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

1. Run [build command](#build-command)

   ```bat
   python build.py build --desktop cpu --opencv=cmake -v
   Rem or if you'd like to use local OpenCV
   python build.py build --desktop cpu --include_opencv_libs -v
   ```

</details>

### Android Configuration

1. Install [Android Studio](https://developer.android.com/studio/install)

1. Install Android SDK and NDK

   Open **SDK Manager > SDK Tools** and install Android SDK Build Tools and NDK.

   :bell: Note the following 2 points:

   - Bazel will use the newest version of Build Tools found automatically, but the latest Bazel (4.2.1) does not support Build Tools >= 31.0.0, so you need to uncheck these versions.
   - Bazel does not support NDK >= r22 yet

   ![Android Studio (SDK Tools)](https://user-images.githubusercontent.com/4690128/144735652-21339ab0-5a45-4277-b7ee-39d106b5e1e6.png)

1. Install JDK (>= 8)

   You can [install OpenJDK](https://developer.android.com/studio/intro/studio-config#jdk) with Android Studio.

1. Set environment variables

   - Linux/macOS

     ```sh
     # Set JAVA_HOME. Don't include extra directories
     #   GOOD: $HOME/.jdks/openjdk-17.0.1/
     #   BAD:  $HOME/.jdks/openjdk-17.0.1/bin
     export JAVA_HOME=/path/to/JDK

     # Set ANDROID_HOME
     # This directory should contain directories such as `platforms` and `platform-tools`.
     export ANDROID_HOME=/path/to/SDK

     # Set ANDROID_NDK_HOME
     # This is usually like `$ANDROID_HOME/ndk/21.4.7075529
     export ANDROID_NDK_HOME=/path/to/NDK
     ```

   - Windows

     Set them using GUI

1. Set API Level

   To support older Android, you need to specify API level for NDK in `WORKSPACE` file.\
   Otherwise, some symbols in `libmediapipe_jni.so` cannot be resolved and `DllNotFoundException` will be thrown at runtime.

   ```bzl
   android_ndk_repository(
     name = "androidndk",
     api_level = 21, # add this line
   )
   ```

## :hammer_and_wrench: Build

`build.py` supports the following commands.

|   Command   |                              Description                              |
| :---------: | :-------------------------------------------------------------------: |
|   `build`   | Build and install required files (libraries, model files, C# scripts) |
|   `clean`   |             Clean cache directories (`build`, `bazel-*`)              |
| `uninstall` |                         Remove install files                          |

### Build Command

Run `python build.py build --help` for more details.

```sh
# Build for Desktop with GPU enabled (Linux only).
python build.py build --desktop gpu -vv

# If you've not installed OpenCV locally, you need to build OpenCV from sources for Desktop.
python build.py build --desktop gpu --opencv cmake -vv

# Build for Desktop with GPU disabled.
# On Windows, you need to copy OpenCV shared libraries (i.e. `opencv_world3416.dll`) to `Packages` as follows.
python build.py build --desktop cpu --include_opencv_libs -vv

# Build for Desktop, Android, and iOS
python build.py build --desktop cpu --android arm64 --ios arm64 -vv
```

You can also specify compilation mode and linker options.

```sh
# Build with debug symbols.
python build.py build -c dbg --android arm64 -vv

# Omit all symbol information.
# This can significantly reduce the library size.
python build.py build --android --linkopt=-s -vv
```

## :plate_with_cutlery: Try sample app

### UnityEditor

Select `Mediapipe/Samples/Scenes/Start Scene` and play.

### Desktop

If you've built native libraries for CPU (i.e. `--desktop cpu`), select `CPU` for inference mode from the Inspector Window.
![preferable-inference-mode](https://user-images.githubusercontent.com/4690128/134795568-156f3d41-b46e-477f-a487-d04c99300c33.png)

### Android, iOS

Make sure that you select `GPU` for inference mode before building the app, because `CPU` inference mode is not supported currently.

## :question: FAQ

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

## :scroll: LICENSE

MIT

Note that some files are distributed under other licenses.

- MediaPipe ([Apache Licence 2.0](https://github.com/google/mediapipe/blob/master/LICENSE))
- FontAwesome ([LICENSE](https://github.com/FortAwesome/Font-Awesome/blob/master/LICENSE.txt))
