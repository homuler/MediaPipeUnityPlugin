# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

### [0.14.3](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.14.2...v0.14.3) (2024-03-15)

### [0.14.2](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.14.1...v0.14.2) (2024-03-15)


### Features

* get ClassificationResult / List<ClassificationResult> from Packet ([#1134](https://github.com/homuler/MediaPipeUnityPlugin/issues/1134)) ([b9e0c49](https://github.com/homuler/MediaPipeUnityPlugin/commit/b9e0c49ffe6904409fa787786b0897e28adac70d))
* implement AudioClassifier  ([#1137](https://github.com/homuler/MediaPipeUnityPlugin/issues/1137)) ([3753fdf](https://github.com/homuler/MediaPipeUnityPlugin/commit/3753fdf78a528464fffd97912e0e2d2f319bb8e2))
* implement Packet.CreateColMajorMatrix ([#1133](https://github.com/homuler/MediaPipeUnityPlugin/issues/1133)) ([af74bb2](https://github.com/homuler/MediaPipeUnityPlugin/commit/af74bb2450975869aa5146c40c1b3a91074ef940))
* implement Packet<T>.At ([#1136](https://github.com/homuler/MediaPipeUnityPlugin/issues/1136)) ([d397429](https://github.com/homuler/MediaPipeUnityPlugin/commit/d39742905eb19edf626d33db4e6febea8eb7620f))


### Bug Fixes

* dispose native resources of segmentation masks properly ([#1166](https://github.com/homuler/MediaPipeUnityPlugin/issues/1166)) ([019a655](https://github.com/homuler/MediaPipeUnityPlugin/commit/019a65520881d21c5bb7f6c96e409585881c4a67))
* the sync mode fails when trying to get the ImageFrame result ([#1142](https://github.com/homuler/MediaPipeUnityPlugin/issues/1142)) ([b6961e5](https://github.com/homuler/MediaPipeUnityPlugin/commit/b6961e5590274898755cd1212508b7e4f3459e45))

### [0.14.1](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.14.0...v0.14.1) (2024-01-26)


### Bug Fixes

* all landmarks except the first one are always null ([#1125](https://github.com/homuler/MediaPipeUnityPlugin/issues/1125)) ([73554f2](https://github.com/homuler/MediaPipeUnityPlugin/commit/73554f2cacc4e737cfdce7ff807482b425ba9eb0))
* **sample:** Android build error ([#1127](https://github.com/homuler/MediaPipeUnityPlugin/issues/1127)) ([77183c6](https://github.com/homuler/MediaPipeUnityPlugin/commit/77183c6fc6147bc035ae3466045123e58fac65f7))

## [0.14.0](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.13.1...v0.14.0) (2024-01-08)


### ⚠ BREAKING CHANGES

* strongly-typed Packet, once again (#1120)
* remove deprecated APIs (#1119)

### Features

* remove deprecated APIs ([#1119](https://github.com/homuler/MediaPipeUnityPlugin/issues/1119)) ([a6e67e3](https://github.com/homuler/MediaPipeUnityPlugin/commit/a6e67e3b2832fafc93aedf2d9ab7c9100af10da0))
* strongly-typed Packet, once again ([#1120](https://github.com/homuler/MediaPipeUnityPlugin/issues/1120)) ([4f3668e](https://github.com/homuler/MediaPipeUnityPlugin/commit/4f3668e2d99211e1c35a67092772d02f9d0b4376))

### [0.13.1](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.13.0...v0.13.1) (2024-01-06)

## [0.13.0](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.12.0...v0.13.0) (2024-01-06)


### ⚠ BREAKING CHANGES

* **sample:** remove Start Scene (#1017)
* make Status internal (#959)
* remove StatusOr<T> API  (#958)
* implement TaskRunner API (#941)

### Features

* add FaceDetector sample ([#975](https://github.com/homuler/MediaPipeUnityPlugin/issues/975)) ([a3af2d8](https://github.com/homuler/MediaPipeUnityPlugin/commit/a3af2d877417057cbbcb7daa67bfe5d0019f2c21))
* add the numFaces option to FaceDetectorOptions ([#984](https://github.com/homuler/MediaPipeUnityPlugin/issues/984)) ([a1da808](https://github.com/homuler/MediaPipeUnityPlugin/commit/a1da808f0dddfa68c93e0302dca868d5dc852339))
* build Image/ImageFrame from Texture2D ([#1083](https://github.com/homuler/MediaPipeUnityPlugin/issues/1083)) ([bcce0e9](https://github.com/homuler/MediaPipeUnityPlugin/commit/bcce0e9d1e1c25c53f58972ceb633961cf9716ec))
* CalculatorGraph supports non-generic Packet ([#1107](https://github.com/homuler/MediaPipeUnityPlugin/issues/1107)) ([dfd2034](https://github.com/homuler/MediaPipeUnityPlugin/commit/dfd203447752b1e6d4bfe7cd98965b7ce4c8d4f4))
* implement BaseOptions ([#948](https://github.com/homuler/MediaPipeUnityPlugin/issues/948)) ([d1e7b51](https://github.com/homuler/MediaPipeUnityPlugin/commit/d1e7b51b9f31dabf3ce55aa17a1e6c2e8b35300a))
* implement BaseVisionTaskApi ([#956](https://github.com/homuler/MediaPipeUnityPlugin/issues/956)) ([dde2ee2](https://github.com/homuler/MediaPipeUnityPlugin/commit/dde2ee2363a0b35abdeaff29a866dfc1119f3f9f))
* implement Components.Containers.* ([#972](https://github.com/homuler/MediaPipeUnityPlugin/issues/972)) ([3eb66c8](https://github.com/homuler/MediaPipeUnityPlugin/commit/3eb66c8bf01883032f3305c2ae19ef8170c2187f))
* implement DetectionResultAnnotationController ([#983](https://github.com/homuler/MediaPipeUnityPlugin/issues/983)) ([8dbf3f1](https://github.com/homuler/MediaPipeUnityPlugin/commit/8dbf3f1dbbb0ef5b4cd9dfbed9a8d8cf2eb00e76))
* implement FaceDetector ([#974](https://github.com/homuler/MediaPipeUnityPlugin/issues/974)) ([c777113](https://github.com/homuler/MediaPipeUnityPlugin/commit/c7771138cf2361f6184220424fa751ab90f60672))
* implement FaceLandmarker ([#992](https://github.com/homuler/MediaPipeUnityPlugin/issues/992)) ([8f79eee](https://github.com/homuler/MediaPipeUnityPlugin/commit/8f79eeeff3d13adffe3083bb1ae1d4a32b5d6025))
* implement FaceLandmarker sample ([#994](https://github.com/homuler/MediaPipeUnityPlugin/issues/994)) ([4c552e3](https://github.com/homuler/MediaPipeUnityPlugin/commit/4c552e3ff0147c87e23c1750dc94fb42b5be6731))
* implement HandLandmarker ([#997](https://github.com/homuler/MediaPipeUnityPlugin/issues/997)) ([6030526](https://github.com/homuler/MediaPipeUnityPlugin/commit/6030526b675d898efb2a97a140fa83c366340f5a))
* implement HandLandmarker sample ([#998](https://github.com/homuler/MediaPipeUnityPlugin/issues/998)) ([887f74b](https://github.com/homuler/MediaPipeUnityPlugin/commit/887f74b12c979e66559aa0fa147fd8cca6829fe9))
* implement Image ([#957](https://github.com/homuler/MediaPipeUnityPlugin/issues/957)) ([889e8ca](https://github.com/homuler/MediaPipeUnityPlugin/commit/889e8cad23a403215ca4b7316e8ba785586babc5))
* implement ImageVectorPacket ([#1020](https://github.com/homuler/MediaPipeUnityPlugin/issues/1020)) ([e9f06d0](https://github.com/homuler/MediaPipeUnityPlugin/commit/e9f06d0ee492c87f2216cc3fe06a242ade6c2a5c))
* implement non-generic OutputStreamPoller ([#1106](https://github.com/homuler/MediaPipeUnityPlugin/issues/1106)) ([73dbdff](https://github.com/homuler/MediaPipeUnityPlugin/commit/73dbdff2a48146a6e8579c779d38c5ed7aea261d))
* implement non-generic Packet class (bool/bool vector) ([#1068](https://github.com/homuler/MediaPipeUnityPlugin/issues/1068)) ([7441b9c](https://github.com/homuler/MediaPipeUnityPlugin/commit/7441b9c8acd76335a9c7b24404cba8e17955dbdd))
* implement Packet.CreateDouble, Packet.GetDouble ([#1069](https://github.com/homuler/MediaPipeUnityPlugin/issues/1069)) ([36ba644](https://github.com/homuler/MediaPipeUnityPlugin/commit/36ba64452e45968f1a7d87258d2e086b41bd24d8))
* implement Packet.CreateFloat, Packet.GetFloat ([#1070](https://github.com/homuler/MediaPipeUnityPlugin/issues/1070)) ([9a042b2](https://github.com/homuler/MediaPipeUnityPlugin/commit/9a042b2e75de5f393cf4ba77bef0c400228aedac))
* implement Packet.CreateFloatArray(Vector), Packet.GetFloatArray(List) ([#1071](https://github.com/homuler/MediaPipeUnityPlugin/issues/1071)) ([5e0449d](https://github.com/homuler/MediaPipeUnityPlugin/commit/5e0449d6123104dcc5794c751cefc3126a992455))
* implement Packet.CreateGpuBuffer ([#1104](https://github.com/homuler/MediaPipeUnityPlugin/issues/1104)) ([71c63d9](https://github.com/homuler/MediaPipeUnityPlugin/commit/71c63d9af813086ef282d4af8b97d8ff35dae094))
* implement Packet.CreateImage, Packet.GetImage ([#1072](https://github.com/homuler/MediaPipeUnityPlugin/issues/1072)) ([6e14fda](https://github.com/homuler/MediaPipeUnityPlugin/commit/6e14fda3b2ba41979565f1bcd7602c82cbe93880))
* implement Packet.CreateImageFrame, Packet.GetImageFrame ([#1073](https://github.com/homuler/MediaPipeUnityPlugin/issues/1073)) ([b476a43](https://github.com/homuler/MediaPipeUnityPlugin/commit/b476a43795695e54d7c5e274f157ecb99c1dd1b9))
* implement Packet.CreateInt, Packet.GetInt ([#1074](https://github.com/homuler/MediaPipeUnityPlugin/issues/1074)) ([d495193](https://github.com/homuler/MediaPipeUnityPlugin/commit/d495193dd7081142d1a19c9f171b0d0bcccaaa1e))
* implement Packet.CreateProto, Packet.GetProto ([#1079](https://github.com/homuler/MediaPipeUnityPlugin/issues/1079)) ([df48262](https://github.com/homuler/MediaPipeUnityPlugin/commit/df48262e26e4b09237e6c1b84dbb3de7736bb081))
* implement Packet.CreateString, GetString, GetBytes ([#1102](https://github.com/homuler/MediaPipeUnityPlugin/issues/1102)) ([84181bf](https://github.com/homuler/MediaPipeUnityPlugin/commit/84181bf0eebbb625c06112909878ac582dcc4484))
* implement Packet.GetProtoList ([#1080](https://github.com/homuler/MediaPipeUnityPlugin/issues/1080)) ([0a302b2](https://github.com/homuler/MediaPipeUnityPlugin/commit/0a302b2946e1b8d086bc146d03d8d2a316c199e9))
* implement PacketsCallbackTable ([#971](https://github.com/homuler/MediaPipeUnityPlugin/issues/971)) ([eeeec33](https://github.com/homuler/MediaPipeUnityPlugin/commit/eeeec337739b4a96912d0f3a57ab3d2822e31f28))
* implement PoseLandmarker ([#1022](https://github.com/homuler/MediaPipeUnityPlugin/issues/1022)) ([01cdd56](https://github.com/homuler/MediaPipeUnityPlugin/commit/01cdd567ac5939b81114e27e8c13a92bde8275f4))
* implement PoseLandmarker sample ([#1064](https://github.com/homuler/MediaPipeUnityPlugin/issues/1064)) ([bbd79b9](https://github.com/homuler/MediaPipeUnityPlugin/commit/bbd79b9e9534cb7f07066d0c0b5935473e5e1a5d))
* implement TaskRunner API ([#941](https://github.com/homuler/MediaPipeUnityPlugin/issues/941)) ([77d0f16](https://github.com/homuler/MediaPipeUnityPlugin/commit/77d0f1628cd9fa991b8846202ade9dc531b50487))
* implement TextureFrame and TextureFramePool ([#965](https://github.com/homuler/MediaPipeUnityPlugin/issues/965)) ([c827fb1](https://github.com/homuler/MediaPipeUnityPlugin/commit/c827fb1e1bd70cc9a2ca1772d5a5b7e353452799))
* initialize RectPacket from proto ([#970](https://github.com/homuler/MediaPipeUnityPlugin/issues/970)) ([d8593f7](https://github.com/homuler/MediaPipeUnityPlugin/commit/d8593f79f1ea89d2cf41688abd3a1af21e6c78b9))
* marshal ClassificationResult ([#1090](https://github.com/homuler/MediaPipeUnityPlugin/issues/1090)) ([36875b2](https://github.com/homuler/MediaPipeUnityPlugin/commit/36875b29834b905b8b9e957ef39cd484db5b3c4a))
* marshal DetectionResult ([#1089](https://github.com/homuler/MediaPipeUnityPlugin/issues/1089)) ([ff0bd1c](https://github.com/homuler/MediaPipeUnityPlugin/commit/ff0bd1c7aac10e3d3cb89be114e3154dc4f48962))
* marshal Landmarks ([#1098](https://github.com/homuler/MediaPipeUnityPlugin/issues/1098)) ([04752fd](https://github.com/homuler/MediaPipeUnityPlugin/commit/04752fd4b13e05d038a62957de381d3c7d9bd95f))
* marshal NormalizedLandmarks ([#1088](https://github.com/homuler/MediaPipeUnityPlugin/issues/1088)) ([6e5fa2e](https://github.com/homuler/MediaPipeUnityPlugin/commit/6e5fa2e47cf14e5fd95165eb81594d22668c0ea7))
* port ClassificationResult and Landmark ([#991](https://github.com/homuler/MediaPipeUnityPlugin/issues/991)) ([7252d53](https://github.com/homuler/MediaPipeUnityPlugin/commit/7252d53c1c0b23b5ae39cb4b44f97b2f5039f327))
* read pixel data from Image ([2f37866](https://github.com/homuler/MediaPipeUnityPlugin/commit/2f3786687a4852d3a2fdd9888eb968b895c55dca))
* **sample:** drop non-blocking sync mode ([#1110](https://github.com/homuler/MediaPipeUnityPlugin/issues/1110)) ([589b335](https://github.com/homuler/MediaPipeUnityPlugin/commit/589b3352049692aef6933dd7e58705ec66b96b6b))
* **sample:** remove Start Scene ([#1017](https://github.com/homuler/MediaPipeUnityPlugin/issues/1017)) ([059b0b2](https://github.com/homuler/MediaPipeUnityPlugin/commit/059b0b2f6991f3130a9a4b6c9d48ed5f4c93714e))


### Bug Fixes

* avoid a callback being invoked after the OutputStream has been disposed ([#1112](https://github.com/homuler/MediaPipeUnityPlugin/issues/1112)) ([74c1c85](https://github.com/homuler/MediaPipeUnityPlugin/commit/74c1c8500afeca379ea6ba6a5cb9e9d1b08f1aa4))
* determine the GrahpicsFormat at runtime ([#980](https://github.com/homuler/MediaPipeUnityPlugin/issues/980)) ([514a0a9](https://github.com/homuler/MediaPipeUnityPlugin/commit/514a0a96605518ac14d52546f3a8e893827b4acb))
* dispose segmentation mask packets ([#1066](https://github.com/homuler/MediaPipeUnityPlugin/issues/1066)) ([370bc5c](https://github.com/homuler/MediaPipeUnityPlugin/commit/370bc5c765e5ee5c646855c331294ba73689341b))
* dispose TextureFrame references instantly ([#981](https://github.com/homuler/MediaPipeUnityPlugin/issues/981)) ([678596f](https://github.com/homuler/MediaPipeUnityPlugin/commit/678596fd9014d0c5c4445d7063b7d3fb5a48cfdd))
* fix the bazel version to build bazel ([#995](https://github.com/homuler/MediaPipeUnityPlugin/issues/995)) ([bf5ec2a](https://github.com/homuler/MediaPipeUnityPlugin/commit/bf5ec2a8aed221a827f2425f1bd65d7a8f63e0ef))
* flickering when running asynchronously ([#1111](https://github.com/homuler/MediaPipeUnityPlugin/issues/1111)) ([7e1b939](https://github.com/homuler/MediaPipeUnityPlugin/commit/7e1b939f77fe9abbda9815512219fa4668b149fd))
* get pixel data pointer correctly ([#1061](https://github.com/homuler/MediaPipeUnityPlugin/issues/1061)) ([ec3e4d7](https://github.com/homuler/MediaPipeUnityPlugin/commit/ec3e4d726b378dc3117dbdf40ddffc5df9193c24))
* modelAssetBuffer cannot be specified ([#1084](https://github.com/homuler/MediaPipeUnityPlugin/issues/1084)) ([3ee44bd](https://github.com/homuler/MediaPipeUnityPlugin/commit/3ee44bdb51da5fcf4619873fe1ce701309c33bac))
* pass timestamp millisec as a long value ([#1094](https://github.com/homuler/MediaPipeUnityPlugin/issues/1094)) ([af78c5d](https://github.com/homuler/MediaPipeUnityPlugin/commit/af78c5d2c03be9bf877f69d894bd6bc37ee977c1))
* proper autofit image if camera is rotated ([#1043](https://github.com/homuler/MediaPipeUnityPlugin/issues/1043)) ([2d2863e](https://github.com/homuler/MediaPipeUnityPlugin/commit/2d2863ea740a6a5ad01854ea88ab5f48be2a36b6))
* switch front/back cameras ([#1060](https://github.com/homuler/MediaPipeUnityPlugin/issues/1060)) ([6a74693](https://github.com/homuler/MediaPipeUnityPlugin/commit/6a746931eca55e97c8373c3678637d4cb7407abf))


* make Status internal ([#959](https://github.com/homuler/MediaPipeUnityPlugin/issues/959)) ([9b3d33c](https://github.com/homuler/MediaPipeUnityPlugin/commit/9b3d33c79790f76277abe282df0c93373585db87))
* remove StatusOr<T> API  ([#958](https://github.com/homuler/MediaPipeUnityPlugin/issues/958)) ([dd4c50e](https://github.com/homuler/MediaPipeUnityPlugin/commit/dd4c50eccdf297da895b00aeae0145d60173ec0b))

## [0.12.0](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.11.0...v0.12.0) (2023-06-24)


### ⚠ BREAKING CHANGES

* MediaPipe 0.10.1 (#924)
* use Ubuntu 20.04 as the base image (#921)

### Bug Fixes

* include MediaPipeUnity.framework in unitypackage ([#882](https://github.com/homuler/MediaPipeUnityPlugin/issues/882)) ([7c43b75](https://github.com/homuler/MediaPipeUnityPlugin/commit/7c43b75f693d5f3a63056848f0a66e861d908809))


### build

* MediaPipe 0.10.1 ([#924](https://github.com/homuler/MediaPipeUnityPlugin/issues/924)) ([9d5c618](https://github.com/homuler/MediaPipeUnityPlugin/commit/9d5c61805ef4dabfe06f876643acab9900cc9f46))
* use Ubuntu 20.04 as the base image ([#921](https://github.com/homuler/MediaPipeUnityPlugin/issues/921)) ([f08fd92](https://github.com/homuler/MediaPipeUnityPlugin/commit/f08fd926a962e1b04f2bd30be84f79bdab224db9))

## [0.11.0](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.10.3...v0.11.0) (2023-03-03)


### ⚠ BREAKING CHANGES

* hide StdString from developers (#834)

* hide StdString from developers ([#834](https://github.com/homuler/MediaPipeUnityPlugin/issues/834)) ([1eac8b1](https://github.com/homuler/MediaPipeUnityPlugin/commit/1eac8b197727aac44379be9a90834c92d04399d9))

### [0.10.3](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.10.1...v0.10.3) (2022-12-28)


### Features

* add FloatVectorPacket and MatrixPacket ([#767](https://github.com/homuler/MediaPipeUnityPlugin/issues/767)) ([391d7d9](https://github.com/homuler/MediaPipeUnityPlugin/commit/391d7d98b127ce41ceac85ec47f6126664f1bc4e)), closes [#656](https://github.com/homuler/MediaPipeUnityPlugin/issues/656)
* make mask color transparent ([#733](https://github.com/homuler/MediaPipeUnityPlugin/issues/733)) ([f196f46](https://github.com/homuler/MediaPipeUnityPlugin/commit/f196f4631f81afb2f897116c5adbfead3768aab9))


### Bug Fixes

* avoid drawing holistic landmarks twice ([#755](https://github.com/homuler/MediaPipeUnityPlugin/issues/755)) ([30cd149](https://github.com/homuler/MediaPipeUnityPlugin/commit/30cd149d191b3ec2c672d420557a879fe528c459))
* ignore core.autocrlf ([#807](https://github.com/homuler/MediaPipeUnityPlugin/issues/807)) ([f2bc51b](https://github.com/homuler/MediaPipeUnityPlugin/commit/f2bc51b9f728ed1cbc390cca0e061518e9251e95))
* stop running OnValidate on immutable prefabs ([#810](https://github.com/homuler/MediaPipeUnityPlugin/issues/810)) ([f545346](https://github.com/homuler/MediaPipeUnityPlugin/commit/f5453467be5f8b0c15da8816f0ebb0ae92abf260))

### [0.10.1](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.10.0...v0.10.1) (2022-07-09)


### Features

* **objectron:** set confidence parameters at runtime ([#605](https://github.com/homuler/MediaPipeUnityPlugin/issues/605)) ([af86a8e](https://github.com/homuler/MediaPipeUnityPlugin/commit/af86a8e4e7649d63dcb4af2505bc33610a96cbed))


### Bug Fixes

* docker freezes when building a Docker Windows image ([#631](https://github.com/homuler/MediaPipeUnityPlugin/issues/631)) ([727b7ff](https://github.com/homuler/MediaPipeUnityPlugin/commit/727b7ff6c7926a90c0b348adfa973c31b67a3a1c))
* OpenCV paths os macOS ([#592](https://github.com/homuler/MediaPipeUnityPlugin/issues/592)) ([358f7b9](https://github.com/homuler/MediaPipeUnityPlugin/commit/358f7b9ac6f08d4e4f7b1d1fac9d5458e2d1730f))
* some tests fail on Windows ([#644](https://github.com/homuler/MediaPipeUnityPlugin/issues/644)) ([0a90ac1](https://github.com/homuler/MediaPipeUnityPlugin/commit/0a90ac1798b0a18300ee67a97c6756edb9e98e96))

## [0.10.0](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.9.3...v0.10.0) (2022-05-20)


### ⚠ BREAKING CHANGES

* Unity 2021 (#588)
* **plugin:** Logger.minLogLevel -> Logger.MinLogLevel (#580)
* callback functions return StatusArgs instead of Status (#574)
* coordinate transform and other helper methods (#569)

### Features

* add scenes for tutorial ([#582](https://github.com/homuler/MediaPipeUnityPlugin/issues/582)) ([c9e0317](https://github.com/homuler/MediaPipeUnityPlugin/commit/c9e0317ab336b2f9515bc81935eef5c76d032fd9))
* **plugin:** add missing Status factory methods ([#573](https://github.com/homuler/MediaPipeUnityPlugin/issues/573)) ([ba48f78](https://github.com/homuler/MediaPipeUnityPlugin/commit/ba48f781aafeb23c32063f57a396210cac9af72a))
* **plugin:** implement GpuManager ([#579](https://github.com/homuler/MediaPipeUnityPlugin/issues/579)) ([1d9196a](https://github.com/homuler/MediaPipeUnityPlugin/commit/1d9196aebddd64bf8b7b0cf24ea3b746db89f435))
* **plugin:** mediapipe v0.8.10 ([#585](https://github.com/homuler/MediaPipeUnityPlugin/issues/585)) ([738f3f7](https://github.com/homuler/MediaPipeUnityPlugin/commit/738f3f7eccc391bac943e68bbc2178c97a5db124))


### Bug Fixes

* **plugin:** OutputStream#TryGetNext throws if it's not started ([#581](https://github.com/homuler/MediaPipeUnityPlugin/issues/581)) ([9e99400](https://github.com/homuler/MediaPipeUnityPlugin/commit/9e9940046499703ecbcf6df2df5762729e4efd27))
* some tests abort on Windows ([#572](https://github.com/homuler/MediaPipeUnityPlugin/issues/572)) ([bd2c390](https://github.com/homuler/MediaPipeUnityPlugin/commit/bd2c390057f657a4876ab264d3ce82a358405805))


* callback functions return StatusArgs instead of Status ([#574](https://github.com/homuler/MediaPipeUnityPlugin/issues/574)) ([d334e80](https://github.com/homuler/MediaPipeUnityPlugin/commit/d334e80abb8c1d922191e19b10f5297b358553e0))
* coordinate transform and other helper methods ([#569](https://github.com/homuler/MediaPipeUnityPlugin/issues/569)) ([527d3b1](https://github.com/homuler/MediaPipeUnityPlugin/commit/527d3b10ae593d43c74f466c5e23b2c2440c7b30))
* **plugin:** Logger.minLogLevel -> Logger.MinLogLevel ([#580](https://github.com/homuler/MediaPipeUnityPlugin/issues/580)) ([456a822](https://github.com/homuler/MediaPipeUnityPlugin/commit/456a822e5f265ce789e071e7c73cd60e913d4657))


### build

* Unity 2021 ([#588](https://github.com/homuler/MediaPipeUnityPlugin/issues/588)) ([5d392db](https://github.com/homuler/MediaPipeUnityPlugin/commit/5d392db8f8fbd5d3d0ddc7d5015a637b3412ae26))

### [0.9.3](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.9.2...v0.9.3) (2022-05-16)

### [0.9.2](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.9.1...v0.9.2) (2022-05-16)


### Bug Fixes

* Enumerator can be invalid when an item is deleted ([#565](https://github.com/homuler/MediaPipeUnityPlugin/issues/565)) ([d8e061a](https://github.com/homuler/MediaPipeUnityPlugin/commit/d8e061a1ea5668e4b0c3ec4a3ca1abb3232f1c0c))
* **plugin:** check if the target local asset is missing ([#562](https://github.com/homuler/MediaPipeUnityPlugin/issues/562)) ([e3fa454](https://github.com/homuler/MediaPipeUnityPlugin/commit/e3fa454f30bced1f9342e8a3b41951dbe9eebc03))
* **plugin:** preserve Packet constructors for Reflection ([#561](https://github.com/homuler/MediaPipeUnityPlugin/issues/561)) ([a6f27b1](https://github.com/homuler/MediaPipeUnityPlugin/commit/a6f27b127440825aa794a6744e1c0a7d2c6431ed))
* **plugin:** prevent Status returned by a packet callback from being GCed prematurely ([#563](https://github.com/homuler/MediaPipeUnityPlugin/issues/563)) ([f4301af](https://github.com/homuler/MediaPipeUnityPlugin/commit/f4301af93dcca68514311d44ad6014c5b8ea5523))
* **sample:** dispose OutputStream if only it exists ([#564](https://github.com/homuler/MediaPipeUnityPlugin/issues/564)) ([dcee215](https://github.com/homuler/MediaPipeUnityPlugin/commit/dcee215092b169a155d7780370d9b055df46d956))
* **sample:** the default video/image is not selected ([#543](https://github.com/homuler/MediaPipeUnityPlugin/issues/543)) ([77ba1a4](https://github.com/homuler/MediaPipeUnityPlugin/commit/77ba1a4d2249b8db09b3a1e975a01f6e50d21e33))

### [0.9.1](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.9.0...v0.9.1) (2022-04-19)


### Features

* **sample:** Holistic Segmentation Mask ([#521](https://github.com/homuler/MediaPipeUnityPlugin/issues/521)) ([30e968c](https://github.com/homuler/MediaPipeUnityPlugin/commit/30e968c66a168f69c33084593ee68e5b4a7e9533))
* **sample:** MIN_DETECTION_CONFIDENCE and MIN_TRACKING_CONFIDENCE ([#523](https://github.com/homuler/MediaPipeUnityPlugin/issues/523)) ([5222faa](https://github.com/homuler/MediaPipeUnityPlugin/commit/5222faa9a21e1c523614f8c082b92ec521c2005b))
* **sample:** Pose Segmentation Mask ([#520](https://github.com/homuler/MediaPipeUnityPlugin/issues/520)) ([b25430d](https://github.com/homuler/MediaPipeUnityPlugin/commit/b25430d68f4963940b6f79c5e7209e2a8f1bd6d6))
* **sample:** Selfie Segmentation ([#522](https://github.com/homuler/MediaPipeUnityPlugin/issues/522)) ([7d6090d](https://github.com/homuler/MediaPipeUnityPlugin/commit/7d6090de1ae39e040e9a463228ae73cc5a388b04))
* set Protobuf LogHandler ([#526](https://github.com/homuler/MediaPipeUnityPlugin/issues/526)) ([cc3ac5c](https://github.com/homuler/MediaPipeUnityPlugin/commit/cc3ac5ce25d1f2af3db6f8da4ba44897e31277b3))


### Bug Fixes

* constructor should throw if it fails to initialize ([#533](https://github.com/homuler/MediaPipeUnityPlugin/issues/533)) ([e58408e](https://github.com/homuler/MediaPipeUnityPlugin/commit/e58408e142e8aec45dd90351bd021e42c54204a5))

## [0.9.0](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.8.4...v0.9.0) (2022-04-07)


### ⚠ BREAKING CHANGES

* implement ImageFrame#TryReadChannel and MaskShader (#507)

### Features

* OutputStream API ([#516](https://github.com/homuler/MediaPipeUnityPlugin/issues/516)) ([2679000](https://github.com/homuler/MediaPipeUnityPlugin/commit/2679000e86942353a7e165932f8ee6fc127c1abf))


### Bug Fixes

* check Packet types at compile-time more properly ([#509](https://github.com/homuler/MediaPipeUnityPlugin/issues/509)) ([d03895d](https://github.com/homuler/MediaPipeUnityPlugin/commit/d03895d1e63c1f4e60c4e080b8cd89fbd9a3e797))
* forget to commit ([#510](https://github.com/homuler/MediaPipeUnityPlugin/issues/510)) ([81fe7f8](https://github.com/homuler/MediaPipeUnityPlugin/commit/81fe7f8f809aa4eab095e474762a04bd11e56698))
* ignore vertically_flipped in sync mode ([#517](https://github.com/homuler/MediaPipeUnityPlugin/issues/517)) ([7d080e3](https://github.com/homuler/MediaPipeUnityPlugin/commit/7d080e3b20530586b0158fdec3bd00115ca4df03))
* **plugin:** fail to initialize GlSyncPoint ([#515](https://github.com/homuler/MediaPipeUnityPlugin/issues/515)) ([11188cb](https://github.com/homuler/MediaPipeUnityPlugin/commit/11188cb699b14f443aed8aac17b983b4271703f6))


* implement ImageFrame#TryReadChannel and MaskShader ([#507](https://github.com/homuler/MediaPipeUnityPlugin/issues/507)) ([05adcb5](https://github.com/homuler/MediaPipeUnityPlugin/commit/05adcb54c487441075824dd502c2e31c2cf5c62b))

### [0.8.4](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.8.3...v0.8.4) (2022-03-29)


### Features

* **plugin:** port ValidatedGraphConfig API ([#477](https://github.com/homuler/MediaPipeUnityPlugin/issues/477)) ([bde6874](https://github.com/homuler/MediaPipeUnityPlugin/commit/bde68746974b84ad7e8723c0fa927dcddc842907))
* **sample:** implement MIN_(DETECTION|TRACKING)_CONFIDENCE ([#483](https://github.com/homuler/MediaPipeUnityPlugin/issues/483)) ([13c2a38](https://github.com/homuler/MediaPipeUnityPlugin/commit/13c2a38e129b1d0de59867ee57f65920fdee8f30))


### Bug Fixes

* install OpenCV 3.4.16 on Docker Windows Containers ([#484](https://github.com/homuler/MediaPipeUnityPlugin/issues/484)) ([4210673](https://github.com/homuler/MediaPipeUnityPlugin/commit/42106737e3d4b57efddd2e4d8e646b7b8e283fba))
* **sample:** Anchor position is flipped ([#506](https://github.com/homuler/MediaPipeUnityPlugin/issues/506)) ([98cb0c4](https://github.com/homuler/MediaPipeUnityPlugin/commit/98cb0c432d710623a53994cdf3a47e9e1db1ff5a))
* **sample:** front camera image is not rotated properly ([#504](https://github.com/homuler/MediaPipeUnityPlugin/issues/504)) ([9d47f7c](https://github.com/homuler/MediaPipeUnityPlugin/commit/9d47f7c0e7cea557c4feba3dd468901c65bd601c))
* **sample:** GetNativeTexturePtr may never return on M1 Mac ([#505](https://github.com/homuler/MediaPipeUnityPlugin/issues/505)) ([9d7e390](https://github.com/homuler/MediaPipeUnityPlugin/commit/9d7e390b6012b2f5ebed14de599d90821f4f7f62))

### [0.8.3](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.8.2...v0.8.3) (2022-02-27)


### Features

* add Non-Blocking Sync mode to the sample app ([#471](https://github.com/homuler/MediaPipeUnityPlugin/issues/471)) ([3a33e9d](https://github.com/homuler/MediaPipeUnityPlugin/commit/3a33e9ddb4c1069a0d44a20c8e459a8d8ee8d568))
* compile with emscripten ([#395](https://github.com/homuler/MediaPipeUnityPlugin/issues/395)) ([de44dac](https://github.com/homuler/MediaPipeUnityPlugin/commit/de44dac3ad5261212d95bc59f57ef1c65802de11))


### Bug Fixes

* build AAR on Docker ([#441](https://github.com/homuler/MediaPipeUnityPlugin/issues/441)) ([75a736a](https://github.com/homuler/MediaPipeUnityPlugin/commit/75a736a8753850794ed7587561ac437d72ce3785))
* configure plugin settings ([#381](https://github.com/homuler/MediaPipeUnityPlugin/issues/381)) ([ee27e24](https://github.com/homuler/MediaPipeUnityPlugin/commit/ee27e24e963c5485a36f5a63fc7215e732f4d49f))
* memory for RenderTexture leaks ([#451](https://github.com/homuler/MediaPipeUnityPlugin/issues/451)) ([6a1d120](https://github.com/homuler/MediaPipeUnityPlugin/commit/6a1d120a531495941297bb4d41bcf20027b942db))
* no need to pin callbacks ([#460](https://github.com/homuler/MediaPipeUnityPlugin/issues/460)) ([0dafc8a](https://github.com/homuler/MediaPipeUnityPlugin/commit/0dafc8ac1359f759a23bda20ba392dacfc75df2b))
* OpenCV installation path is incorrect ([#459](https://github.com/homuler/MediaPipeUnityPlugin/issues/459)) ([6e60f75](https://github.com/homuler/MediaPipeUnityPlugin/commit/6e60f75a1056ad3f220215f32341d8bb1127a689))
* **plugin:** load mediapipe_c.dll on Windows properly ([#455](https://github.com/homuler/MediaPipeUnityPlugin/issues/455)) ([e237ba7](https://github.com/homuler/MediaPipeUnityPlugin/commit/e237ba78fe0b8c024304248b1855777ed4b380af))
* **sample:** smooth_landmarks option is not working ([#454](https://github.com/homuler/MediaPipeUnityPlugin/issues/454)) ([c90861c](https://github.com/homuler/MediaPipeUnityPlugin/commit/c90861cc7ac4f42e13e2906ad056e562e80a77f7))
* SerializedProto's memory leaks ([#461](https://github.com/homuler/MediaPipeUnityPlugin/issues/461)) ([ac6316d](https://github.com/homuler/MediaPipeUnityPlugin/commit/ac6316d63fd882c562e3c5c10ffab86a58d71ccb))
* typo and missing [Test] ([#426](https://github.com/homuler/MediaPipeUnityPlugin/issues/426)) ([018bd9f](https://github.com/homuler/MediaPipeUnityPlugin/commit/018bd9ffccdaaf2365b0590ba24fbee7f46b9be8))

### [0.8.2](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.8.1...v0.8.2) (2021-12-15)


### Features

* **deps:** MediaPipe v0.8.9 ([#376](https://github.com/homuler/MediaPipeUnityPlugin/issues/376)) ([a16d74c](https://github.com/homuler/MediaPipeUnityPlugin/commit/a16d74c0a170f3bf271df7a09a7b63f46a55a5f1))

### [0.8.1](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.8.0...v0.8.1) (2021-12-02)


### Bug Fixes

* **sample:** Hand Landmarks color is wrong in Holistic scene ([b1a2720](https://github.com/homuler/MediaPipeUnityPlugin/commit/b1a2720f36ca3b44d124d8b7234ce5b0c4e33ebe))
* **sample:** image is flipped in sync mode ([a3668be](https://github.com/homuler/MediaPipeUnityPlugin/commit/a3668befb8731a22a81a6e2e2cf489b9c951f5a7))
* flip screen image when using front facing camera ([f04ba48](https://github.com/homuler/MediaPipeUnityPlugin/commit/f04ba48645c6668ff3ffa6d7cd51125b5add44e1))
* releasing active render texture ([9537ec7](https://github.com/homuler/MediaPipeUnityPlugin/commit/9537ec76b8547d0e68a41232a6a1f62c486268b4))
* **example:** model_complexity is ignored ([0d9db0b](https://github.com/homuler/MediaPipeUnityPlugin/commit/0d9db0b8f47aff82ccd4352322a8d2b5abfe509a))
* **sample:** memory leaks when using VideoSource ([cab8e26](https://github.com/homuler/MediaPipeUnityPlugin/commit/cab8e26cb627f393f6dcbc3849b1675f39f56427))
* build with bazel 4.2.1 on macOS ([4921db1](https://github.com/homuler/MediaPipeUnityPlugin/commit/4921db1d41e829a6503a22f91f88c40aa39555a2))
* synchronize the input image and the output annotation ([#359](https://github.com/homuler/MediaPipeUnityPlugin/issues/359)) ([47598ce](https://github.com/homuler/MediaPipeUnityPlugin/commit/47598ce915863ab6218d88e1ef3ce93436b8e835))
* validate bundle_id ([06423ca](https://github.com/homuler/MediaPipeUnityPlugin/commit/06423ca40c0a6d3daf8519505f62e8a7cd592719))

## [0.8.0](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.7.0...v0.8.0) (2021-11-22)


### ⚠ BREAKING CHANGES

* MediaPipe v0.8.8 (#356)
* add .editorconfig and analyzers (#308)

### Features

* **sample:** support refineFaceLandmarks option (Holistic) ([b669ac1](https://github.com/homuler/MediaPipeUnityPlugin/commit/b669ac117b05a2302973698ed7cf223f343c63d3))
* **sample:** support refineLandmarks option (Face Mesh) ([a76194f](https://github.com/homuler/MediaPipeUnityPlugin/commit/a76194f413093f45c9fb35d3c423b6af76c80d0d))
* allow the default camera resolution to be set ([#347](https://github.com/homuler/MediaPipeUnityPlugin/issues/347)) ([4826ee2](https://github.com/homuler/MediaPipeUnityPlugin/commit/4826ee2b73d951e664a9ff751ff31788fdacc21a))


### Bug Fixes

* add missing Assert() ([a7dbfd7](https://github.com/homuler/MediaPipeUnityPlugin/commit/a7dbfd7bf5ed775c9c8fa6f05ec290089eb6ae0d))
* AssetBundle can be loaded only once ([#309](https://github.com/homuler/MediaPipeUnityPlugin/issues/309)) ([702434d](https://github.com/homuler/MediaPipeUnityPlugin/commit/702434d58e78f28702e952f9f35ee6eaaa2d4afb))
* dead link ([#333](https://github.com/homuler/MediaPipeUnityPlugin/issues/333)) ([4d90ac4](https://github.com/homuler/MediaPipeUnityPlugin/commit/4d90ac44627dd0996792e56fdc0817a7b15ada4a))
* docker command ([2994e44](https://github.com/homuler/MediaPipeUnityPlugin/commit/2994e4425d347260006e15edc8d375e0d35a3b0f))
* enable OpenCL on Android ([383b373](https://github.com/homuler/MediaPipeUnityPlugin/commit/383b3730c3963518fb7ff3f93239257d9891945f))
* MediaPipeVideoGraph fails in CPU mode ([ca77c3f](https://github.com/homuler/MediaPipeUnityPlugin/commit/ca77c3f8398df44489e2fc3bf12b9d7dc7ef7b30))
* missing MarshalAs ([67155d5](https://github.com/homuler/MediaPipeUnityPlugin/commit/67155d53315a58d1b78779cde649d05eb58a56b1))
* show errors when failed to initialize GpuResources ([f059091](https://github.com/homuler/MediaPipeUnityPlugin/commit/f0590916986e75ccf178c90099be1de8a6f3a4f3))
* thread hangs after running tests ([9c3db9a](https://github.com/homuler/MediaPipeUnityPlugin/commit/9c3db9ad6872d5dfac411aec37d51ec1950e7fc1))
* unzip does not extract *.so recursively ([a9edb9e](https://github.com/homuler/MediaPipeUnityPlugin/commit/a9edb9e8f2e8c40da7bd92376cb567fa9a59c703))


* add .editorconfig and analyzers ([#308](https://github.com/homuler/MediaPipeUnityPlugin/issues/308)) ([a3b90d1](https://github.com/homuler/MediaPipeUnityPlugin/commit/a3b90d13eac636ff837d52723eaa6f0046adb459))


### build

* MediaPipe v0.8.8 ([#356](https://github.com/homuler/MediaPipeUnityPlugin/issues/356)) ([c603366](https://github.com/homuler/MediaPipeUnityPlugin/commit/c603366b527fcd10640ef86a009e471db3866e7d))

## [0.7.0](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.6.2...v0.7.0) (2021-09-27)


### Features

* New Sample App ([#296](https://github.com/homuler/MediaPipeUnityPlugin/issues/296)) ([7bb877d](https://github.com/homuler/MediaPipeUnityPlugin/commit/7bb877de4887ad4bf23c72b39649c41bb3650d54))


### Bug Fixes

* cannot pass linker options to clang ([b8ea5c1](https://github.com/homuler/MediaPipeUnityPlugin/commit/b8ea5c1fb0b5f0eeebb5de731147d2db46a76e23))
* PacketPresenceCalculator does not work with PoseTracking ([d58ab72](https://github.com/homuler/MediaPipeUnityPlugin/commit/d58ab72c8149fce183ce804aef2e218f74ff70dc))

### [0.6.2](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.6.1...v0.6.2) (2021-07-11)


### Features

* **example:** enable to get pose world landmarks ([abbade0](https://github.com/homuler/MediaPipeUnityPlugin/commit/abbade082cbed35209f5c28ca5790f6c59c5e045))
* **example:** enable to switch face detection models ([58084e8](https://github.com/homuler/MediaPipeUnityPlugin/commit/58084e8e46c67b5ad14c0fc964b84abe613be96d))

### [0.6.1](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.6.0...v0.6.1) (2021-07-04)


### Bug Fixes

* cannot install msys2 packages ([5db311e](https://github.com/homuler/MediaPipeUnityPlugin/commit/5db311e825a2a9cad174799805dcdf62760dcf03))
* holistic sample flickers ([49c275b](https://github.com/homuler/MediaPipeUnityPlugin/commit/49c275b30b35c07dcd6d9a702bd4b864cd330abd))
* rules_cc's branch name ([#211](https://github.com/homuler/MediaPipeUnityPlugin/issues/211)) ([24576ce](https://github.com/homuler/MediaPipeUnityPlugin/commit/24576ce968c05c748c1ed56f82866c30ea9190d4))
* typo (debug -> dbg) ([67fcb0a](https://github.com/homuler/MediaPipeUnityPlugin/commit/67fcb0aca0b38cf61ac9073b8fb9a1f071adbe7a))
* use gcc-10 and g++-10 ([a3332df](https://github.com/homuler/MediaPipeUnityPlugin/commit/a3332df9049a5337bcb3478e3bfa5924356d5b25))

## [0.6.0](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.5.3...v0.6.0) (2021-05-15)


### Features

* read .binarypb ([3ca8642](https://github.com/homuler/MediaPipeUnityPlugin/commit/3ca8642ab57b7f067276da1c67ff8112b9f89c10))
* **sdk:** implement FaceGeometryPacket ([473c3a0](https://github.com/homuler/MediaPipeUnityPlugin/commit/473c3a0fa1de9f2ea0ab982936fa947d102f7ab4))


### Bug Fixes

* return type is wrong ([4ca0f5f](https://github.com/homuler/MediaPipeUnityPlugin/commit/4ca0f5f289b06e610c88cee744d1da50fb270356))

### [0.5.3](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.5.2...v0.5.3) (2021-04-30)


### Bug Fixes

* **docker:** zip command is not installed ([aa8e1bd](https://github.com/homuler/MediaPipeUnityPlugin/commit/aa8e1bd1fc97046c6fbb0ed21646d0e1eb3811d8))

### [0.5.2](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.5.1...v0.5.2) (2021-04-30)


### Bug Fixes

* copytree fails on Windows (Docker) ([fd0e83c](https://github.com/homuler/MediaPipeUnityPlugin/commit/fd0e83cdf561b22e09162363a5310bbbd0d04cd6))
* fail to build mediapipe_android.aar ([24fe5db](https://github.com/homuler/MediaPipeUnityPlugin/commit/24fe5dbfed6e5a9bc40821898542d3c6906ea11f))
* modify bsdtar command typo "s" ([#120](https://github.com/homuler/MediaPipeUnityPlugin/issues/120)) ([33c0bd3](https://github.com/homuler/MediaPipeUnityPlugin/commit/33c0bd375b623d47c918cdb4c7d22ad87253013b))
* nuget fails on Ubuntu or Debian ([e00e4ae](https://github.com/homuler/MediaPipeUnityPlugin/commit/e00e4ae8d35af780e5ff8fa38b09efbea433b473))
* rules_foreign_cc version must be 0.1.0 ([780d405](https://github.com/homuler/MediaPipeUnityPlugin/commit/780d405da003f472f2b8e14fdd9c71ef21c616f6))
* wildcard is not expanded ([dd99867](https://github.com/homuler/MediaPipeUnityPlugin/commit/dd9986746cb746e06cc7a4ec846f59eaa3cc3990))
* **example:** failed to compile when the target is Android ([92cad4b](https://github.com/homuler/MediaPipeUnityPlugin/commit/92cad4bbe9ba52514034c52ac5b5f0a99accab06))

### [0.5.1](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.5.0...v0.5.1) (2021-03-21)


### ⚠ BREAKING CHANGES

* place ResourceManager classes under SDK directory

### Features

* **example:** Objectron ([bd470eb](https://github.com/homuler/MediaPipeUnityPlugin/commit/bd470eb3a90af6e25e2aa95195458a662fb60965))


### Bug Fixes

* add a missing command ([de132bd](https://github.com/homuler/MediaPipeUnityPlugin/commit/de132bdf8fe5eed96fab1d5775a0dd93812a5399))
* build error ([fe2f97f](https://github.com/homuler/MediaPipeUnityPlugin/commit/fe2f97f1410a2189b89e93e13101c2e30508caab))
* build for iOS and Android ([7a815f9](https://github.com/homuler/MediaPipeUnityPlugin/commit/7a815f951ef552a924db76d1eaf6033155e13768))
* failed to apply a patch ([049ca2f](https://github.com/homuler/MediaPipeUnityPlugin/commit/049ca2fabe5b5c7e3d4f87cb5aa1b3bae8ea865e))
* graph starts before saving model files ([7f9f6b0](https://github.com/homuler/MediaPipeUnityPlugin/commit/7f9f6b02d43a6990f465f8f2a2b0e76676a5a546))


* place ResourceManager classes under SDK directory ([c67a456](https://github.com/homuler/MediaPipeUnityPlugin/commit/c67a456c1dde283dd165ad4e7650bb197b6e9720))

## [0.5.0](https://github.com/homuler/MediapipeUnityPlugin/compare/v0.4.3...v0.5.0) (2021-02-22)


### ⚠ BREAKING CHANGES

* pkg_model -> pkg_asset

### Features

* **sdk:** implement FloatArrayPacket ([9a0780c](https://github.com/homuler/MediapipeUnityPlugin/commit/9a0780c87c2d425a7c1f3678f95dd003308a7913))
* compile StickerBuffer ([0a4720b](https://github.com/homuler/MediapipeUnityPlugin/commit/0a4720b485f055f6502f73456607f5cda7210cf5))
* implement APIs to get model matrices ([fe35345](https://github.com/homuler/MediapipeUnityPlugin/commit/fe35345a5908f1579da39ceb9a5c7f583e64ea36))
* InstantMotionTracking ([ee7c909](https://github.com/homuler/MediapipeUnityPlugin/commit/ee7c90938d2b97414fa4269ff5321da3cf7f20f2))
* StringPacket can contain null bytes ([1d5240e](https://github.com/homuler/MediapipeUnityPlugin/commit/1d5240e4e7a868401c57376582b84586c1495923))


### Bug Fixes

* **example:** not to create redundunt instances ([56ee74f](https://github.com/homuler/MediapipeUnityPlugin/commit/56ee74f24446b8b598176fe68c9d42c4fa1a09da))
* **test:** thread hangs ([32f30f3](https://github.com/homuler/MediapipeUnityPlugin/commit/32f30f3622e71613803d55f1619f388ee782d005))
* not to flip input images horizontally when mediapipe outputs images ([28e429c](https://github.com/homuler/MediapipeUnityPlugin/commit/28e429c15d8db1e828afe7bc715e9c231f55a706))
* typo ([ee91e5b](https://github.com/homuler/MediapipeUnityPlugin/commit/ee91e5b8db7e8a9c9b923f2c17e24bec81fbfde4))


* pkg_model -> pkg_asset ([20083ff](https://github.com/homuler/MediapipeUnityPlugin/commit/20083ffdd8fcef89764f751a232e85531ae15bba))

### [0.4.3](https://github.com/homuler/MediapipeUnityPlugin/compare/v0.4.2...v0.4.3) (2021-02-07)


### Features

* Holistic with iris ([0bf2bdd](https://github.com/homuler/MediapipeUnityPlugin/commit/0bf2bddf9e98fcc02ccc7bc63f05536da05eb487))


### Bug Fixes

* **example:** request user permission before searching webcam devices ([9954a52](https://github.com/homuler/MediapipeUnityPlugin/commit/9954a5264e7d9bde3f80a915de28582f99b69056))
* add missing attributes ([683b88f](https://github.com/homuler/MediapipeUnityPlugin/commit/683b88f1906befc98c1e3ea30cfb2afd69c49cd1))
* **example:** stop disposing gpu_helper ([94deaba](https://github.com/homuler/MediapipeUnityPlugin/commit/94deaba0599cd8726711fcc37350d1fe1f66099c))
* **example:** Unity Editor hangs after exiting play mode ([6cc0ed1](https://github.com/homuler/MediapipeUnityPlugin/commit/6cc0ed15ee74447cf651b76f01cb9bbe6bfece4b))
* correct compilation conditions ([b787ee3](https://github.com/homuler/MediapipeUnityPlugin/commit/b787ee3f4131fd527ed6d5c6f10592ee00a83115))
* standalone executable does not shut down ([85c77aa](https://github.com/homuler/MediapipeUnityPlugin/commit/85c77aa079aa18233f422503e9b457a762233212))

### [0.4.2](https://github.com/homuler/MediapipeUnityPlugin/compare/v0.4.1...v0.4.2) (2021-01-30)


### Features

* **example:** Box Tracking ([c6d5560](https://github.com/homuler/MediapipeUnityPlugin/commit/c6d55602a1c8bdc91fdcb04d34f819a82fe2d6a6))
* render pose landmarks ([6a26384](https://github.com/homuler/MediapipeUnityPlugin/commit/6a263849aa80d31ee2ad54b53a17b75cc3cd6553))
* render pose roi and detection ([026b6c8](https://github.com/homuler/MediapipeUnityPlugin/commit/026b6c833a35165e471616e7f621d974778b246c))
* **example:** holistic on CPU, Android and iOS ([47ddbdd](https://github.com/homuler/MediapipeUnityPlugin/commit/47ddbddab0f0631fbb6ba465d3d29aa655edf60c))
* **sdk:** build for windows ([aab99a8](https://github.com/homuler/MediapipeUnityPlugin/commit/aab99a8d3374b7f06ed6af828484713a017450b2))


### Bug Fixes

* **example:** remove stale annotations ([61f2041](https://github.com/homuler/MediapipeUnityPlugin/commit/61f20419aac6c92caf34a1968def936505461acf))
* **example:** timestamp can be same as the previous one on Windows ([411fc68](https://github.com/homuler/MediapipeUnityPlugin/commit/411fc68e906f0fe88fd627312b958e9883109072))
* cannot make ios_arm64 ([43e0cba](https://github.com/homuler/MediapipeUnityPlugin/commit/43e0cba8c50d657622fabc09db1b96126fbeea5c))
* limit path length for Windows ([4838408](https://github.com/homuler/MediapipeUnityPlugin/commit/48384080bdaa469c2231eb38371bfbc780e42d43))
* made it possible to run editor while on iOS/Android platform target ([#37](https://github.com/homuler/MediapipeUnityPlugin/issues/37)) ([b7e3bb7](https://github.com/homuler/MediapipeUnityPlugin/commit/b7e3bb7d2e83213058c790bd32d801b92e6d4285))
* maybe required to preload ([6646d56](https://github.com/homuler/MediapipeUnityPlugin/commit/6646d565ce3d9dffa4974893d21e285531a738e8))
* patch (box tracking) ([8063e6d](https://github.com/homuler/MediapipeUnityPlugin/commit/8063e6d03a49ef0bd44756cdec39d962856d2626))
* use microseconds as a Timestamp value ([8e00358](https://github.com/homuler/MediapipeUnityPlugin/commit/8e003585b1d257d5b76d6c05d1205e3458cf2730))
* wrong file path ([14fc75d](https://github.com/homuler/MediapipeUnityPlugin/commit/14fc75db3dc7b9df90e86ad3aa959b98a93224cd))

### [0.4.1](https://github.com/homuler/MediapipeUnityPlugin/compare/v0.4.0...v0.4.1) (2021-01-17)


### Features

* add android_arm64 target ([70d46d9](https://github.com/homuler/MediapipeUnityPlugin/commit/70d46d9b2926c2e603f0d9143742fe6419fe412b))

## [0.4.0](https://github.com/homuler/MediapipeUnityPlugin/compare/v0.3.0...v0.4.0) (2021-01-16)


### ⚠ BREAKING CHANGES

* define callbacks as static methods for IL2CPP

### Features

* **sdk:** GlCalculatorHelper#GetGlContext ([3238cf6](https://github.com/homuler/MediapipeUnityPlugin/commit/3238cf6ce0598f28076967c8f67c20794e273833))


### Bug Fixes

* remove readonly ([7fdf71a](https://github.com/homuler/MediapipeUnityPlugin/commit/7fdf71a401ea4c8e50b84444e3518bbfc558ab38))
* suppress C linkage errors ([461d019](https://github.com/homuler/MediapipeUnityPlugin/commit/461d019eb04545397aa398496e8e066140eb5312))
* wait for WebCamTexture to be initialized ([8fd204c](https://github.com/homuler/MediapipeUnityPlugin/commit/8fd204cfed441340566618f49fdf8e7a1eb11b2f))


* define callbacks as static methods for IL2CPP ([0172e01](https://github.com/homuler/MediapipeUnityPlugin/commit/0172e01bf59a4a8593530ec14742f19b076e2575))

## [0.3.0](https://github.com/homuler/MediapipeUnityPlugin/compare/v0.2.2...v0.3.0) (2020-12-31)


### ⚠ BREAKING CHANGES

* **example:** pass the input texture name to MediaPipe
* **example:** MediaPipe rotates the input image
* use OpenGL ES on Android

### Features

* **example:** enable to specify the output texture on Android ([18e00a8](https://github.com/homuler/MediapipeUnityPlugin/commit/18e00a85ac271b123178e593184184e8715ed22e))
* **example:** implement TextureFramePool ([9723843](https://github.com/homuler/MediapipeUnityPlugin/commit/972384368459628b151b79c7f799f2f9547b93fd))
* **example:** pass the input texture name to MediaPipe ([7f3628d](https://github.com/homuler/MediapipeUnityPlugin/commit/7f3628dcdce156c35f865fbfa354ecd3b28aa867))
* **example:** render external textures directly ([ebbf0ed](https://github.com/homuler/MediapipeUnityPlugin/commit/ebbf0ed4367748254a42cda891674817ee78cb33))
* add Official Demo Graph ([7a340f9](https://github.com/homuler/MediapipeUnityPlugin/commit/7a340f97da91f71c9c3850cd88fa549fd908e4b5))
* use OpenGL ES on Android ([a10efce](https://github.com/homuler/MediapipeUnityPlugin/commit/a10efce8ead138192a1d6fc71796078bce529fc3))
* **sdk:** add GlTextureBuffer APIs ([1643620](https://github.com/homuler/MediapipeUnityPlugin/commit/1643620ed92fe7630213f058c19864b40810a443))
* **sdk:** build CalculatorGraphConfig.cs from proto srcs ([9e576e2](https://github.com/homuler/MediapipeUnityPlugin/commit/9e576e2e2141ed9995ee8abdd8d1b4a68d3b69b7))
* **sdk:** CreateDestinationTexture from GpuBuffer ([48ba090](https://github.com/homuler/MediapipeUnityPlugin/commit/48ba0902f1e1071f5a14831fbb148e0777329121))
* **sdk:** implement EglSurfaceHolder ([380a8c5](https://github.com/homuler/MediapipeUnityPlugin/commit/380a8c5e8d719846a0d12c71a9ba1a4b680d2f7d))
* **sdk:** implement name related APIs under mediapipe::tool ([e967820](https://github.com/homuler/MediapipeUnityPlugin/commit/e967820c8f43c9c2809aa75841acdaaacfd2fa97))


### Bug Fixes

* SIGSEGV when GlTextureBuffer is destroyed ([a1e4cc0](https://github.com/homuler/MediapipeUnityPlugin/commit/a1e4cc074861403160962c7c0ddc1da4f41823b3))
* **example:** use FlowLimiterCalculator in OfficialDemoCPU graph ([f92d71a](https://github.com/homuler/MediapipeUnityPlugin/commit/f92d71a33fc39c0f99ebe7c0e435a4d623f1de1d))


* **example:** MediaPipe rotates the input image ([0eda94c](https://github.com/homuler/MediapipeUnityPlugin/commit/0eda94cd25686664a70c594c29fc6617ef55ac94))

### [0.2.2](https://github.com/homuler/MediapipeUnityPlugin/compare/v0.2.1...v0.2.2) (2020-12-19)


### Features

* add APIs to initialize GpuResources with the current context ([c66d255](https://github.com/homuler/MediapipeUnityPlugin/commit/c66d25583caf0e410bd9011e8d0509b61eaa967d))
* add Glog APIs for debugging ([085f7cc](https://github.com/homuler/MediapipeUnityPlugin/commit/085f7ccdedc24b302a4eb46c4d1f38ee32d27a8c))
* implement CalculatorGraph#ObserveOutputStream ([fad3ca1](https://github.com/homuler/MediapipeUnityPlugin/commit/fad3ca1c77da4100a2cc3f7d8b02513354d081b7))


### Bug Fixes

* add missing dependencies for Mac ([94a2a76](https://github.com/homuler/MediapipeUnityPlugin/commit/94a2a76b73b3c98dc25c9bb20e2fea0e34e83e7c))
* build for macOS ([b24e3eb](https://github.com/homuler/MediapipeUnityPlugin/commit/b24e3eb93d17e3c49e6523e2064503cbbbf3a029))
* **sdk:** set GlTextureBuffer's shared pointer ([5bc68c7](https://github.com/homuler/MediapipeUnityPlugin/commit/5bc68c75d64a0858f2bfcfc9832ba5559a55f7ac))
* hangs if resource files not found ([a4186c2](https://github.com/homuler/MediapipeUnityPlugin/commit/a4186c2d7bd0c587421d7e42f00825007ffd3185))
* I/F has changed ([65bc5a7](https://github.com/homuler/MediapipeUnityPlugin/commit/65bc5a7fb59aafd3c16e9704902ec7aa9a0b8126))
* method name typo ([4c642b5](https://github.com/homuler/MediapipeUnityPlugin/commit/4c642b58d07d6cd4dfd8091b9065111996dfb034))
* typo ([ef78637](https://github.com/homuler/MediapipeUnityPlugin/commit/ef78637b8393c4e043cc58eb1b9a1cf3f4f305bb))
* width <-> height ([27d2eff](https://github.com/homuler/MediapipeUnityPlugin/commit/27d2effb9519678383245465368eb111cd1f8977))
* width <-> height ([62e4fb5](https://github.com/homuler/MediapipeUnityPlugin/commit/62e4fb52b0cdec010d2f084cb2d1fc5b1f7689bf))

### [0.2.1](https://github.com/homuler/MediapipeUnityPlugin/compare/v0.2.0...v0.2.1) (2020-11-24)


### Features

* **sdk:** GlContext and GlSyncToken API ([79b3e83](https://github.com/homuler/MediapipeUnityPlugin/commit/79b3e83983e37a77b9cedb76a7e90a1218317ea7))
* **sdk:** implement GlSyncToken API ([9f8435e](https://github.com/homuler/MediapipeUnityPlugin/commit/9f8435efac84345db90211a04502c8a116075fb4))
* **sdk:** implement GlTextureBuffer API ([31d3deb](https://github.com/homuler/MediapipeUnityPlugin/commit/31d3deb11045c112aac2281991a90176c68ac7a9))
* **sdk:** implement Timestamp API ([f647755](https://github.com/homuler/MediapipeUnityPlugin/commit/f647755382063cb48583fbd8af00d86d99833366))
* add CalculatorGraph APIs ([679c3fb](https://github.com/homuler/MediapipeUnityPlugin/commit/679c3fb19851464d72fd162fc1b8cb073e888e56))
* add GlContext APIs ([755a643](https://github.com/homuler/MediapipeUnityPlugin/commit/755a64327f7c7fccf5499d702bbdfb818d339b38))
* handle SIGABRT ([ee0c4c3](https://github.com/homuler/MediapipeUnityPlugin/commit/ee0c4c33aaf81c5265443a15d9266ac16efbbf55))
* implement IntPacket ([2eea5b2](https://github.com/homuler/MediapipeUnityPlugin/commit/2eea5b28a30a0a224d94ff0d2586b8b149123c47))
* upgrade MediaPipe(0.8.0) ([3b58752](https://github.com/homuler/MediapipeUnityPlugin/commit/3b58752e860b6bf1055bdbe88911b715bccf340e))


### Bug Fixes

* add missing dependencies ([6524d57](https://github.com/homuler/MediapipeUnityPlugin/commit/6524d57331e3d724c8cd28c31ba42e2366864b74))
* **build:** fix aar path ([6cab320](https://github.com/homuler/MediapipeUnityPlugin/commit/6cab3202b65ec86357bac32af18ae49b7ebe0ec2))
* **sdk:** correct GpuBufferFormat values ([260a048](https://github.com/homuler/MediapipeUnityPlugin/commit/260a04826beadbf5a950187c4520e170a06b5708))
* fails to longjmp sometimes ([f884865](https://github.com/homuler/MediapipeUnityPlugin/commit/f88486520ddd1a7f5f083a36841b5ab1879cb41a))
* ptr is not set ([7c6af2f](https://github.com/homuler/MediapipeUnityPlugin/commit/7c6af2fc1993a7810d2185ca5945c1c7e6afaf90))
* thread hangs when an error occured in RunInGlContext ([388899b](https://github.com/homuler/MediapipeUnityPlugin/commit/388899b94aacebd1ec8b18a6b2e943b94ba6accb))

## [0.2.0](https://github.com/homuler/MediapipeUnityPlugin/compare/v0.1.1...v0.2.0) (2020-10-14)


### Features

* build for Android ([d058ddd](https://github.com/homuler/MediapipeUnityPlugin/commit/d058ddd717aa32f0482423443e72a5933c276df1))
* **example:** read local files in UnityEditor env ([e0e8795](https://github.com/homuler/MediapipeUnityPlugin/commit/e0e87951d9e4d304f60797280d96c58de3462f6d))
* **example:** use AssetBundle ([25ce035](https://github.com/homuler/MediapipeUnityPlugin/commit/25ce035e0ac29b67fa678fb0aa444813ed06b56b))
* **sdk:** implement path resolver for unity ([468d444](https://github.com/homuler/MediapipeUnityPlugin/commit/468d4441b0b268dd1a0ca11b18c5ed4b61522f4d))


### Bug Fixes

* **example:** wait for the assets to be loaded ([4cd0d2e](https://github.com/homuler/MediapipeUnityPlugin/commit/4cd0d2ec9e4b279ff839d7c70f12eee1e09b1103))

### [0.1.1](https://github.com/homuler/MediaPipeUnityPlugin/compare/v0.1.0...v0.1.1) (2020-09-27)


### Features

* **example:** enable to switch graph ([2bb63f4](https://github.com/homuler/MediaPipeUnityPlugin/commit/2bb63f44b8f17cb223b4e25e9f1dae1650ce7c3d))


### Bug Fixes

* **example:** close the input stream ([26038da](https://github.com/homuler/MediaPipeUnityPlugin/commit/26038da460fc7e206cbc6419e4bd91b1939ce2b6))
