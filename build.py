import argparse
import glob
import os
import platform
import shutil
import stat
import subprocess

try:
  from ctypes import windll
  windll.kernel32.SetConsoleMode(windll.kernel32.GetStdHandle(-11), 7)
except ImportError:
  pass

_BAZEL_BIN_PATH = 'bazel-bin'
_BAZEL_OUT_PATH = 'bazel-out'
_BUILD_PATH = 'build'
_NUGET_PATH = '.nuget'
_ANALYZER_PATH = os.path.join('Assets', 'Analyzers')
_STREAMING_ASSETS_PATH = os.path.join('Assets', 'StreamingAssets')
_INSTALL_PATH = os.path.join('Packages', 'com.github.homuler.mediapipe', 'Runtime')

class Console:
  def __init__(self, verbose):
    self.verbose = verbose

  def v(self, message):
    if self.verbose > 0:
      self.log(33, 'DEBUG', message)

  def info(self, message):
    self.log(32, 'INFO', message)

  def warn(self, message):
    self.log(35, 'WARN', message)

  def error(self, message):
    self.log(31, 'ERROR', message)

  def log(self, color, level, message):
    print('\033[' + str(color) + 'm' + level + '\033[0m (build.py): ' + message)


class Command:
  def __init__(self, command_args):
    self.verbose = command_args.args.verbose
    self.console = Console(self.verbose)

  def _run_command(self, command_list, shell=True):
    self.console.v(f"Running `{' '.join(command_list)}`")

    if shell:
      return subprocess.run(' '.join(command_list), check=True, shell=shell)

    return subprocess.run(command_list, check=True)

  def _copy(self, source, dest, mode=0o755):
    self.console.v(f"Copying '{source}' to '{dest}'...")
    dest_dir = os.path.dirname(dest)

    if not os.path.exists(dest_dir):
      self.console.v(f"Creating '{dest_dir}'...")
      os.makedirs(dest_dir, 0o755)

    shutil.copy(source, dest)
    self.console.v(f"Changing the mode of '{dest}'...")
    os.chmod(dest, mode)

  def _copytree(self, source, dest):
    self.console.v(f"Copying '{source}' to '{dest}' recursively...")

    if not os.path.exists(dest):
      self.console.v(f"Creating '{dest}'...")
      os.makedirs(dest, 0o755)

    # `shutil.copytree` fails on Windows if target file exists, so run `cp -r` instead.
    self._run_command(['cp', '-r', f'{source}/*', dest])

  def _remove(self, path):
    self.console.v(f"Removing '{path}'...")
    os.remove(path)

  def _rmtree(self, path):
    if os.path.exists(path):
      self.console.v(f"Removing '{path}'...")
      shutil.rmtree(path)
    else:
      self.console.v(f"Tried to remove '{path}', but it does not exist")

  def _unzip(self, source, dest):
    self.console.v(f"Unarchiving '{source}' to '{dest}'...")
    shutil.unpack_archive(source, dest)


class BuildCommand(Command):
  def __init__(self, command_args):
    Command.__init__(self, command_args)

    self.system = platform.system()
    self.desktop = command_args.args.desktop
    self.android = command_args.args.android
    self.ios= command_args.args.ios
    self.resources = command_args.args.resources
    self.analyzers = command_args.args.analyzers
    self.opencv = command_args.args.opencv
    self.include_opencv_libs = command_args.args.include_opencv_libs

    self.compilation_mode = command_args.args.compilation_mode
    self.linkopt = command_args.args.linkopt

  def run(self):
    self.console.info('Building protobuf sources...')
    self._run_command(self._build_proto_srcs_commands())
    self._unzip(
      os.path.join(_BAZEL_BIN_PATH, 'mediapipe_api', 'mediapipe_proto_srcs.zip'),
      os.path.join(_BUILD_PATH, 'Scripts', 'Protobuf'))
    self.console.info('Built protobuf sources')

    self.console.info('Downloading dlls...')
    self._run_command(self._build_proto_dlls_commands())

    for f in glob.glob(os.path.join(_NUGET_PATH, '**', 'lib', 'netstandard2.0', '*.dll'), recursive=True):
      basename = os.path.basename(f)
      self._copy(f, os.path.join(_BUILD_PATH, 'Plugins', 'Protobuf', basename))

    self.console.info('Downloaded protobuf dlls')

    if self.resources:
      self.console.info('Building resource files')
      self._run_command(self._build_resources_commands())
      self._unzip(
        os.path.join(_BAZEL_BIN_PATH, 'mediapipe_api', 'mediapipe_assets.zip'),
        os.path.join(_BUILD_PATH, 'Resources'))
      self._unzip(
        os.path.join(_BAZEL_BIN_PATH, 'mediapipe_api', 'mediapipe_assets.zip'),
        _STREAMING_ASSETS_PATH)

      self.console.info('Built resource files')

    if self.desktop:
      self.console.info('Building native libraries for Desktop...')
      self._run_command(self._build_desktop_commands())
      self._unzip(
        os.path.join(_BAZEL_BIN_PATH, 'mediapipe_api', 'mediapipe_desktop.zip'),
        os.path.join(_BUILD_PATH, 'Plugins'))

      if self.include_opencv_libs:
        if self.opencv == 'cmake':
          self.console.warn('OpenCV objects are included in libmediapipe_c, so skip copying OpenCV library files')
        else:
          self._run_command(self._build_opencv_libs())
          self._unzip(
            os.path.join(_BAZEL_BIN_PATH, 'mediapipe_api', 'opencv_libs.zip'),
            os.path.join(_BUILD_PATH, 'Plugins', 'OpenCV'))

      self.console.info('Built native libraries for Desktop')

    if self.android:
      self.console.info('Building native libraries for Android...')
      self._run_command(self._build_android_commands())
      self._copy(
        os.path.join(_BAZEL_BIN_PATH, 'mediapipe_api', 'java', 'com', 'github', 'homuler', 'mediapipe', 'mediapipe_android.aar'),
        os.path.join(_BUILD_PATH, 'Plugins', 'Android', 'mediapipe_android.aar'))

      self.console.info('Built native libraries for Android')

    if self.ios:
      self.console.info('Building native libaries for iOS...')
      self._run_command(self._build_ios_commands())
      self._unzip(self._find_latest_built_framework(), os.path.join(_BUILD_PATH, 'Plugins', 'iOS'))

      self.console.info('Built native libraries for iOS')

    self.console.info('Installing built resources...')
    # _copytree fails on Windows, so run `cp -r` instead.
    self._copytree(_BUILD_PATH, _INSTALL_PATH)

    # install analyzers
    if self.analyzers:
      self.console.info('Installing Roslyn Analyzers...')
      for f in glob.glob(os.path.join(_NUGET_PATH, '**', 'analyzers', 'dotnet', 'cs', '*.dll'), recursive=True):
        self._copy(f, _ANALYZER_PATH)

    self.console.info('Installed')

  def _is_windows(self):
    return self.system == 'Windows'

  def _build_common_commands(self):
    commands = ['bazel']

    if self._is_windows():
      # limit the path length for Windows
      # @see https://docs.bazel.build/versions/master/windows.html#avoid-long-path-issues
      commands += ['--output_user_root', 'C:/_bzl']

    commands += ['build', '-c', self.compilation_mode]
    commands += self._build_linkopt()

    if self._is_windows():
      python_bin_path_key = 'PYTHON_BIN_PATH'
      if python_bin_path_key not in os.environ:
        raise RuntimeError(f'`{python_bin_path_key}` is not set')

      python_bin_path = os.environ[python_bin_path_key].replace('\\', '//')
      commands += ['--action_env', f'{python_bin_path_key}="{python_bin_path}"']

      # Required to compile OpenCV
      # Without this environment variable, Visual Studio instances won't be found
      # cf. https://github.com/bazelbuild/rules_foreign_cc/issues/793
      program_data_key = 'ProgramData'
      if program_data_key in os.environ:
        commands += ['--action_env', program_data_key]

      # Enable CMake to detect processors when configuring OpenCV
      processor_architecture_key = 'PROCESSOR_ARCHITECTURE'
      if processor_architecture_key in os.environ:
        commands += ['--action_env', processor_architecture_key]

      processor_identifier_key = 'PROCESSOR_IDENTIFIER'
      if processor_identifier_key in os.environ:
        commands += ['--action_env', processor_identifier_key]

      processor_level_key = 'PROCESSOR_LEVEL'
      if processor_level_key in os.environ:
        commands += ['--action_env', processor_level_key]

      processor_revision_key = 'PROCESSOR_REVISION'
      if processor_revision_key in os.environ:
        commands += ['--action_env', processor_revision_key]

    if self.verbose > 1:
      commands.append('--verbose_failures')

    if self.verbose > 2:
      commands.append('--sandbox_debug')

    return commands

  def _build_linkopt(self):
    if self.linkopt is None or len(self.linkopt) == 0:
      return []

    return ['--linkopt={}'.format(l) for l in self.linkopt]

  def _build_opencv_switch(self):
    commands = [f'--@opencv//:switch={self.opencv}']

    return commands

  def _build_desktop_options(self):
    commands = []

    if self.desktop == 'gpu':
      commands += ['--copt', '-DMESA_EGL_NO_X11_HEADERS', '--copt', '-DEGL_NO_X11']
    else:
      commands += ['--define', 'MEDIAPIPE_DISABLE_GPU=1']

    commands += self._build_opencv_switch()

    return commands

  def _build_desktop_commands(self):
    if self.desktop is None:
      return []

    commands = self._build_common_commands()
    commands += self._build_desktop_options()
    commands.append('//mediapipe_api:mediapipe_desktop')
    return commands

  def _build_opencv_libs(self):
    if not self.include_opencv_libs:
      return []

    commands = self._build_common_commands()
    commands += self._build_desktop_options()
    commands.append('//mediapipe_api:opencv_libs')

    return commands

  def _build_android_commands(self):
    if self.android is None:
      return []

    commands = self._build_common_commands()
    commands.append(f'--config=android_{self.android}')
    commands.append('//mediapipe_api/java/com/github/homuler/mediapipe:mediapipe_android')
    return commands

  def _build_ios_commands(self):
    if self.ios is None:
      return []

    commands = self._build_common_commands()
    commands += [f'--config=ios_{self.ios}', '--copt=-fembed-bitcode', '--apple_bitcode=embedded']
    commands.append('//mediapipe_api/objc:MediaPipeUnity')
    return commands

  def _build_resources_commands(self):
    if not self.resources:
      return []

    commands = self._build_common_commands()
    commands.append('//mediapipe_api:mediapipe_assets')
    return commands

  def _build_proto_srcs_commands(self):
    commands = self._build_common_commands()
    commands.append('//mediapipe_api:mediapipe_proto_srcs')
    return commands

  def _build_proto_dlls_commands(self):
    return ['nuget', 'install', '-o', _NUGET_PATH, '-Source', 'https://api.nuget.org/v3/index.json']

  def _find_latest_built_framework(self):
    zip_files = glob.glob(os.path.join(_BAZEL_OUT_PATH, '*', 'bin', 'mediapipe_api', 'objc', 'MediaPipeUnity.zip'))

    if not zip_files:
      raise RuntimeError('MediaPipeUnity.zip has not been built yet')

    zip_files.sort(key=lambda x: os.path.getatime(x), reverse=True)
    return zip_files[0]

class CleanCommand(Command):
  def __init__(self, command_args):
    Command.__init__(self, command_args)

  def run(self):
    self._rmtree(_BUILD_PATH)
    self._rmtree(_NUGET_PATH)
    self._run_command(['bazel', 'clean', '--expunge'])


class UninstallCommand(Command):
  def __init__(self, command_args):
    Command.__init__(self, command_args)

    self.desktop = command_args.args.desktop
    self.android = command_args.args.android
    self.ios = command_args.args.ios
    self.resources = command_args.args.resources
    self.protobuf = command_args.args.protobuf
    self.analyzers = command_args.args.analyzers

  def run(self):
    self._rmtree(_BUILD_PATH)

    if self.desktop:
      self.console.info('Uninstalling native libraries for Desktop...')
      for f in glob.glob(os.path.join(_INSTALL_PATH, 'Plugins', '*'), recursive=True):
        if f.endswith('.dll') or f.endswith('.dylib') or f.endswith('.so'):
          self._remove(f)

    if self.android:
      self.console.info('Uninstalling native libraries for Android...')

      aar_path = os.path.join(_INSTALL_PATH, 'Plugins', 'Android', 'mediapipe_android.aar')

      if os.path.exists(aar_path):
        self._remove(aar_path)

    if self.ios:
      self.console.info('Uninstalling native libraries for iOS...')

      ios_framework_path = os.path.join(_INSTALL_PATH, 'Plugins', 'iOS', 'MediaPipeUnity.framework')

      if os.path.exists(ios_framework_path):
        self._rmtree(ios_framework_path)

    if self.resources:
      self.console.info('Uninstalling resource files...')

      for f in glob.glob(os.path.join(_INSTALL_PATH, 'Resources', '*'), recursive=True):
        if not f.endswith('.meta'):
          self._remove(f)

      for f in glob.glob(os.path.join(_STREAMING_ASSETS_PATH, '*'), recursive=False):
        self._remove(f)

    if self.protobuf:
      self.console.info('Uninstalling protobuf sources and dlls...')

      for f in glob.glob(os.path.join(_INSTALL_PATH, 'Plugins', 'Protobuf', '*.dll'), recursive=True):
        self._remove(f)

      for f in glob.glob(os.path.join(_INSTALL_PATH, 'Scripts', 'Protobuf', '*'), recursive=True):
        if not f.endswith('.meta'):
          self._remove(f)

    if self.analyzers:
      self.console.info('Uninstalling analyzers...')

      for f in glob.glob(os.path.join(_ANALYZER_PATH, '*.dll'), recursive=True):
        self._remove(f)


class HelpCommand(Command):
  def __init__(self, args):
    self.args = args

  def run(self):
    self.args.argument_parser.print_help()


class Argument:
  argument_parser = None
  args = None

  def __init__(self):
    self.argument_parser = argparse.ArgumentParser()
    subparsers = self.argument_parser.add_subparsers(dest='command')

    build_command_parser = subparsers.add_parser('build', help='Build and install native libraries')
    build_command_parser.add_argument('--desktop', choices=['cpu', 'gpu'])
    build_command_parser.add_argument('--android', choices=['armv7', 'arm64', 'fat'])
    build_command_parser.add_argument('--ios', choices=['arm64'])
    build_command_parser.add_argument('--resources', action=argparse.BooleanOptionalAction, default=True)
    build_command_parser.add_argument('--analyzers', action=argparse.BooleanOptionalAction, default=False, help='Install Roslyn Analyzers')
    build_command_parser.add_argument('--compilation_mode', '-c', choices=['fastbuild', 'opt', 'dbg'], default='opt')
    build_command_parser.add_argument('--opencv', choices=['local', 'cmake'], default='local', help='Decide to which OpenCV to link for Desktop native libraries')
    build_command_parser.add_argument('--include_opencv_libs', action='store_true', help='Include OpenCV\'s native libraries for Desktop')
    build_command_parser.add_argument('--linkopt', '-l', action='append', help='Linker options')
    build_command_parser.add_argument('--verbose', '-v', action='count', default=0)

    clean_command_parser = subparsers.add_parser('clean', help='Clean cache files')
    clean_command_parser.add_argument('--verbose', '-v', action='count', default=0)

    uninstall_command_parser = subparsers.add_parser('uninstall', help='Remove installed files')
    uninstall_command_parser.add_argument('--desktop', action=argparse.BooleanOptionalAction, default=True)
    uninstall_command_parser.add_argument('--android', action=argparse.BooleanOptionalAction, default=True)
    uninstall_command_parser.add_argument('--ios', action=argparse.BooleanOptionalAction, default=True)
    uninstall_command_parser.add_argument('--resources', action=argparse.BooleanOptionalAction, default=True)
    uninstall_command_parser.add_argument('--protobuf', action=argparse.BooleanOptionalAction, default=True)
    uninstall_command_parser.add_argument('--analyzers', action=argparse.BooleanOptionalAction, default=True)
    uninstall_command_parser.add_argument('--verbose', '-v', action='count', default=0)

    self.args = self.argument_parser.parse_args()

  def command(self):
    if self.args.command == 'build':
      return BuildCommand(self)
    elif self.args.command == 'clean':
      return CleanCommand(self)
    elif self.args.command == 'uninstall':
      return UninstallCommand(self)
    else:
      return HelpCommand(self)


Argument().command().run()
