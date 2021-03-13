using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Mediapipe {
  internal class AsyncOperationAwaiter<T> : INotifyCompletion where T : AsyncOperation {
    T _asyncOperation;

    public AsyncOperationAwaiter(T asyncOperation) {
      _asyncOperation = asyncOperation;
    }

    public bool IsCompleted {
      get { return _asyncOperation.isDone; }
    }

    public T GetResult() {
      return _asyncOperation;
    }

    public void OnCompleted(Action continuation) {
      _asyncOperation.completed += _ => { continuation(); };
    }
  }
}
