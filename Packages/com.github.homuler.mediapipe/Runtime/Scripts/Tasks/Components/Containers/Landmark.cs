// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;

// TODO: use System.MathF
using Mathf = UnityEngine.Mathf;

namespace Mediapipe.Tasks.Components.Containers
{
  /// <summary>
  ///   Landmark represents a point in 3D space with x, y, z coordinates. The
  ///   landmark coordinates are in meters. z represents the landmark depth, and the
  ///   smaller the value the closer the world landmark is to the camera.
  /// </summary>
  public readonly struct Landmark : IEquatable<Landmark>
  {
    private const float _LandmarkTolerance = 1e-6f;

    public readonly float x;
    public readonly float y;
    public readonly float z;
    /// <summary>
    ///   Landmark visibility. Should stay unset if not supported.
    ///   Float score of whether landmark is visible or occluded by other objects.
    ///   Landmark considered as invisible also if it is not present on the screen
    ///   (out of scene bounds). Depending on the model, visibility value is either a
    ///   sigmoid or an argument of sigmoid.
    /// </summary>
    public readonly float? visibility;
    /// <summary>
    ///   Landmark presence. Should stay unset if not supported.
    ///   Float score of whether landmark is present on the scene (located within
    ///   scene bounds). Depending on the model, presence value is either a result of
    ///   sigmoid or an argument of sigmoid function to get landmark presence
    ///   probability.
    /// </summary>
    public readonly float? presence;
    /// <summary>
    ///   Landmark name. Should stay unset if not supported.
    /// </summary>
    public readonly string name;

    internal Landmark(float x, float y, float z, float? visibility, float? presence) : this(x, y, z, visibility, presence, null)
    {
    }

    internal Landmark(float x, float y, float z, float? visibility, float? presence, string name)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.visibility = visibility;
      this.presence = presence;
      this.name = name;
    }

#nullable enable
    public override bool Equals(object? obj) => obj is Landmark other && Equals(other);
#nullable disable

    bool IEquatable<Landmark>.Equals(Landmark other)
    {
      return Mathf.Abs(x - other.x) < _LandmarkTolerance &&
        Mathf.Abs(y - other.y) < _LandmarkTolerance &&
        Mathf.Abs(z - other.z) < _LandmarkTolerance;
    }

    // TODO: use HashCode.Combine
    public override int GetHashCode() => Tuple.Create(x, y, z).GetHashCode();
    public static bool operator ==(in Landmark lhs, in Landmark rhs) => lhs.Equals(rhs);
    public static bool operator !=(in Landmark lhs, in Landmark rhs) => !(lhs == rhs);

    public static Landmark CreateFrom(Mediapipe.Landmark proto)
    {
      return new Landmark(
        proto.X, proto.Y, proto.Z,
#pragma warning disable IDE0004 // for Unity 2020.3.x
        proto.HasVisibility ? (float?)proto.Visibility : null,
        proto.HasPresence ? (float?)proto.Presence : null
#pragma warning restore IDE0004 // for Unity 2020.3.x
      );
    }

    public override string ToString()
      => $"{{ \"x\": {x}, \"y\": {y}, \"z\": {z}, \"visibility\": {Util.Format(visibility)}, \"presence\": {Util.Format(presence)}, \"name\": \"{name}\" }}";
  }

  /// <summary>
  ///   A normalized version of above Landmark struct. All coordinates should be
  ///   within [0, 1].
  /// </summary>
  public readonly struct NormalizedLandmark : IEquatable<NormalizedLandmark>
  {
    private const float _LandmarkTolerance = 1e-6f;

    public readonly float x;
    public readonly float y;
    public readonly float z;
    public readonly float? visibility;
    public readonly float? presence;
    public readonly string name;

    internal NormalizedLandmark(float x, float y, float z, float? visibility, float? presence) : this(x, y, z, visibility, presence, null)
    {
    }

    internal NormalizedLandmark(float x, float y, float z, float? visibility, float? presence, string name)
    {
      this.x = x;
      this.y = y;
      this.z = z;
      this.visibility = visibility;
      this.presence = presence;
      this.name = name;
    }

#nullable enable
    public override bool Equals(object? obj) => obj is NormalizedLandmark other && Equals(other);
#nullable disable

    bool IEquatable<NormalizedLandmark>.Equals(NormalizedLandmark other)
    {
      return Mathf.Abs(x - other.x) < _LandmarkTolerance &&
        Mathf.Abs(y - other.y) < _LandmarkTolerance &&
        Mathf.Abs(z - other.z) < _LandmarkTolerance;
    }

    // TODO: use HashCode.Combine
    public override int GetHashCode() => Tuple.Create(x, y, z).GetHashCode();
    public static bool operator ==(in NormalizedLandmark lhs, in NormalizedLandmark rhs) => lhs.Equals(rhs);
    public static bool operator !=(in NormalizedLandmark lhs, in NormalizedLandmark rhs) => !(lhs == rhs);

    public static NormalizedLandmark CreateFrom(Mediapipe.NormalizedLandmark proto)
    {
      return new NormalizedLandmark(
        proto.X, proto.Y, proto.Z,
#pragma warning disable IDE0004 // for Unity 2020.3.x
        proto.HasVisibility ? (float?)proto.Visibility : null,
        proto.HasPresence ? (float?)proto.Presence : null
#pragma warning restore IDE0004 // for Unity 2020.3.x
      );
    }

    public override string ToString()
      => $"{{ \"x\": {x}, \"y\": {y}, \"z\": {z}, \"visibility\": {Util.Format(visibility)}, \"presence\": {Util.Format(presence)}, \"name\": \"{name}\" }}";
  }

  /// <summary>
  ///   A list of Landmarks.
  /// </summary>
  public readonly struct Landmarks
  {
    public readonly IReadOnlyList<Landmark> landmarks;

    internal Landmarks(IReadOnlyList<Landmark> landmarks)
    {
      this.landmarks = landmarks;
    }

    public static Landmarks CreateFrom(LandmarkList proto)
    {
      var landmarks = new List<Landmark>(proto.Landmark.Count);
      foreach (var landmark in proto.Landmark)
      {
        landmarks.Add(Landmark.CreateFrom(landmark));
      }
      return new Landmarks(landmarks);
    }

    public override string ToString() => $"{{ \"landmarks\": {Util.Format(landmarks)} }}";
  }

  /// <summary>
  ///   A list of NormalizedLandmarks.
  /// </summary>
  public readonly struct NormalizedLandmarks
  {
    public readonly IReadOnlyList<NormalizedLandmark> landmarks;

    internal NormalizedLandmarks(IReadOnlyList<NormalizedLandmark> landmarks)
    {
      this.landmarks = landmarks;
    }

    public static NormalizedLandmarks CreateFrom(NormalizedLandmarkList proto)
    {
      var landmarks = new List<NormalizedLandmark>(proto.Landmark.Count);
      foreach (var landmark in proto.Landmark)
      {
        landmarks.Add(NormalizedLandmark.CreateFrom(landmark));
      }
      return new NormalizedLandmarks(landmarks);
    }

    public override string ToString() => $"{{ \"landmarks\": {Util.Format(landmarks)} }}";
  }
}
