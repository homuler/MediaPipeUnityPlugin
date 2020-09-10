BUILD := default
MODE := gpu

bazelflags.default := -c opt
bazelflags.debug := --compilation_mode=dbg
bazelflags.gpu := --copt -DMESA_EGL_NO_X11_HEADERS --copt -DEGL_NO_X11
bazelflags.cpu := --define MEDIAPIE_DISABLE_GPU=1
BAZELFLAGS := ${bazelflags.${BUILD}} ${bazelflags.${MODE}}

plugindir := Assets/Mediapipe/SDK/Plugins
protobuf_version := 3.13.0
protobuf_tarball := protobuf-$(protobuf_version).tar.gz

all: $(plugindir)/Google.Protobuf.dll $(plugindir)/libmediapipe_api.so

$(plugindir)/libmediapipe_api.so:
	cd C && bazel build ${BAZELFLAGS} //mediapipe_api:mediapipe_c && cd .. && mv -f C/bazel-bin/mediapipe_api/libmediapipe_c.so $(plugindir)

$(plugindir)/Google.Protobuf.dll: Temp/$(protobuf_tarball)
	cd Temp/protobuf-$(protobuf_version)/csharp && ./buildall.sh && mv src/Google.Protobuf/bin/Release/net45/* ../../../$(plugindir)

Temp/$(protobuf_tarball):
	mkdir -p Temp && cd Temp && curl -L https://github.com/protocolbuffers/protobuf/archive/v3.13.0.tar.gz -o $(protobuf_tarball) && tar xf $(protobuf_tarball)

clean:
	rm $(plugindir)/*.dll $(plugindir)/*.so $(plugindir)/*.pdb
