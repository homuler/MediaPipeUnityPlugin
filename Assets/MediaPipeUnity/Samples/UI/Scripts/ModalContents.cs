// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mediapipe.Unity.Sample.UI
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

    protected void InitializeDropdown<T>(Dropdown dropdown, string defaultValue)
    {
      dropdown.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(T)));
      dropdown.AddOptions(options);

      var defaultValueIndex = options.FindIndex(option => option == defaultValue);

      if (defaultValueIndex >= 0)
      {
        dropdown.value = defaultValueIndex;
      }
    }
  }
}
