// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Threading;
using System.Threading.Tasks;

namespace Mediapipe
{
  internal static class TaskExtension
  {
    // polyfill for Task.WaitAsync
    public static async Task<T> WaitAsync<T>(this Task<T> task, CancellationToken cancellationToken)
    {
      var tcs = new TaskCompletionSource<T>();
      using (cancellationToken.Register((state) => ((TaskCompletionSource<T>)state).TrySetCanceled(), tcs))
      {
        return await (await Task.WhenAny(task, tcs.Task).ConfigureAwait(false)).ConfigureAwait(false);
      }
    }
  }
}
