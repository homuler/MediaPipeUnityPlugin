ARG UID=1000
ARG ANDROID_SDK_BUILD_TOOLS_VERSION=35.0.0
ARG ANDROID_SDK_VERSION=36
ARG ANDROID_NDK_VERSION=28.2.13676358

FROM ubuntu:22.04 AS base

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
        openjdk-17-jdk \
        pkg-config \
        software-properties-common \
        sudo \
        unzip \
        wget \
        zip

# Install gcc-13
RUN add-apt-repository ppa:ubuntu-toolchain-r/test -y && \
    apt-get update && \
    apt-get install -y --no-install-recommends gcc-13 g++-13 && \
    update-alternatives --install /usr/bin/gcc gcc /usr/bin/gcc-13 100 --slave /usr/bin/g++ g++ /usr/bin/g++-13

# Install Clang 16
RUN curl https://apt.llvm.org/llvm.sh -o llvm.sh && \
    chmod +x llvm.sh && \
    ./llvm.sh 16 && \
    ln -sf /usr/bin/clang-16 /usr/bin/clang && \
    ln -sf /usr/bin/clang++-16 /usr/bin/clang++ && \
    ln -sf /usr/bin/clang-format-16 /usr/bin/clang-format

# Upgrade binutils (>= 2.40)
RUN apt-get install -y --no-install-recommends bison texinfo && \
    curl -O https://ftp.gnu.org/gnu/binutils/binutils-2.40.tar.gz && \
    tar -xvzof binutils-2.40.tar.gz && \
    cd binutils-2.40 && \
    mkdir build && \
    cd build && \
    ../configure --prefix=/usr --enable-gold --enable-ld=default && \
    make -j `nproc` && \
    make install

# Install Python 3.12
RUN add-apt-repository ppa:deadsnakes/ppa -y && \
    apt-get update && \
    apt-get install -y --no-install-recommends python3.12 python3.12-dev python3.12-venv python3-pip && \
    curl -sSL https://bootstrap.pypa.io/get-pip.py -o get-pip.py && \
    python3.12 get-pip.py && \
    update-alternatives --install /usr/bin/python python /usr/bin/python3.12 10 && \
    update-alternatives --install /usr/bin/python3 python3 /usr/bin/python3.12 10


FROM base AS android

WORKDIR /tmp

ARG ANDROID_SDK_BUILD_TOOLS_VERSION
ARG ANDROID_SDK_VERSION
ARG ANDROID_NDK_VERSION

ENV COMMANDLINETOOLS_ZIP=commandlinetools.zip
ENV COMMANDLINETOOLS_SHA256=7ec965280a073311c339e571cd5de778b9975026cfcbe79f2b1cdcb1e15317ee


RUN curl -L https://dl.google.com/android/repository/commandlinetools-linux-13114758_latest.zip -o ${COMMANDLINETOOLS_ZIP} && \
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

ENV NPM_PREFIX=/home/mediapipe/.npm-packages
ENV PATH=${NPM_PREFIX}/bin:/home/mediapipe/.local/bin:${PATH}

# install numpy
RUN python -m pip install --no-cache-dir --user numpy


FROM builder

ARG ANDROID_NDK_VERSION

ENV ANDROID_HOME=/opt/android
ENV ANDROID_NDK_HOME=/opt/android/ndk/${ANDROID_NDK_VERSION}
ENV PYTHON_BIN_PATH=/usr/bin/python3.12

COPY packages.config .
COPY .bazelrc .
COPY .bazelversion .
COPY platform_mappings .
COPY build.py .
COPY WORKSPACE .
COPY mediapipe_api mediapipe_api
COPY third_party third_party

CMD ["/bin/bash"]
