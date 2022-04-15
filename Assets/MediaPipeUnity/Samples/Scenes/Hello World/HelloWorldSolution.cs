// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.HelloWorld
{
  public class HelloWorldSolution : Solution
  {
    [SerializeField] private HelloWorldGraph _graphRunner;
    public int loop = 10;
    public RunningMode runningMode;

    private Coroutine _coroutine;

    public override void Play()
    {
      Debug.Log("Play");
      if (_coroutine != null)
      {
        Stop();
      }
      base.Play();
      _graphRunner.Initialize();
      _coroutine = StartCoroutine(Run());
    }

    public override void Stop()
    {
      base.Stop();
      StopCoroutine(_coroutine);
      _graphRunner.Stop();
    }

    private IEnumerator Run()
    {
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      if (runningMode == RunningMode.Async)
      {
        _graphRunner.OnOutput.AddListener(OnOutput);
        _graphRunner.StartRunAsync().AssertOk();
      }
      else
      {
        _graphRunner.StartRun().AssertOk();
      }

      var count = loop;
      while (count-- > 0)
      {
        yield return new WaitWhile(() => isPaused);

        _graphRunner.AddTextToInputStream("Hello World!").AssertOk();

        if (runningMode == RunningMode.Sync)
        {
          // When running synchronously, wait for the outputs here (blocks the main thread).
          var output = _graphRunner.FetchNextValue();
          Logger.Log("HelloWorld (Sync)", output);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    private void OnOutput(string output)
    {
      Logger.Log("HelloWorld (Async)", output);
    }
  }
}
