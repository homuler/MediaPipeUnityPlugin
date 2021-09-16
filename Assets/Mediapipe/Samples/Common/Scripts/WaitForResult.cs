using System;
using System.Collections;
using UnityEngine;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity {
  public class WaitForResult : CustomYieldInstruction {
    public object result { get; private set; }

    protected object tmpResult;
    protected bool isDone = false;

    readonly MonoBehaviour runner;
    readonly IEnumerator inner;
    readonly Coroutine coroutine;

    public bool isError { get; private set; } = false;
    public Exception error { get; private set; }
    public override bool keepWaiting {
      get { return !isDone && !isError; }
    }

    public WaitForResult(MonoBehaviour runner, IEnumerator inner, long timeoutMillisec = Int64.MaxValue) {
      this.runner = runner;
      this.inner = inner;
      coroutine = runner.StartCoroutine(Run(timeoutMillisec));
    }

    IEnumerator Run(long timeoutMillisec) {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();

      while(true) {
        try {
          if (stopwatch.ElapsedMilliseconds > timeoutMillisec) {
            runner.StopCoroutine(coroutine);
            throw new TimeoutException($"{stopwatch.ElapsedMilliseconds}ms has passed");
          }
          if (!inner.MoveNext()) {
            break;
          }
          tmpResult = inner.Current;
        } catch (Exception e) {
          isError = true;
          error = e;
          yield break;
        }
        yield return tmpResult;
      }
      Done(tmpResult);
    }

    protected virtual void Done(object result) {
      this.result = result;
      isDone = true;
    }
  }

  public class WaitForResult<T> : WaitForResult {
    public new T result { get; private set; }

    public WaitForResult(MonoBehaviour runner, IEnumerator inner, long timeoutMillisec = Int64.MaxValue) : base(runner, inner, timeoutMillisec) {}

    protected override void Done(object result) {
      this.result = (T)result;
      isDone = true;
    }
  }
}
