ARG UID=1000
ARG ANDROID_SDK_VERSION=30
ARG ANDROID_NDK_VERSION=21.4.7075529

FROM archlinux:latest AS base

ARG RANKMIRROS
ARG MIRROR_COUNTRY=JP,US
ARG MIRROR_COUNT=10

# Update mirrorlist
RUN if [[ "${RANKMIRROS}" ]]; then \
        pacman -Syu pacman-contrib --needed --noconfirm && \
        { \
            readarray -td, a <<< "${MIRROR_COUNTRY},"; \
            declare -p a; unset 'a[-1]'; \
            curl -s "https://archlinux.org/mirrorlist/?protocol=https$(printf "&country=%s" "${a[@]}")&use_mirror_status=on" | \
            sed -e 's/^#Server/Server/' -e '/^#/d' | \
            rankmirrors -n ${MIRROR_COUNT} - > /etc/pacman.d/mirrorlist; \
        } && \
        tee -a /etc/pacman.d/mirrorlist <<< 'Server = http://mirrors.evowise.com/archlinux/$repo/os/$arch' && \
        tee -a /etc/pacman.d/mirrorlist <<< 'Server = http://mirror.rackspace.com/archlinux/$repo/os/$arch' && \
        tee -a /etc/pacman.d/mirrorlist <<< 'Server = https://mirror.rackspace.com/archlinux/$repo/os/$arch'; \
    fi

ARG UID

# Create a privileged user
RUN pacman -Sy archlinux-keyring --noconfirm && pacman -Syuu sudo --needed --noconfirm
RUN echo -e "%wheel ALL=(ALL) NOPASSWD: ALL\n" > /etc/sudoers.d/01_wheel

# Support systemd-homed
RUN sed -r -i 's/^UID_MAX\s*[0-9]+/UID_MAX\t\t\t60513/' /etc/login.defs && \
    sed -r -i 's/^GID_MAX\s*[0-9]+/GID_MAX\t\t\t60513/' /etc/login.defs

RUN useradd -u ${UID} -m mediapipe && usermod -aG wheel mediapipe

USER mediapipe
WORKDIR /home/mediapipe

# install yay
RUN sudo pacman -Sy base-devel git glibc jdk11-openjdk unzip zip --needed --noconfirm
RUN git clone https://aur.archlinux.org/yay.git
RUN cd yay && makepkg -si --noconfirm
RUN rm -rf yay


FROM base AS android

USER root
WORKDIR /tmp

ARG ANDROID_SDK_VERSION
ARG ANDROID_NDK_VERSION

ENV COMMANDLINETOOLS_ZIP commandlinetools.zip
ENV COMMANDLINETOOLS_SHA256 87f6dcf41d4e642e37ba03cb2e387a542aa0bd73cb689a9e7152aad40a6e7a08

RUN curl -L https://dl.google.com/android/repository/commandlinetools-linux-6858069_latest.zip -o ${COMMANDLINETOOLS_ZIP} && \
    (test "$(sha256sum ${COMMANDLINETOOLS_ZIP})" = "${COMMANDLINETOOLS_SHA256}  ${COMMANDLINETOOLS_ZIP}" || { echo 'Checksum Failed'; exit 1; })
RUN sudo unzip ${COMMANDLINETOOLS_ZIP} -d /opt/android
RUN yes | /opt/android/cmdline-tools/bin/sdkmanager --sdk_root=/opt/android --licenses
RUN /opt/android/cmdline-tools/bin/sdkmanager --sdk_root=/opt/android --install \
    "platforms;android-${ANDROID_SDK_VERSION}" \
    "build-tools;30.0.3" \
    "ndk;${ANDROID_NDK_VERSION}"
RUN rm ${COMMANDLINETOOLS_ZIP}


FROM base AS builder

# install Android SDK and NDK
COPY --from=android /opt/android /opt/android

ENV NPM_PREFIX /home/mediapipe/.npm-packages
ENV PATH ${NPM_PREFIX}/bin:/home/mediapipe/.local/bin:${PATH}

# install Bazelisk
RUN yay -Sy npm --needed --noconfirm && \
    echo -e "prefix = ${NPM_PREFIX}\n" > /home/mediapipe/.npmrc && \
    npm install -g @bazel/bazelisk

# install other dependencies
RUN yay -Sy mesa nuget python-pip --needed --noconfirm && \
    pip install --user numpy

FROM builder

ARG ANDROID_NDK_VERSION

ENV ANDROID_HOME /opt/android
ENV ANDROID_NDK_HOME /opt/android/ndk/${ANDROID_NDK_VERSION}

COPY packages.config .
COPY .bazelrc .
COPY .bazelversion .
COPY build.py .
COPY WORKSPACE .
COPY mediapipe_api mediapipe_api
COPY third_party third_party

CMD ["/bin/bash"]
