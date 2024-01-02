// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

// TODO: use System.MathF
using Mathf = UnityEngine.Mathf;

namespace Mediapipe.Tasks.Components.Containers
{
  /// <summary>
  ///   Defines a rectangle, used e.g. as part of detection results or as input region-of-interest.
  /// </summary>
  public readonly struct Rect
  {
    public readonly int left;
    public readonly int top;
    public readonly int right;
    public readonly int bottom;

    internal Rect(int left, int top, int right, int bottom)
    {
      this.left = left;
      this.top = top;
      this.right = right;
      this.bottom = bottom;
    }

    internal Rect(NativeRect nativeRect) : this(nativeRect.left, nativeRect.top, nativeRect.right, nativeRect.bottom) { }

    public override string ToString() => $"{{ \"left\": {left}, \"top\": {top}, \"right\": {right}, \"bottom\": {bottom} }}";
  }

  /// <summary>
  ///   A rectangle, used as part of detection results or as input region-of-interest.
  ///
  ///   The coordinates are normalized wrt the image dimensions, i.e. generally in
  ///   [0,1] but they may exceed these bounds if describing a region overlapping the
  ///   image. The origin is on the top-left corner of the image.
  /// </summary>
  public readonly struct RectF : IEquatable<RectF>
  {
    private const float _RectFTolerance = 1e-4f;

    public readonly float left;
    public readonly float top;
    public readonly float right;
    public readonly float bottom;

    internal RectF(float left, float top, float right, float bottom)
    {
      this.left = left;
      this.top = top;
      this.right = right;
      this.bottom = bottom;
    }

    internal RectF(NativeRectF nativeRect) : this(nativeRect.left, nativeRect.top, nativeRect.right, nativeRect.bottom) { }

#nullable enable
    public override bool Equals(object? obj) => obj is RectF other && Equals(other);
#nullable disable

    bool IEquatable<RectF>.Equals(RectF other)
    {
      return Mathf.Abs(left - other.left) < _RectFTolerance &&
        Mathf.Abs(top - other.top) < _RectFTolerance &&
        Mathf.Abs(right - other.right) < _RectFTolerance &&
        Mathf.Abs(bottom - other.bottom) < _RectFTolerance;
    }

    // TODO: use HashCode.Combine
    public override int GetHashCode() => Tuple.Create(left, top, right, bottom).GetHashCode();
    public static bool operator ==(RectF lhs, RectF rhs) => lhs.Equals(rhs);
    public static bool operator !=(RectF lhs, RectF rhs) => !(lhs == rhs);

    public override string ToString() => $"{{ \"left\": {left}, \"top\": {top}, \"right\": {right}, \"bottom\": {bottom} }}";
  }
}
