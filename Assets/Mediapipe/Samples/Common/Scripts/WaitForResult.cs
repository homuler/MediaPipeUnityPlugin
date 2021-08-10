using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity {
  public class WaitForResult<T> : CustomYieldInstruction {
    public T result { get; private set; }

    object tmpResult;
    bool isDone = false;
    IEnumerator inner;

    public override bool keepWaiting {
      get { return !isDone; }
    }

    public WaitForResult(MonoBehaviour runner, IEnumerator inner) {
      this.inner = inner;
      runner.StartCoroutine(Run());
    }

    IEnumerator Run() {
      while(inner.MoveNext()) {
        tmpResult = inner.Current;
        yield return tmpResult;
      }
      result = (T)tmpResult;
      isDone = true;
    }
  }
}
