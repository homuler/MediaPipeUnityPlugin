# Build Instructions

- [(Recommended) Using GitHub Actions](#recommended-using-github-actions)
- [Building Locally](#building-locally)
  - [Supported Platforms](#supported-platforms)
  - [Building on Linux](#building-on-linux)
  - [Building on macOS](#building-on-macos)
  - [Building on Windows](#building-on-windows)
  - [Building for Android](#building-for-android)
- [Build Command](#build-command)
- [Known Issues](#known-issues)
- [Troubleshooting](#troubleshooting)
- [FAQ](#faq)

This repository does not include the libraries (e.g. \*.so, \*.dll) and models (e.g. \*.tflite) required to run MediaPipeUnityPlugin.\
You can download pre-built packages that include these files from [the release page](https://github.com/homuler/MediaPipeUnityPlugin/releases).

However, you'll need to build the libraries yourself in the following cases:

- You want to reduce the library size by removing unnecessary code.
- You want to use calculators that are not included in the release package.
- You want to try out the latest features that are not yet included in the release package.
- You want to use a different version of MediaPipe, especially [a customized version](#how-to-use-a-customized-version-of-mediapipe).

This documentation describes how to build the package yourself.

# (Recommended) Using GitHub Actions

This repository includes a GitHub Actions Workflow for building libraries, so it's the easiest way to build the package.\
Fork this repository and run [the build-package workflow](https://github.com/homuler/MediaPipeUnityPlugin/actions/workflows/build-package.yml) in your forked repository.

![The build-package workflow page](https://github.com/user-attachments/assets/97760494-5c53-49e1-8fa7-34fdd3a05239)

For information on what values to specify when running the build workflow, please refer to the [Build Command](#build-command) section.

# Building Locally

While [using GitHub Actions](#recommended-using-github-actions) is recommended for building for all platforms, you can also build in your local environment, for example when doing trial & error on your PC.

> :bell: If you encounter build issues, please refer to the GitHub Actions workflow definition.
> If the error cannot be reproduced in the GitHub Actions workflow, we generally cannot provide support.

## Supported Platforms

> :warning: GPU mode is not supported on macOS and Windows.

|                            |       Editor       |   Linux (x86_64)   |     macOS [^1]     |  Windows (x86_64)  |      Android       |        iOS         |
| :------------------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: | :----------------: |
|       Linux (AMD64)        | :heavy_check_mark: | :heavy_check_mark: |                    |                    | :heavy_check_mark: |                    |
|         macOS [^1]         | :heavy_check_mark: |                    | :heavy_check_mark: |                    | :heavy_check_mark: | :heavy_check_mark: |
| Windows 10/11 (AMD64) [^2] | :heavy_check_mark: |                    |                    | :heavy_check_mark: | :heavy_check_mark: |                    |

[^1]: Universal macOS Library can be built.
[^2]: Running MediaPipe on Windows is experimental. For more information, see the [MediaPipe documentation](https://ai.google.dev/edge/mediapipe/framework/getting_started/install#installing_on_windows).

## Building on Linux

> :warning: When building a library (`libmediapipe_c.so`) for Linux, if the GLibc version on the target machine is older than the GLibc version on the machine where the library was built, the library will not work.

### (Recommended) Building with Docker

> :warning: When building a library (`libmediapipe_c.so`) for Linux, it cannot be used if the GLibc version on the target machine < 2.31.

1. Install Docker

   Ensure that you can run the `docker` command without `sudo`.

1. Build the Docker image:

   ```sh
   docker build --build-arg UID=$(id -u) -t mediapipe_unity:latest . -f docker/linux/x86_64/Dockerfile
   ```

1. Start the Docker container:

   ```sh
   # Mount the `Packages` directory to the container and start it.
   docker run \
       --mount type=bind,src=$PWD/Packages,dst=/home/mediapipe/Packages \
       --mount type=bind,src=$PWD/Assets,dst=/home/mediapipe/Assets \
       -it mediapipe_unity:latest
   ```

1. Run [the build script](#build-script) on the container:

   ```sh
   # Example of building for Desktop (GPU) and Android
   python build.py build --desktop gpu --opencv cmake --android arm64 -v
   ```

Once the command completes successfully, the necessary files will be installed on the host machine.

### Building on the Host Machine

> :warning: If Docker is available, it is strongly recommended to use [Docker](#building-with-docker) for building.

#### Prerequisites

The following commands/tools/libraries need to be installed:

- Python (version >= 3.9.0, < 3.13.0)
  - You can also use uv, etc.
- Bazelisk (latest)
- GCC/G++ (version >= 13.0.0)
- Clang (version >= 16.0.0)
- binutils (version >= 2.40)
- NuGet (latest)

#### Arch Linux

The actual commands vary by distribution, but the following is an example for Arch Linux.
If you are using a different distribution, please modify the commands accordingly.

1. Install the necessary packages:

   ```sh
   pacman -Sy unzip mesa
   ```

1. Install NumPy:

   ```sh
   pip install numpy --user
   ```

1. Run [the build script](#build-script).

## Building on macOS

1. Install [Homebrew](https://brew.sh).

1. Install Python (version >= 3.9.0, < 3.13.0) and NumPy:

   ```sh
   brew install python
   export PATH=$PATH:"$(brew --prefix)/opt/python/libexec/bin"

   # Ensure Python version is >= 3.9.0, < 3.13.0
   python --version
   # Expected output: Python 3.9.x

   pip3 install --user six numpy
   ```

1. Install [Bazelisk](https://github.com/bazelbuild/bazelisk#installation):

   ```sh
   brew install bazelisk
   ```

1. Install [NuGet](https://docs.microsoft.com/en-us/nuget/install-nuget-client-tools#nugetexe-cli):

   ```sh
   brew install nuget
   ```

1. Install Xcode (version >= 16.0) from the App Store. Then, install the Command Line Tools:

   ```sh
   sudo xcodebuild -license # If you haven't agreed to the license yet
   sudo xcode-select -s /Applications/Xcode.app
   xcode-select --install
   ```

1. Run [the build script](#build-script).

## Building on Windows

> :warning: Do **NOT** set `core.autocrlf` in `.gitconfig` (check with `git config -l`).
> To successfully build libraries, ensure the source code is checked out _as is_ without altering the line feed code.

### (Recommended) Building with Docker Windows Container

> :warning: The Hyper-V backend is required, which means that Windows 10/11 Home is not supported.

1. Install [Docker Desktop](https://docs.docker.com/desktop/windows/install/).

1. Switch to Windows Containers.

1. Build a Docker image:

   ```bat
   docker build -t mediapipe_unity:windows . -f docker/windows/x86_64/Dockerfile
   ```

1. Run a Docker container:

   ```bat
   Rem Run with the `Packages` directory mounted to the container
   Rem Specify `--cpus` and `--memory` options according to your machine.
   docker run --cpus=16 --memory=32g ^
       --mount type=bind,src=%CD%\Packages,dst=C:\mediapipe\Packages ^
       --mount type=bind,src=%CD%\Assets,dst=C:\mediapipe\Assets ^
       -it mediapipe_unity:windows
   ```

1. Run [the build script](#build-script) on the container:

Once the command completes successfully, the necessary files will be installed on your host machine.

### Building with Docker Linux Container

> :warning: This method is only for building libraries for _Android_ and cannot be used for _Windows_.

1. Install [Docker Desktop](https://docs.docker.com/desktop/windows/install/).

1. Switch to Linux containers.

1. Build a Docker image:

   ```bat
   docker build -t mediapipe_unity:linux . -f docker/linux/x86_64/Dockerfile
   ```

1. Run a Docker container:

   ```bat
   Rem Run with the `Packages` directory mounted to the container
   Rem Specify `--cpus` and `--memory` options according to your machine.
   docker run --cpus=16 --memory=16g ^
       --mount type=bind,src=%CD%\Packages,dst=/home/mediapipe/Packages ^
       --mount type=bind,src=%CD%\Assets,dst=/home/mediapipe/Assets ^
       -it mediapipe_unity:linux
   ```

1. Run [the build script](#build-script) on the container:

Once the command completes successfully, the necessary files will be installed on your host machine.

### Building on the Host Machine

> :warning: The following steps cannot be used to build libraries for _Android_.\
> To build libraries for Android, refer to [Building with Docker Linux Container](Windows.md#building-with-docker-linux-container).

> :warning: Use `cmd.exe` to run commands. Some commands may not work correctly with MSYS2.

1. Install [MSYS2](https://www.msys2.org/) and update the `%PATH%` environment variable.

   If MSYS2 is installed in `C:\msys64`, add `C:\msys64\usr\bin` to your `%PATH%` environment variable.

1. Install the necessary packages:

   ```sh
   pacman -S git patch unzip
   ```

1. Install Python (version >= 3.9.0, < 3.13.0) and allow the installer to update the `%PATH%` environment variable.

   Download the Python Windows installer from https://www.python.org/downloads/windows/ and install it.

1. Install Visual C++ Build Tools 2022 and WinSDK.

   1. Download the Build Tools from https://visualstudio.microsoft.com/visual-cpp-build-tools/ and run the Visual Studio Installer.

   1. Select `Desktop development with C++` and install it.

   ![Visual Studio Installer](https://user-images.githubusercontent.com/4690128/144782083-7320741b-3ca4-4442-95ae-40421c7725ae.png)

1. Install [Bazelisk](https://docs.bazel.build/versions/main/install-bazelisk.html) and add its location to the `%PATH%` environment variable.

   Ensure you rename the executable to `bazel.exe`.

1. (Optional) Set Bazel environment variables.

   If you have multiple Visual Studios or Win SDKs installed, set the environment variables here.\
   For more details, refer to [“Build on Windows”](https://docs.bazel.build/versions/main/windows.html#build-on-windows) in the Bazel official documentation.

   ```bat
   Rem Find the exact paths and version numbers from your local installation.

   set BAZEL_VS=C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools
   set BAZEL_VC=C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\VC
   set BAZEL_VC_FULL_VERSION=<Your local VC version>
   set BAZEL_WINSDK_FULL_VERSION=<Your local WinSDK version>
   ```

1. Install [NuGet](https://docs.microsoft.com/en-us/nuget/install-nuget-client-tools#nugetexe-cli) and add its location to the `%PATH%` environment variable.

1. Install NumPy:

   ```bat
   pip install numpy --user
   ```

1. Run [the build script](#build-script).

## Building for Android

When building for Android (Linux/macOS), follow these additional steps:

1. Install [Android Studio](https://developer.android.com/studio/install)

1. Launch Android Studio, open **SDK Manager > SDK Tools**, and install Android SDK Build Tools and Android NDK

1. Set environment variables

   ```sh
   # Set ANDROID_HOME
   # Set the path to the directory containing `platforms` and `platform-tools`
   export ANDROID_HOME=/path/to/SDK

   # Set ANDROID_NDK_HOME
   # This is typically something like `$ANDROID_HOME/ndk/28.2.13676358`
   export ANDROID_NDK_HOME=/path/to/NDK
   ```

1. When running the build command, you may want to specify the `--android_ndk_api_level` option.

   ```sh
   python build.py build --android arm64 --android_ndk_api_level 21 -vv
   ```

# Build Script

`build.py` supports the following commands.

|   Command   |                        Description                        |
| :---------: | :-------------------------------------------------------: |
|   `build`   | Build and install required files (libraries, model files) |
|   `clean`   |       Clean cache directories (`build`, `bazel-*`)        |
| `uninstall` |                  Remove installed files                   |

## Build Command

Run `python build.py build --help` for more details.

```sh
# Build for Desktop with GPU enabled.
python build.py build --desktop gpu --opencv cmake -vv

# Build for Desktop with GPU disabled.
python build.py build --desktop cpu --opencv cmake -vv

# Build a Universal macOS Library (macOS only).
python build.py build --desktop cpu --opencv cmake --macos_universal -vv

# Build for Desktop, Android, and iOS
python build.py build --desktop cpu --android arm64 --ios arm64 --opencv cmake -vv

# Specify Android NDK level
python build.py build --android arm64 --android_ndk_api_level 21 -vv

# Specify required solutions
python build.py build --desktop gpu --opencv cmake --solutions face_mesh hands pose -vv
```

You can also specify the compilation mode and linker options.

```sh
# Build with debug symbols.
python build.py build -c dbg --android arm64 -vv

# Omit all symbol information.
# This can significantly reduce the library size.
python build.py build --android arm64 --linkopt=-s -vv
```

# Known Issues

1. (iOS) In some situations, the iOS Framework won't be updated.

   Due to [an issue with bazel](https://github.com/bazelbuild/bazel/issues/12389), we cannot determine the output path beforehand.\
   If you have built this plugin across versions, you may encounter this problem.\
   In that case, run `python build.py clean` and try your build command again.

# Troubleshooting

## DllNotFoundException

This error can occur for a variety of reasons, so it is necessary to isolate the cause.

1.  Native libraries are not built yet

If native libraries (`libmediapipe_c.{so,dylib,dll} / mediapipe_android.aar / MediaPipeUnity.Framework`) don't exist under `Packages/com.github.homuler.mediapipe/Runtime/Plugins`, run the [build command](#build-command) first, and make sure that the command finishes successfully.

1.  Native libraries are incompatible with your device

   -  Libraries built on Linux machines (`libmediapipe_c.so`) won't work on your Windows machine.
   -  If it occurs on your Android device, maybe

      - the architecture is wrong (`--android armv7`) or
      - the API Level is not compatible (specify the `--android_ndk_api_level` option when running the [build command](#build-command)) or
      - `libc++_shared.so` is not bundled with your apk(cf. [README](../README.md#for-android))

1.  Dependent libraries are not linked

This error occurs when some symbols in the libraries are undefined and cannot be resolved at runtime, which is very typical when OpenCV is not configured properly.\
 If the cause lies in OpenCV, you can also build and link OpenCV statically with the `--opencv cmake` option instead.

**Tips**\
 In this case, when you check [Load on startup](https://docs.unity3d.com/Manual/PluginInspector.html) and click the `Apply` button, error logs like the following will be output.

```txt
Plugins: Couldn't open Packages/com.github.homuler.mediapipe/Runtime/Plugins/libmediapipe_c.so, error:  Packages/com.github.homuler.mediapipe/Runtime/Plugins/libmediapipe_c.so: undefined symbol: _ZN2cv8fastFreeEPv
```

1.  Dependent libraries do not exist or are not loaded

    When you build an app and copy it to another machine, you need to bundle dependent libraries with it.

    For example, if you linked OpenCV dynamically, OpenCV must be installed on the target machine too.\
    If you are unsure of the cause, try checking the dependent libraries using a command such as `ldd`.

1.  When you cannot identify the cause...

    If the error occurs on UnityEditor, try loading `{lib}mediapipe_c.{so,dylib,dll}` on startup.

    ![load-on-startup](https://user-images.githubusercontent.com/4690128/135591282-8a2011b1-9ae8-4a6a-a5fb-cc3a21f1125f.png)

    `DllNotFoundException` should be thrown right after that, and if you're lucky, the error message will be more verbose now.

    Otherwise, try loading the library (on your target device) by yourself using tools or code like below.

    <details>
    <summary>Android</summary>

    ```cs
    public void LoadLibrary()
    {
       using (var system = new AndroidJavaClass("java.lang.System"))
       {
          system.CallStatic("loadLibrary", "mediapipe_jni");
       }
    }
    ```

    </details>

    <details>
    <summary>Windows</summary>
        
    ```cs
    public void LoadLibrary()
    {
    #if UNITY_EDITOR_WIN
       var path = Path.Combine("Packages", "com.github.homuler.mediapipe", "Runtime", "Plugins", "libmediapipe_c.dll");
    #elif UNITY_STANDALONE_WIN
       var path = Path.Combine(Application.dataPath, "Plugins", "libmediapipe_c.dll");
    #endif

    var handle = LoadLibraryW(path);

    if (handle != IntPtr.Zero)
    {
    // Success
    if (!FreeLibrary(handle))
    {
    Debug.LogError($"Failed to unload {path}: {Marshal.GetLastWin32Error()}");
         }
      }
      else
      {
         // Error
         // cf. https://docs.microsoft.com/en-us/windows/win32/debug/system-error-codes--0-499-
         var errorCode = Marshal.GetLastWin32Error();
         Debug.LogError($"Failed to load {path}: {errorCode}");

          if (errorCode == 126)
          {
             Debug.LogError("Check missing dependencies using [Dependencies](https://github.com/lucasg/Dependencies). If you're sure that required libraries exist, open the plugin inspector for those libraries and check `Load on startup`.");
          }

    }
    }

    [DllImport("kernel32", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr LoadLibraryW(string path);

    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FreeLibrary(IntPtr handle);

    ````
    </details>

    <details>
    <summary>Linux/macOS</summary>

    ```cs
    public void LoadLibrary()
    {
    #if UNITY_EDITOR_LINUX
       var path = Path.Combine("Packages", "com.github.homuler.mediapipe", "Runtime", "Plugins", "libmediapipe_c.so");
    #elif UNITY_STANDALONE_LINUX
       var path = Path.Combine(Application.dataPath, "Plugins", "libmediapipe_c.so");
    #elif UNITY_EDITOR_OSX
       var path = Path.Combine("Packages", "com.github.homuler.mediapipe", "Runtime", "Plugins", "libmediapipe_c.dylib");
    #elif UNITY_STANDALONE_OSX
       var path = Path.Combine(Application.dataPath, "Plugins", "libmediapipe_c.dylib");
    #endif

       var handle = dlopen(path, 2);

       if (handle != IntPtr.Zero)
       {
          // Success
          var result = dlclose(handle);

          if (result != 0)
          {
             Debug.LogError($"Failed to unload {path}");
          }
       }
       else
       {
          Debug.LogError($"Failed to load {path}: {Marshal.GetLastWin32Error()}");
          var error = Marshal.PtrToStringAnsi(dlerror());
          // TODO: release memory

          if (error != null)
          {
             Debug.LogError(error);
          }
       }
    }

    [DllImport("dl", SetLastError = true, ExactSpelling = true)]
    private static extern IntPtr dlopen(string name, int flags);

    [DllImport("dl", ExactSpelling = true)]
    private static extern IntPtr dlerror();

    [DllImport("dl", ExactSpelling = true)]
    private static extern int dlclose(IntPtr handle);
    ````

    </details>

## /bin/bash: line 1: $'\r': command not found

This error likely occurs because `core.autocrlf=true` is set in your `.gitconfig` (check with `git config -l`).\
Ensure you check out the source code without altering the line feed code.

For more information, visit: https://git-scm.com/book/en/v2/Customizing-Git-Git-Configuration#_formatting_and_whitespace.

## ERROR: PKGBUILD contains CRLF characters and cannot be sourced.

Refer to the previous section for a solution.

## ERROR [internal] load metadata for mcr.microsoft.com/windows/servercore:ltsc2019

This error occurs when building a Windows Docker image using a Linux daemon.\
Switch to Windows containers first, or verify that you are not mistakenly trying to create a Windows image when you intended to create a Linux image.

For more details, see: https://docs.docker.com/desktop/windows/#switch-between-windows-and-linux-containers.

## no matching toolchains found for types @bazel_tools//tools/android:sdk_toolchain_type

To build libraries for Android, ensure that `ANDROID_HOME` and `ANDROID_NDK_HOME` are set.\
Refer to the [Building for Android](#building-for-android) section for more information.

## java.io.IOException: ERROR: src/main/native/windows/process.cc(202): CreateProcessW("_command_" ...

The _command_ is often `git`.\
This error occurs when building libraries on Windows and the path to the _command_ is not resolved.\
Ensure the path to the desired command is included in your `%PATH%`.

## java.io.IOException: Error downloading [...]: Checksum was xxx but wanted yyy

If you have modified some URLs in the WORKSPACE file, you may have forgotten to update the corresponding `sha256sum` value.\
Alternatively, the contents may have been altered.

## cc_toolchain_suite '@local_config_cc//:toolchain' does not contain a toolchain for cpu 'ios_arm64'

This error indicates that Bazel could not find Xcode on your machine.\
If you have installed Xcode, verify its path:

```sh
xcode-select --path
```

If the output is like `/Library/Developer/CommandLineTools`, then run the following command and try again:

```sh
# Change the path to suit your environment
sudo xcode-select --switch /Applications/Xcode.app/Contents/Developer
python3 build.py clean
```

## Undefined symbols for architecture arm64: "\_glReadPixels"

Add `OpenGLES` to the Framework Dependencies.\
For more information, visit: https://github.com/homuler/MediaPipeUnityPlugin/issues/385#issuecomment-997788547.

## AttributeError: module 'argparse' has no attribute 'BooleanOptionalAction'

Install Python >= 3.9.0.

## SyntaxError: invalid syntax

Install Python >= 3.9.0.

# FAQ

## How to use a customized version of MediaPipe?
See https://github.com/homuler/MediaPipeUnityPlugin/issues/1122.

## How do I integrate a UaaL using this plugin into an application?
See https://github.com/homuler/MediaPipeUnityPlugin/issues/1258.
