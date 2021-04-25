ARG UID=1000
ARG ANDROID_NDK_VERSION=r21e

FROM archlinux:latest AS builder

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
RUN pacman -Syuu sudo --needed --noconfirm
RUN echo -e "%wheel ALL=(ALL) NOPASSWD: ALL\n" > /etc/sudoers.d/01_wheel
RUN useradd -u ${UID} -m mediapipe && usermod -aG wheel mediapipe

USER mediapipe
WORKDIR /home/mediapipe

# install yay
RUN sudo pacman -Sy base-devel git glibc unzip --needed --noconfirm
RUN git clone https://aur.archlinux.org/yay.git
RUN cd yay && makepkg -si --noconfirm
RUN rm -rf yay


FROM builder AS android

ARG ANDROID_NDK_VERSION

RUN yay -Sy android-sdk android-sdk-build-tools android-sdk-platform-tools android-platform --needed --noconfirm

ENV ANDROID_NDK_ZIP android-ndk.zip
ENV ANDROID_NDK_SHA1 c3ebc83c96a4d7f539bd72c241b2be9dcd29bda9

RUN curl -L https://dl.google.com/android/repository/android-ndk-${ANDROID_NDK_VERSION}-linux-x86_64.zip -o ${ANDROID_NDK_ZIP} && \
    (test "$(sha1sum ${ANDROID_NDK_ZIP})" = "${ANDROID_NDK_SHA1}  ${ANDROID_NDK_ZIP}" || { echo 'Checksum Failed'; exit 1; })
RUN sudo unzip android-ndk.zip -d /opt
RUN sudo mv /opt/android-ndk-${ANDROID_NDK_VERSION} /opt/android-ndk
RUN rm android-ndk.zip


FROM builder

# install Android SDK and NDK
COPY --from=android /opt/android-sdk /opt/android-sdk
COPY --from=android /opt/android-ndk /opt/android-ndk
ENV ANDROID_HOME /opt/android-sdk
ENV ANDROID_NDK_HOME /opt/android-ndk

# install FFmpeg
RUN mkdir ffmpeg
ADD docker/linux/x86_64/ffmpeg/ ffmpeg/
RUN cd ffmpeg && \
    yay -Sy && \
    makepkg -si --needed --noconfirm && \
    cd .. && \
    rm -rf ffmpeg

# install dependencies
RUN yay -Sy bazel nuget mesa python-numpy --needed --noconfirm

ADD build.py .
ADD WORKSPACE .
ADD .bazelrc .
ADD packages.config .
ADD mediapipe_api mediapipe_api
ADD third_party third_party

CMD ["/bin/bash"]
