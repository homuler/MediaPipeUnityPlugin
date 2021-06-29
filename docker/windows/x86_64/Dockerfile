# escape=`

ARG NODE_VERSION=14.16.0
ARG BAZEL_VERSION=4.0.0
ARG PYTHON_VERSION=3.9.3

FROM mcr.microsoft.com/windows/servercore:ltsc2019 as builder

SHELL ["cmd", "/C"]

RUN mkdir C:\TEMP


FROM builder as msys2

RUN setx path "C:\msys64\usr\bin;%PATH%"

# Install MSYS2
# TODO: check sha256
RUN curl -L http://repo.msys2.org/distrib/x86_64/msys2-base-x86_64-20210604.sfx.exe -o C:\TEMP\msys2-installer.exe
RUN C:\TEMP\msys2-installer.exe -y -oC:\ && del C:\TEMP\msys2-installer.exe

# This command may hang and in that case, "C:\ProgramData\Docker\tmp\hcs*\Files\$Recycle.Bin\" should be removed manually.
# cf. https://github.com/docker/for-win/issues/8910
RUN bash.exe -l -c "pacman -Syuu --needed --noconfirm --noprogressbar" && `
    bash.exe -l -c "pacman -Syu --needed --noconfirm --noprogressbar" && `
    bash.exe -l -c "pacman -Sy --needed --noconfirm --noprogressbar" && `
    bash.exe -l -c "pacman -S --needed --noconfirm git patch unzip" && `
    bash.exe -l -c "rm -r /var/cache/pacman/pkg/*"


FROM builder as python

ARG PYTHON_VERSION

# Install Python
RUN curl -L https://www.python.org/ftp/python/%PYTHON_VERSION%/python-%PYTHON_VERSION%-amd64.exe -o C:\TEMP\python-installer.exe
RUN C:\TEMP\python-installer.exe -Wait /quiet InstallAllUsers=1 TargetDir=C:\Python PrependPath=1 Shortcuts=0 Include_doc=0 Include_test=0
RUN python.exe -m pip install --upgrade pip && python.exe -m pip install numpy


FROM builder as opencv

# Install OpenCV
RUN curl -L https://github.com/opencv/opencv/releases/download/3.4.10/opencv-3.4.10-vc14_vc15.exe -o C:\TEMP\opencv-installer.exe
RUN start /wait C:\TEMP\opencv-installer.exe -gm2 -y -o"C:\"


FROM builder

ARG NODE_VERSION
ARG BAZEL_VERSION

ENV PYTHON_INSTALL_PATH=C:\Python
ENV PYTHON_BIN_PATH=${PYTHON_INSTALL_PATH}\python.exe

RUN setx path "C:\bin;%PYTHON_INSTALL_PATH%;%PYTHON_INSTALL_PATH%\Scripts;C:\msys64\usr\bin;%PATH%"

COPY --from=msys2 C:\msys64 C:\msys64
COPY --from=python C:\Python C:\Python
COPY --from=opencv C:\opencv C:\opencv

# Install Bazel and NuGet
RUN mkdir C:\bin && `
    curl -L https://github.com/bazelbuild/bazel/releases/download/%BAZEL_VERSION%/bazel-%BAZEL_VERSION%-windows-x86_64.exe -o C:\bin\bazel.exe && `
    curl -L https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -o C:\bin\nuget.exe

# InstallVisual C++ Build Tools 2019 and WinSDK
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

WORKDIR C:\mediapipe

ADD .bazelrc .
ADD build.py .
ADD packages.config .
ADD WORKSPACE .
ADD mediapipe_api mediapipe_api
ADD third_party third_party

CMD ["cmd"]
