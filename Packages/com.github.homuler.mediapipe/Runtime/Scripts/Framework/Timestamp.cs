// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class Timestamp : MpResourceHandle, IEquatable<Timestamp>
  {
    public Timestamp(IntPtr ptr) : base(ptr) { }

    public Timestamp(long value) : base()
    {
      UnsafeNativeMethods.mp_Timestamp__l(value, out var ptr).Assert();
      this.ptr = ptr;
    }

    protected override void DeleteMpPtr()
    {
      UnsafeNativeMethods.mp_Timestamp__delete(ptr);
    }

    #region IEquatable<Timestamp>
    public bool Equals(Timestamp other)
    {
      return other != null && Microseconds() == other.Microseconds();
    }

#pragma warning disable IDE0049
    public override bool Equals(System.Object obj)
    {
      var timestampObj = obj == null ? null : (obj as Timestamp);

      return timestampObj != null && Equals(timestampObj);
    }
#pragma warning restore IDE0049

#pragma warning disable IDE0002
    public static bool operator ==(Timestamp x, Timestamp y)
    {
      return (((object)x) == null || ((object)y) == null) ? System.Object.Equals(x, y) : x.Equals(y);
    }

    public static bool operator !=(Timestamp x, Timestamp y)
    {
      return (((object)x) == null || ((object)y) == null) ? !System.Object.Equals(x, y) : !x.Equals(y);
    }
#pragma warning restore IDE0002

    public override int GetHashCode()
    {
      return Microseconds().GetHashCode();
    }
    #endregion

    public long Value()
    {
      return SafeNativeMethods.mp_Timestamp__Value(mpPtr);
    }

    public double Seconds()
    {
      return SafeNativeMethods.mp_Timestamp__Seconds(mpPtr);
    }

    public long Microseconds()
    {
      return SafeNativeMethods.mp_Timestamp__Microseconds(mpPtr);
    }

    public bool IsSpecialValue()
    {
      return SafeNativeMethods.mp_Timestamp__IsSpecialValue(mpPtr);
    }

    public bool IsRangeValue()
    {
      return SafeNativeMethods.mp_Timestamp__IsRangeValue(mpPtr);
    }

    public bool IsAllowedInStream()
    {
      return SafeNativeMethods.mp_Timestamp__IsAllowedInStream(mpPtr);
    }

    public string DebugString()
    {
      return MarshalStringFromNative(UnsafeNativeMethods.mp_Timestamp__DebugString);
    }

    public Timestamp NextAllowedInStream()
    {
      UnsafeNativeMethods.mp_Timestamp__NextAllowedInStream(mpPtr, out var nextPtr).Assert();

      GC.KeepAlive(this);
      return new Timestamp(nextPtr);
    }

    public Timestamp PreviousAllowedInStream()
    {
      UnsafeNativeMethods.mp_Timestamp__PreviousAllowedInStream(mpPtr, out var prevPtr).Assert();

      GC.KeepAlive(this);
      return new Timestamp(prevPtr);
    }

    public static Timestamp FromSeconds(double seconds)
    {
      UnsafeNativeMethods.mp_Timestamp_FromSeconds__d(seconds, out var ptr).Assert();

      return new Timestamp(ptr);
    }

    #region SpecialValues
    public static Timestamp Unset()
    {
      UnsafeNativeMethods.mp_Timestamp_Unset(out var ptr).Assert();

      return new Timestamp(ptr);
    }

    public static Timestamp Unstarted()
    {
      UnsafeNativeMethods.mp_Timestamp_Unstarted(out var ptr).Assert();

      return new Timestamp(ptr);
    }

    public static Timestamp PreStream()
    {
      UnsafeNativeMethods.mp_Timestamp_PreStream(out var ptr).Assert();

      return new Timestamp(ptr);
    }

    public static Timestamp Min()
    {
      UnsafeNativeMethods.mp_Timestamp_Min(out var ptr).Assert();

      return new Timestamp(ptr);
    }

    public static Timestamp Max()
    {
      UnsafeNativeMethods.mp_Timestamp_Max(out var ptr).Assert();

      return new Timestamp(ptr);
    }

    public static Timestamp PostStream()
    {
      UnsafeNativeMethods.mp_Timestamp_PostStream(out var ptr).Assert();

      return new Timestamp(ptr);
    }

    public static Timestamp OneOverPostStream()
    {
      UnsafeNativeMethods.mp_Timestamp_OneOverPostStream(out var ptr).Assert();

      return new Timestamp(ptr);
    }

    public static Timestamp Done()
    {
      UnsafeNativeMethods.mp_Timestamp_Done(out var ptr).Assert();

      return new Timestamp(ptr);
    }
    #endregion
  }
}
