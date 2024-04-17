// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity.Sample.UI
{
  public class Modal : MonoBehaviour
  {
    [SerializeField] private TaskApiRunner _taskApiRunner;

    private GameObject _contents;

    public void Open(GameObject contents)
    {
      _contents = Instantiate(contents, gameObject.transform);
      _contents.transform.localScale = new Vector3(0.8f, 0.8f, 1);
      gameObject.SetActive(true);
    }

    public void OpenAndPause(GameObject contents)
    {
      Open(contents);
      _taskApiRunner?.Pause();
    }

    public void CloseAndResume(bool forceRestart = false)
    {
      gameObject.SetActive(false);

      if (_contents != null)
      {
        Destroy(_contents);
      }

      if (_taskApiRunner != null)
      {
        if (forceRestart)
        {
          _taskApiRunner.Play();
        }
        else
        {
          _taskApiRunner.Resume();
        }
      }
    }
  }
}
