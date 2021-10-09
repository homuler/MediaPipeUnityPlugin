// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;

namespace Mediapipe.Unity.UI
{
  public class ModalContents : MonoBehaviour
  {
    protected Modal GetModal()
    {
      return gameObject.transform.parent.gameObject.GetComponent<Modal>();
    }

    public virtual void Exit()
    {
      GetModal().Close();
    }
  }
}
