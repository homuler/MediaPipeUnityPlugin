# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

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
