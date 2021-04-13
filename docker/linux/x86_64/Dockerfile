ARG UID=1000
ARG OPENCV_VERSION=3.4.12
ARG ANDROID_NDK_VERSION=r21e

FROM archlinux:latest AS builder

ARG UID

# https://serverfault.com/questions/1052963/pacman-doesnt-work-in-docker-image
RUN patched_glibc=glibc-linux4-2.33-4-x86_64.pkg.tar.zst && \
    curl -LO "https://repo.archlinuxcn.org/x86_64/$patched_glibc" && \
    bsdtar -C / -xvf "$patched_glibc"

# Create a privileged user
RUN pacman -Syuu sudo --needed --noconfirm
RUN echo -e "%wheel ALL=(ALL) NOPASSWD: ALL\n" > /etc/sudoers.d/01_wheel
RUN useradd -u ${UID} -m mediapipe && usermod -aG wheel mediapipe

USER mediapipe
WORKDIR /home/mediapipe

# install yay
RUN sudo pacman -S base-devel git unzip --needed --noconfirm
RUN git clone https://aur.archlinux.org/yay.git
RUN cd yay && makepkg -si --noconfirm
RUN rm -rf yay


FROM builder AS opencv3

ARG OPENCV_VERSION

# install OpenCV 3
RUN mkdir opencv3
ADD docker/linux/x86_64/opencv3/PKGBUILD opencv3/
RUN cd opencv3 && makepkg -si --noconfirm
RUN rm -rf opencv3


FROM builder AS android

ARG ANDROID_NDK_VERSION

# install Android SDK and NDK
RUN yay -S android-sdk android-sdk-build-tools android-sdk-platform-tools android-platform --needed --noconfirm
RUN curl -L https://dl.google.com/android/repository/android-ndk-${ANDROID_NDK_VERSION}-linux-x86_64.zip -o android-ndk.zip
RUN sudo unzip android-ndk.zip -d /opt
RUN sudo mv /opt/android-ndk-${ANDROID_NDK_VERSION} /opt/android-ndk
RUN rm android-ndk.zip


FROM builder

# install dependencies
RUN yay -S bazel nuget glibc mesa python-numpy --needed --noconfirm

COPY --from=opencv3 /opt/opencv3 /usr
COPY --from=android /opt/android-sdk /opt/android-sdk
COPY --from=android /opt/android-ndk /opt/android-ndk

ENV ANDROID_HOME /opt/android-sdk
ENV ANDROID_NDK_HOME /opt/android-ndk

ADD build.py .
ADD WORKSPACE .
ADD .bazelrc .
ADD packages.config .
ADD mediapipe_api mediapipe_api
ADD third_party third_party

CMD ["/bin/bash"]
