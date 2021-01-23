builddir := .build
sdkdir := Assets/MediaPipe/SDK
plugindir := $(sdkdir)/Plugins
modeldir := $(sdkdir)/Models
scriptdir := $(sdkdir)/Scripts

bazelflags.gpu := --copt -DMESA_EGL_NO_X11_HEADERS --copt -DEGL_NO_X11
bazelflags.cpu := --define MEDIAPIPE_DISABLE_GPU=1
ifeq ($(OS),Windows_NT)
  bazelflags.cpu += --action_env PYTHON_BIN_PATH="$(PYTHON_BIN_PATH)"
endif
bazelflags.android_arm := --config=android_arm
bazelflags.android_arm64 := --config=android_arm64
bazelflags.ios_arm64 := --config=ios_arm64 --copt=-fembed-bitcode --apple_bitcode=embedded

proto_srcdir := $(scriptdir)/Protobuf

protobuf_version := 3.13.0
protobuf_root := $(builddir)/protobuf-$(protobuf_version)
protobuf_tarball := $(protobuf_root).tar.gz
protobuf_csharpdir := $(protobuf_root)/csharp
protobuf_bindir := $(protobuf_csharpdir)/src/Google.Protobuf/bin/Release/net45
protobuf_dll := $(protobuf_bindir)/Google.Protobuf.dll

bazel_root := C/bazel-bin/mediapipe_api
bazel_desktop_target := mediapipe_api:mediapipe_desktop
bazel_android_target := mediapipe_api/java/org/homuler/mediapipe/unity:mediapipe_android
bazel_ios_target := mediapipe_api/objc:MediaPipeUnity
bazel_models_target := mediapipe_api:mediapipe_models
bazel_protos_target := mediapipe_api:mediapipe_proto_srcs
bazel_common_target := $(bazel_models_target) $(bazel_protos_target)

.PHONY: all gpu cpu android_arm android_arm64 ios_arm64 clean \
	install install-protobuf install-mediapipe_desktop install-mediapipe_android install-mediapipe_ios install-models \
	uninstall uninstall-protobuf uninstall-mediapipe_desktop uninstall-mediapipe_android uninstall-mediapipe_ios uninstall-models

# build
gpu: | $(protobuf_dll)
	cd C && bazel build -c opt $(bazelflags.gpu) $(bazel_desktop_target) $(bazel_common_target)

cpu: | $(protobuf_dll)
	cd C && bazel build -c opt $(bazelflags.cpu) $(bazel_desktop_target) $(bazel_common_target)

android_arm: | $(protobuf_dll)
	cd C && bazel build -c opt $(bazelflags.android_arm) $(bazel_android_target) $(bazel_common_target)

android_arm64: | $(protobuf_dll)
	cd C && bazel build -c opt $(bazelflags.android_arm64) $(bazel_android_target) $(bazel_common_target)

ios_arm64: | $(protobuf_dll)
	cd C && bazel build -c opt $(bazelflags.ios_arm64) $(bazel_ios_target) $(bazel_common_target)

$(plugindir)/Google.Protobuf.dll: Temp/$(protobuf_tarball)
	cd Temp/protobuf-$(protobuf_version)/csharp && ./buildall.sh && mv src/Google.Protobuf/bin/Release/net45/* ../../../$(plugindir)

$(protobuf_dll): | $(protobuf_root)
	./$(protobuf_csharpdir)/buildall.sh

clean:
	rm -r $(builddir) && \
	cd C && \
	bazel clean

# install
install: install-protobuf install-mediapipe_desktop install-mediapipe_android install-mediapipe_ios install-models install-protos

install-protobuf: | $(plugindir)/Protobuf
	cp $(protobuf_bindir)/* $(plugindir)/Protobuf

install-mediapipe_desktop:
ifneq ("$(wildcard $(bazel_root)/mediapipe_desktop.zip)", "")
	unzip $(bazel_root)/mediapipe_desktop.zip -d $(plugindir)
else
	# skip installing mediapipe_desktop
endif

install-mediapipe_android:
ifneq ("$(wildcard $(bazel_root)/java/org/homuler/mediapipe/unity/mediapipe_android.aar)", "")
	cp -f $(bazel_root)/java/org/homuler/mediapipe/unity/mediapipe_android.aar $(plugindir)/Android
else
	# skip installing mediapipe_android.aar
endif

install-mediapipe_ios:
ifneq ("$(wildcard $(bazel_root)/objc/MediaPipeUnity.zip)", "")
	unzip $(bazel_root)/objc/MediaPipeUnity.zip -d $(plugindir)/iOS
else
	# skip installing MediaPipeUnityFramework.zip
endif


install-models: | $(modeldir)
ifneq ("$(wildcard $(bazel_root)/mediapipe_models.zip)", "")
	unzip $(bazel_root)/mediapipe_models.zip -d $(modeldir)
else
	# skip installing models
endif

install-protos: | $(proto_srcdir)
ifneq ("$(wildcard $(bazel_root)/mediapipe_proto_srcs.zip)", "")
	unzip $(bazel_root)/mediapipe_proto_srcs.zip -d $(proto_srcdir)
else
	# skip installing proto sources
endif

uninstall: uninstall-models uninstall-mediapipe_ios uninstall-mediapipe_android uninstall-mediapipe_desktop uninstall-protobuf

uninstall-protobuf:
	rm -r $(plugindir)/Protobuf

uninstall-mediapipe_desktop:
	rm -f $(plugindir)/libmediapipe_c.so && rm -f $(plugindir)/libmediapipe_c.dylib	&& rm -f $(plugindir)/libmediapipe_c.dll

uninstall-mediapipe_android:
	rm -f $(plugindir)/Android/mediapipe_android.aar

uninstall-mediapipe_ios:
	rm -rf $(plugindir)/iOS/MediaPipeUnity.framework

uninstall-models:
	rm -f $(modeldir)/*.bytes && rm -f $(modeldir)/*.txt

# create directories
$(builddir):
	mkdir -p $@

$(plugindir)/Protobuf:
	mkdir -p $@

$(modeldir):
	mkdir -p $@

# sources
$(protobuf_root): | $(protobuf_tarball)
	tar xf $(protobuf_tarball) -C $(builddir)

$(protobuf_tarball): | $(builddir)
	curl -L https://github.com/protocolbuffers/protobuf/archive/v$(protobuf_version).tar.gz -o $@
