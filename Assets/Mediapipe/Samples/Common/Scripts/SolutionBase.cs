using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity {
  public abstract class SolutionBase : MonoBehaviour {
    protected Bootstrap bootstrap;
    protected bool isPaused;

    protected virtual IEnumerator Start() {
      bootstrap = GameObject.Find("Bootstrap").GetComponent<Bootstrap>();
      yield return new WaitUntil(() => bootstrap.isFinished);

      Play();
    }

    /// <summary>
    ///   Start the main program from the beginning.
    /// </summary>
    public virtual void Play() {
      isPaused = false;
    }

    /// <summary>
    ///   Pause the main program.
    /// <summary>
    public virtual void Pause() {
      isPaused = true;
    }

    /// <summary>
    ///    Resume the main program.
    ///    If the main program has not begun, it'll do nothing.
    /// </summary>
    public virtual void Resume() {
      isPaused = false;
    }

    /// <summary>
    ///   Stops the main program.
    /// </summary>
    public virtual void Stop() {
      isPaused = true;
    }
  }
}
