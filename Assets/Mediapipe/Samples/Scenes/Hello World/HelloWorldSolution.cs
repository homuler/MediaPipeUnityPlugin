using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity.HelloWorld
{
  public class HelloWorldSolution : Solution
  {
    [SerializeField] HelloWorldGraph graphRunner;
    public int loop = 10;
    public RunningMode runningMode;

    Coroutine coroutine;

    public override void Play()
    {
      Debug.Log("Play");
      if (coroutine != null)
      {
        Stop();
      }
      base.Play();
      graphRunner.Initialize();
      coroutine = StartCoroutine(Run());
    }

    public override void Stop()
    {
      base.Stop();
      StopCoroutine(coroutine);
      graphRunner.Stop();
    }

    IEnumerator Run()
    {
      Logger.LogInfo(TAG, $"Running Mode = {runningMode}");

      if (runningMode == RunningMode.Async)
      {
        graphRunner.OnOutput.AddListener(OnOutput);
        graphRunner.StartRunAsync().AssertOk();
      }
      else
      {
        graphRunner.StartRun().AssertOk();
      }

      var count = loop;
      while (count-- > 0)
      {
        yield return new WaitWhile(() => isPaused);

        graphRunner.AddTextToInputStream("Hello World!").AssertOk();

        if (runningMode == RunningMode.Sync)
        {
          // When running synchronously, wait for the outputs here (blocks the main thread).
          var output = graphRunner.FetchNextValue();
          Logger.Log("HelloWorld (Sync)", output);
        }

        yield return new WaitForEndOfFrame();
      }
    }

    void OnOutput(string output)
    {
      Logger.Log("HelloWorld (Async)", output);
    }
  }
}
