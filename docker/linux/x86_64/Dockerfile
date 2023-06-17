ARG UID=1000
ARG ANDROID_SDK_BUILD_TOOLS_VERSION=30.0.3
ARG ANDROID_SDK_VERSION=30
ARG ANDROID_NDK_VERSION=21.4.7075529

FROM ubuntu:20.04 AS base

ENV DEBIAN_FRONTEND=noninteractive

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        apt-transport-https \
        build-essential \
        ca-certificates \
        clang \
        curl \
        dirmngr \
        git \
        gnupg \
        libegl1-mesa-dev \
        libgles2-mesa-dev \
        mesa-common-dev \
        nasm \
        npm \
        openjdk-11-jdk \
        pkg-config \
        software-properties-common \
        sudo \
        unzip \
        zip

FROM base AS android

WORKDIR /tmp

ARG ANDROID_SDK_BUILD_TOOLS_VERSION
ARG ANDROID_SDK_VERSION
ARG ANDROID_NDK_VERSION

ENV COMMANDLINETOOLS_ZIP commandlinetools.zip
ENV COMMANDLINETOOLS_SHA256 124f2d5115eee365df6cf3228ffbca6fc3911d16f8025bebd5b1c6e2fcfa7faf


RUN curl -L https://dl.google.com/android/repository/commandlinetools-linux-7583922_latest.zip -o ${COMMANDLINETOOLS_ZIP} && \
    (test "$(sha256sum ${COMMANDLINETOOLS_ZIP})" = "${COMMANDLINETOOLS_SHA256}  ${COMMANDLINETOOLS_ZIP}" || { echo 'Checksum Failed'; exit 1; }) && \
    unzip ${COMMANDLINETOOLS_ZIP} -d /opt/android

RUN  yes | /opt/android/cmdline-tools/bin/sdkmanager --sdk_root=/opt/android --licenses
RUN /opt/android/cmdline-tools/bin/sdkmanager --sdk_root=/opt/android --install \
        "platforms;android-${ANDROID_SDK_VERSION}" \
        "build-tools;${ANDROID_SDK_BUILD_TOOLS_VERSION}" \
        "ndk;${ANDROID_NDK_VERSION}" && \
    rm ${COMMANDLINETOOLS_ZIP}


FROM base AS builder

ARG UID
WORKDIR /tmp

# install Android SDK and NDK
COPY --from=android /opt/android /opt/android

# Setup Python
RUN apt-get install -y --no-install-recommends python3.9 python3.9-dev python3.9-distutils && \
    curl -L https://bootstrap.pypa.io/get-pip.py -o get-pip.py && \
    python3.9 get-pip.py && \
    update-alternatives --install /usr/local/bin/python python /usr/bin/python3.9 10 && \
    update-alternatives --install /usr/local/bin/pip pip /usr/local/bin/pip3.9 10

# Install NuGet
RUN gpg --homedir /tmp --no-default-keyring --keyring /usr/share/keyrings/mono-official-archive-keyring.gpg --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF && \
    echo "deb [signed-by=/usr/share/keyrings/mono-official-archive-keyring.gpg] https://download.mono-project.com/repo/ubuntu stable-focal main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list && \
    apt update

####
# NOTE
#   At the moment (2021.12), the next step is very unstable in some country (https://github.com/mono/mono/issues/21273).
#
#   If you encounter 504 Timeout errors, insert RUN commands as follows and install those *.deb files forcefully (note `; exit 0` at the end).
#
#   # if failing to download libmono-system-reactive-providers2.2-cil_6.12.0.122-0xamarin1+ubuntu1804b1_all.deb...
#   RUN curl -X GET https://download.mono-project.com/repo/ubuntu/pool/main/m/mono/libmono-system-reactive-providers2.2-cil_6.12.0.122-0xamarin1+ubuntu1804b1_all.deb -o libmono-system-reactive-providers2.2-cil_6.12.0.122-0xamarin1+ubuntu1804b1_all.deb && \
#       dpkg -i libmono-system-reactive-providers2.2-cil_6.12.0.122-0xamarin1+ubuntu1804b1_all.deb ; exit 0
#
#   # Repeat if failing to download other files
#   # RUN curl -X GET ... -o ... ; exit 0
#
#   RUN apt --fix-broken install -y

RUN apt-get install -y --no-install-recommends mono-complete && \
    curl -o /usr/local/bin/nuget.exe https://dist.nuget.org/win-x86-commandline/latest/nuget.exe && \
    bash -c 'echo -e "#!/bin/bash\nmono /usr/local/bin/nuget.exe \$@" | tee -a /usr/local/bin/nuget' && \
    chmod +x /usr/local/bin/nuget

# install Bazelisk
RUN apt-get install -y --no-install-recommends npm && \
    npm install -g @bazel/bazelisk

# Create user
RUN bash -c 'echo -e "%wheel ALL=(ALL) NOPASSWD: ALL\n" > /etc/sudoers.d/01_wheel'
RUN addgroup wheel && useradd -u ${UID} -m mediapipe && usermod -aG wheel mediapipe

USER mediapipe
WORKDIR /home/mediapipe

ENV NPM_PREFIX /home/mediapipe/.npm-packages
ENV PATH ${NPM_PREFIX}/bin:/home/mediapipe/.local/bin:${PATH}

# install numpy
RUN pip install --no-cache-dir --user numpy


FROM builder

ARG ANDROID_NDK_VERSION

ENV ANDROID_HOME /opt/android
ENV ANDROID_NDK_HOME /opt/android/ndk/${ANDROID_NDK_VERSION}
ENV PYTHON_BIN_PATH /usr/bin/python3.9

COPY packages.config .
COPY .bazelrc .
COPY .bazelversion .
COPY build.py .
COPY WORKSPACE .
COPY mediapipe_api mediapipe_api
COPY third_party third_party

CMD ["/bin/bash"]
