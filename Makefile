BUILD := default
MODE := gpu

builddir := .build
sdkdir := Assets/MediaPipe/SDK
plugindir := $(sdkdir)/Plugins
modeldir := $(sdkdir)/Models
scriptdir := $(sdkdir)/Scripts

bazelflags.gpu := --copt -DMESA_EGL_NO_X11_HEADERS --copt -DEGL_NO_X11
bazelflags.cpu := --define MEDIAPIPE_DISABLE_GPU=1
bazelflags.android_arm := --config=android_arm
baseltarget.model = //mediapipe_api:mediapipe_models

proto_srcdir := $(scriptdir)/Protobuf

protobuf_version := 3.13.0
protobuf_root := $(builddir)/protobuf-$(protobuf_version)
protobuf_tarball := $(protobuf_root).tar.gz
protobuf_csharpdir := $(protobuf_root)/csharp
protobuf_bindir := $(protobuf_csharpdir)/src/Google.Protobuf/bin/Release/net45
protobuf_dll := $(protobuf_bindir)/Google.Protobuf.dll

bazel_root := C/bazel-bin/mediapipe_api
bazel_models_target := //mediapipe_api:mediapipe_models
bazel_protos_target := //mediapipe_api:mediapipe_proto_srcs
bazel_common_target := $(bazel_models_target) $(bazel_protos_target)

.PHONY: all gpu cpu android_arm clean \
	install install-protobuf install-mediapipe_c install-mediapipe_android install-models \
	uninstall uninstall-protobuf uninstall-mediapipe_c uninstall-mediapipe_android uninstall-models

# build
gpu: | $(protobuf_dll)
	cd C && bazel build -c opt ${bazelflags.gpu} //mediapipe_api:libmediapipe_c.so $(bazel_common_target)

cpu: | $(protobuf_dll)
	cd C && bazel build -c opt ${bazelflags.cpu} //mediapipe_api:libmediapipe_c.so $(bazel_common_target)

mac_cpu: | $(protobuf_dll)
	cd C && bazel build -c opt ${bazelflags.cpu} //mediapipe_api:libmediapipe_c.dylib $(bazel_common_target)

android_arm: | $(protobuf_dll)
	cd C && bazel build -c opt ${bazelflags.android_arm} //mediapipe_api/java/org/homuler/mediapipe/unity:mediapipe_android $(bazel_common_target)

$(plugindir)/Google.Protobuf.dll: Temp/$(protobuf_tarball)
	cd Temp/protobuf-$(protobuf_version)/csharp && ./buildall.sh && mv src/Google.Protobuf/bin/Release/net45/* ../../../$(plugindir)

$(protobuf_dll): | $(protobuf_root)
	./$(protobuf_csharpdir)/buildall.sh

clean:
	rm -r $(builddir) && \
	cd C && \
	bazel clean

# install
install: install-protobuf install-mediapipe_c install-mediapipe_android install-mediapipe_mac install-models install-protos 

install-protobuf: | $(plugindir)/Protobuf
	cp $(protobuf_bindir)/* $(plugindir)/Protobuf

install-mediapipe_c:
ifneq ("$(wildcard $(bazel_root)/libmediapipe_c.so)", "")
	cp -f $(bazel_root)/libmediapipe_c.so $(plugindir)
else
	echo "skip installing libmediapipe_c.so"
endif

install-mediapipe_mac:
ifneq ("$(wildcard $(bazel_root)/libmediapipe_c.dylib)", "")
	cp -f $(bazel_root)/libmediapipe_c.dylib $(plugindir)
else
	echo "skip installing libmediapipe_c.dylib"
endif

install-mediapipe_android:
ifneq ("$(wildcard $(bazel_root)/java/org/homuler/mediapipe/unity/mediapipe_android.aar)", "")
	cp -f $(bazel_root)/java/org/homuler/mediapipe/unity/mediapipe_android.aar $(plugindir)/Android
else
	echo "skip installing mediapipe_android.aar"
endif

install-models: | $(modeldir)
ifneq ("$(wildcard $(bazel_root)/mediapipe_models.zip)", "")
	unzip $(bazel_root)/mediapipe_models.zip -d $(modeldir)
else
	echo "skip installing models"
endif

install-protos: | $(proto_srcdir)
ifneq ("$(wildcard $(bazel_root)/mediapipe_proto_srcs.zip)", "")
	unzip $(bazel_root)/mediapipe_proto_srcs.zip -d $(proto_srcdir)
else
	echo "skip installing proto sources"
endif

uninstall: uninstall-models uninstall-mediapipe_android uninstall-mediapipe_c uninstall-protobuf uninstall-mediapipe_c_mac

uninstall-protobuf:
	rm -r $(plugindir)/Protobuf

uninstall-mediapipe_c:
	rm -f $(plugindir)/libmediapipe_c.so

uninstall-mediapipe_c_mac:
	rm -f $(plugindir)/libmediapipe_c.dylib

uninstall-mediapipe_android:
	rm -f $(plugindir)/Android/mediapipe_android.aar

uninstall-models:
	rm -rf $(modeldir)

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
