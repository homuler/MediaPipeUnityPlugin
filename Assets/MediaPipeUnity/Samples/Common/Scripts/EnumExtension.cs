// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.ComponentModel;

namespace Mediapipe.Unity.Sample
{
  public static class EnumExtension
  {
    public static string GetDescription(this Enum value)
    {
      var type = value.GetType();
      var name = Enum.GetName(type, value);
      if (name == null)
      {
        return null;
      }

      var field = type.GetField(name);
      if (field == null)
      {
        return null;
      }

      var attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
      if (attr is DescriptionAttribute descriptionAttribute)
      {
        return descriptionAttribute.Description;
      }
      return null;
    }
  }
}
