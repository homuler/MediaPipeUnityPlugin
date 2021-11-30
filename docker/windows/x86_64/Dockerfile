# escape=`

ARG NODE_VERSION=16.13.0
ARG BAZEL_VERSION=4.2.1
ARG PYTHON_VERSION=3.9.9
ARG ANDROID_SDK_VERSION=30
ARG ANDROID_NDK_VERSION=21.4.7075529

FROM mcr.microsoft.com/windows/servercore:ltsc2019 as base

SHELL ["cmd", "/C"]

RUN mkdir C:\TEMP


FROM base as builder

ARG NODE_VERSION

RUN setx path "C:\msys64\usr\bin;%PATH%"

# Install MSYS2
# TODO: check sha256
RUN curl -L http://repo.msys2.org/distrib/x86_64/msys2-base-x86_64-20210604.sfx.exe -o C:\TEMP\msys2-installer.exe && `
    C:\TEMP\msys2-installer.exe -y -oC:\ && `
    del C:\TEMP\msys2-installer.exe

# This command may hang and in that case, "C:\ProgramData\Docker\tmp\hcs*\Files\$Recycle.Bin\" should be removed manually.
# cf. https://github.com/docker/for-win/issues/8910
RUN bash.exe -l -c "pacman -Syuu --needed --noconfirm --noprogressbar" && `
    bash.exe -l -c "pacman -Syu --needed --noconfirm --noprogressbar" && `
    bash.exe -l -c "pacman -Sy --needed --noconfirm --noprogressbar" && `
    bash.exe -l -c "pacman -S --needed --noconfirm git patch unzip zip p7zip" && `
    bash.exe -l -c "rm -r /var/cache/pacman/pkg/*"

# Install Visual C++ Build Tools 2019 and WinSDK
ARG CHANNEL_URL=https://aka.ms/vs/16/release/channel

RUN curl -L https://nodejs.org/dist/v%NODE_VERSION%/node-v%NODE_VERSION%-x64.msi -o C:\TEMP\node-install.msi && `
    start /wait msiexec.exe /i C:\TEMP\node-install.msi /l*vx "%TEMP%\MSI-node-install.log" /qn ADDLOCAL=ALL && `
    del C:\TEMP\node-install.msi && `
    curl -L %CHANNEL_URL% -o C:\TEMP\VisualStudio.chman && `
    curl -L https://aka.ms/vs/16/release/vs_buildtools.exe -o C:\TEMP\vs_buildtools.exe && `
    C:\TEMP\vs_buildtools.exe --quiet --wait --norestart --nocache `
        --installPath "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools" `
        --channelUri C:\TEMP\VisualStudio.chman `
        --installChannelUri C:\TEMP\VisualStudio.chman `
        --add Microsoft.VisualStudio.Workload.VCTools --includeRecommended && `
    curl -L https://aka.ms/vs/16/release/vc_redist.x64.exe -o C:\TEMP\vc_redist.x64.exe && `
    start /wait C:\TEMP\vc_redist.x64.exe /install /quiet /norestart && `
    del C:\TEMP\vc_redist.x64.exe


FROM base as python

ARG PYTHON_VERSION

# Install Python
RUN curl -L https://www.python.org/ftp/python/%PYTHON_VERSION%/python-%PYTHON_VERSION%-amd64.exe -o C:\TEMP\python-installer.exe && `
    C:\TEMP\python-installer.exe -Wait /quiet InstallAllUsers=1 TargetDir=C:\Python PrependPath=1 Shortcuts=0 Include_doc=0 Include_test=0 && `
    del C:\TEMP\python-installer.exe && `
    C:\Python\python.exe -m pip install --upgrade pip && `
    C:\Python\python.exe -m pip install numpy


FROM base as opencv

# Install OpenCV
RUN curl -L https://github.com/opencv/opencv/releases/download/3.4.10/opencv-3.4.10-vc14_vc15.exe -o C:\TEMP\opencv-installer.exe && `
    start /wait C:\TEMP\opencv-installer.exe -gm2 -y -oC:\ && `
    del C:\TEMP\opencv-installer.exe


FROM builder as android

ARG ANDROID_SDK_VERSION
ARG ANDROID_NDK_VERSION

ENV OPENJDK_ZIP OpenJDK11U-jdk_x64.zip
ENV OPENJDK_SHA256 087d096032efe273d7e754a25c85d8e8cf44738a3e597ad86f55e0971acc3b8e
ENV JAVA_HOME C:\Java\jdk-11.0.13+8

# Install Java
RUN curl -L https://github.com/adoptium/temurin11-binaries/releases/download/jdk-11.0.13%2B8/OpenJDK11U-jdk_x64_windows_hotspot_11.0.13_8.zip -o C:\TEMP\%OPENJDK_ZIP% && `
    for /F %s in ('sha256sum /c/TEMP/%OPENJDK_ZIP%') do ((test "%s" = "%OPENJDK_SHA256%" || (echo %s 'Checksum Failed' && exit 1))) && `
    unzip C:\TEMP\%OPENJDK_ZIP% -d C:\Java && `
    del C:\TEMP\%OPENJDK_ZIP%

ENV COMMANDLINETOOLS_ZIP commandlinetools.zip
ENV COMMANDLINETOOLS_SHA256 f9e6f91743bcb1cc6905648ca751bc33975b0dd11b50d691c2085d025514278c

# Install Android SDK and NDK
RUN curl -L https://dl.google.com/android/repository/commandlinetools-win-7583922_latest.zip -o C:\TEMP\%COMMANDLINETOOLS_ZIP% && `
    for /F %s in ('sha256sum /c/TEMP/%COMMANDLINETOOLS_ZIP%') do ((test "%s" = "%COMMANDLINETOOLS_SHA256%" || (echo 'Checksum Failed' && exit 1))) && `
    unzip C:\TEMP\%COMMANDLINETOOLS_ZIP% -d C:\Android && `
    del C:\TEMP\%COMMANDLINETOOLS_ZIP%

# `yes` command does not work here (standard output: No space left on device)
RUN powershell -command "for($i=0;$i -lt 30;$i++) { $response += """y`n""" }; $response" | C:\Android\cmdline-tools\bin\sdkmanager --sdk_root=C:\Android --licenses && `
    C:\Android\cmdline-tools\bin\sdkmanager --sdk_root=C:\Android --install `
        "platforms;android-%ANDROID_SDK_VERSION%" `
        "platform-tools" `
        "build-tools;30.0.3" `
        "ndk;%ANDROID_NDK_VERSION%"


FROM builder

ARG ANDROID_NDK_VERSION
ARG BAZEL_VERSION

ENV ANDROID_HOME C:\Android
ENV ANDROID_NDK_HOME ${ANDROID_HOME}\ndk\${ANDROID_NDK_VERSION}
ENV JAVA_HOME=C:\Java\jdk-11.0.13+8
ENV PYTHON_INSTALL_PATH=C:\Python
ENV PYTHON_BIN_PATH=${PYTHON_INSTALL_PATH}\python.exe

# Install Bazel and NuGet
RUN setx path "C:\bin;%PYTHON_INSTALL_PATH%;%PYTHON_INSTALL_PATH%\Scripts;%JAVA_HOME%\bin;%PATH%" && `
    mkdir C:\bin && `
    curl -L https://github.com/bazelbuild/bazel/releases/download/%BAZEL_VERSION%/bazel-%BAZEL_VERSION%-windows-x86_64.exe -o C:\bin\bazel.exe && `
    curl -L https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -o C:\bin\nuget.exe

COPY --from=python C:\Python C:\Python
COPY --from=opencv C:\opencv C:\opencv
COPY --from=android C:\Android C:\Android
COPY --from=android C:\Java C:\Java

ENV WS_LONG_PATHS_VERSION 3.4.1
ENV WS_LONG_PATHS_BASE ndk-wsls-${WS_LONG_PATHS_VERSION}
ENV WS_LONG_PATHS_7Z ${WS_LONG_PATHS_BASE}.7z

# Patch for Android NDK
RUN curl -L https://github.com/simdsoft/wsLongPaths/releases/download/v%WS_LONG_PATHS_VERSION%/%WS_LONG_PATHS_7Z% -o C:\TEMP\%WS_LONG_PATHS_7Z% && `
    bash.exe -c "7z x -o/c/TEMP /c/TEMP/${WS_LONG_PATHS_7Z}" && `
    C:\TEMP\%WS_LONG_PATHS_BASE%\install.bat %ANDROID_NDK_HOME% %ANDROID_HOME%

# Must be run in a separated RUN command to avoid "The process cannot access the file because it is being used by another process" error
RUN del C:\TEMP\%WS_LONG_PATHS_7Z% && `
    rmdir /s /q C:\TEMP\%WS_LONG_PATHS_BASE%

WORKDIR C:\mediapipe

ADD .bazelrc .
ADD build.py .
ADD packages.config .
ADD WORKSPACE .
ADD mediapipe_api mediapipe_api
ADD third_party third_party

CMD ["cmd"]
