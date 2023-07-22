// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using UnityEngine;

namespace Mediapipe.Tasks.Core
{
  internal class PacketsCallbackTable
  {
    private const int _MaxSize = 20;

    private static int _Counter = 0;
    private static readonly GlobalInstanceTable<int, TaskRunner.PacketsCallback> _Table = new GlobalInstanceTable<int, TaskRunner.PacketsCallback>(_MaxSize);

    public static (int, TaskRunner.NativePacketsCallback) Add(TaskRunner.PacketsCallback callback)
    {
      var callbackId = _Counter++;
      _Table.Add(callbackId, callback);
      return (callbackId, InvokeCallbackIfFound);
    }

    public static bool TryGetValue(int id, out TaskRunner.PacketsCallback callback) => _Table.TryGetValue(id, out callback);

    [AOT.MonoPInvokeCallback(typeof(TaskRunner.NativePacketsCallback))]
    private static void InvokeCallbackIfFound(int callbackId, IntPtr statusPtr, IntPtr packetMapPtr)
    {
      // NOTE: if status is not OK, packetMap will be nullptr
      if (packetMapPtr == IntPtr.Zero)
      {
        var status = new Status(statusPtr, false);
        Debug.LogError(status.ToString());
        return;
      }

      if (TryGetValue(callbackId, out var callback))
      {
        callback(new PacketMap(packetMapPtr, false));
      }
    }
  }
}
