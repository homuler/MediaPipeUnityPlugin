# Hello World! on Desktop

## Build
```sh
# Don't forget to include "//mediapipe/calculators/core:pass_through_calculator".
bazel build -c opt --define MEDIAPIPE_DISABLE_GPU=1 //mediapipe/apis:mediapipe_c
```
