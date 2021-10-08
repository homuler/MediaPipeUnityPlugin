// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using UnityEngine;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity
{
  public class WaitForResult : CustomYieldInstruction
  {
    public object result { get; private set; }

    protected object tmpResult;
    protected bool isDone = false;

    private readonly MonoBehaviour _runner;
    private readonly IEnumerator _inner;
    private readonly Coroutine _coroutine;

    public bool isError { get; private set; } = false;
    public Exception error { get; private set; }
    public override bool keepWaiting => !isDone && !isError;

    public WaitForResult(MonoBehaviour runner, IEnumerator inner, long timeoutMillisec = long.MaxValue)
    {
      _runner = runner;
      _inner = inner;
      _coroutine = runner.StartCoroutine(Run(timeoutMillisec));
    }

    private IEnumerator Run(long timeoutMillisec)
    {
      var stopwatch = new Stopwatch();
      stopwatch.Start();

      while (true)
      {
        try
        {
          if (stopwatch.ElapsedMilliseconds > timeoutMillisec)
          {
            _runner.StopCoroutine(_coroutine);
            throw new TimeoutException($"{stopwatch.ElapsedMilliseconds}ms has passed");
          }
          if (!_inner.MoveNext())
          {
            break;
          }
          tmpResult = _inner.Current;
        }
        catch (Exception e)
        {
          isError = true;
          error = e;
          yield break;
        }
        yield return tmpResult;
      }
      Done(tmpResult);
    }

    protected virtual void Done(object result)
    {
      this.result = result;
      isDone = true;
    }
  }

  public class WaitForResult<T> : WaitForResult
  {
    public new T result { get; private set; }

    public WaitForResult(MonoBehaviour runner, IEnumerator inner, long timeoutMillisec = long.MaxValue) : base(runner, inner, timeoutMillisec) { }

    protected override void Done(object result)
    {
      this.result = (T)result;
      isDone = true;
    }
  }
}
