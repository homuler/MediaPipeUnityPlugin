// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Tasks.Components.Containers
{
  /// <summary>
  ///   A keypoint, defined by the coordinates (x, y), normalized by the image dimensions.
  /// </summary>
  public readonly struct NormalizedKeypoint
  {
    /// <summary>
    ///   x in normalized image coordinates.
    /// </summary>
    public readonly float x;
    /// <summary>
    ///   y in normalized image coordinates.
    /// </summary>
    public readonly float y;
    /// <summary>
    ///   optional label of the keypoint.
    /// </summary>
    public readonly string label;
    /// <summary>
    ///   optional score of the keypoint.
    /// </summary>
    public readonly float? score;

    internal NormalizedKeypoint(float x, float y, string label, float? score)
    {
      this.x = x;
      this.y = y;
      this.label = label;
      this.score = score;
    }

    internal NormalizedKeypoint(NativeNormalizedKeypoint nativeKeypoint) : this(
      nativeKeypoint.x,
      nativeKeypoint.y,
      nativeKeypoint.label,
#pragma warning disable IDE0004 // for Unity 2020.3.x
      nativeKeypoint.hasScore ? (float?)nativeKeypoint.score : null)
#pragma warning restore IDE0004 // for Unity 2020.3.x
    {
    }

    public override string ToString() => $"{{ \"x\": {x}, \"y\": {y}, \"label\": \"{label}\", \"score\": {Util.Format(score)} }}";
  }
}
