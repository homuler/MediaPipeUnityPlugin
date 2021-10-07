// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe
{
  internal static class Protobuf
  {
    public delegate void ProtobufLogHandler(int level, string filename, int line, string message);
    // TODO: Overwrite protobuf logger to show logs in Console Window.
  }
}
