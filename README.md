# Mediapipe Unity Plugin

This is a sample Unity (2019.3.12f1) Plugin to use Mediapipe (only tested on Linux).

## Prerequisites
Please build the native mediapipe plugins...

```sh
git clone https://github.com/homuler/mediapipe.git
cd mediapipe
git checkout feature/c-api-for-unity

bazel build -c opt //mediapipe/apis:mediapipe_c
```

and place the generated shared object (e.g. `libmediapipe_c.so`) in Assets/Mediapipe/SDK/Plugins/.
