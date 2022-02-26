// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Unity
{
  [System.Serializable]
  public enum RunningMode
  {
    Async,
    NonBlockingSync,
    Sync,
  }

  public static class RunningModeExtension
  {
    public static bool IsSynchronous(this RunningMode runningMode)
    {
      return runningMode == RunningMode.Sync || runningMode == RunningMode.NonBlockingSync;
    }
  }
}
