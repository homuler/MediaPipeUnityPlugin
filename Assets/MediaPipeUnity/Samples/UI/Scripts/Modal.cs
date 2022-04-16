// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity.UI
{
  public class Modal : MonoBehaviour
  {
    [SerializeField] private Solution _solution;
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
      if (_solution != null)
      {
        _solution.Pause();
      }
    }

    public void Close()
    {
      gameObject.SetActive(false);

      if (_contents != null)
      {
        Destroy(_contents);
      }
    }

    public void CloseAndResume(bool forceRestart = false)
    {
      Close();

      if (_solution == null)
      {
        return;
      }

      if (forceRestart)
      {
        _solution.Play();
      }
      else
      {
        _solution.Resume();
      }
    }
  }
}
