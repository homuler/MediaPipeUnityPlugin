using System.Collections;
using UnityEngine;

namespace Mediapipe.Unity {
  public abstract class SolutionBase : MonoBehaviour {
    protected Bootstrap bootstrap;

    protected virtual IEnumerator Start() {
      bootstrap = GameObject.Find("Bootstrap").GetComponent<Bootstrap>();
      yield return new WaitUntil(() => bootstrap.isFinished);
    }
  }
}
