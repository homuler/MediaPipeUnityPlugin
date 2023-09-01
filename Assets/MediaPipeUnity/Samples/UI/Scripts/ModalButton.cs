// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity.Sample.UI
{
  public class ModalButton : MonoBehaviour
  {
    [SerializeField] private GameObject _modalPanel;
    [SerializeField] private GameObject _contents;

    private Modal modal => _modalPanel.GetComponent<Modal>();

    public void Open()
    {
      if (_contents != null)
      {
        modal.Open(_contents);
      }
    }

    public void OpenAndPause()
    {
      if (_contents != null)
      {
        modal.OpenAndPause(_contents);
      }
    }
  }
}
