using UnityEngine;

namespace Mediapipe {
  internal static class AsyncOperationExtension {
    public static AsyncOperationAwaiter<T> GetAwaiter<T>(this T operation) where T : AsyncOperation {
      return new AsyncOperationAwaiter<T>(operation);
    }
  }
}
