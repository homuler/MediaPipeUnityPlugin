// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;

namespace Mediapipe
{
  public class ImagePacket : Packet<Image>
  {
    /// <summary>
    ///   Creates an empty <see cref="ImagePacket" /> instance.
    /// </summary>
    public ImagePacket() : base(true) { }

    [UnityEngine.Scripting.Preserve]
    public ImagePacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) { }

    public ImagePacket(Image image) : base()
    {
      UnsafeNativeMethods.mp__MakeImagePacket__Pif(image.mpPtr, out var ptr).Assert();
      image.Dispose(); // respect move semantics

      this.ptr = ptr;
    }

    public ImagePacket(Image image, Timestamp timestamp) : base()
    {
      UnsafeNativeMethods.mp__MakeImagePacket_At__Pif_Rt(image.mpPtr, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp);
      image.Dispose(); // respect move semantics

      this.ptr = ptr;
    }

    public ImagePacket At(Timestamp timestamp)
    {
      return At<ImagePacket>(timestamp);
    }

    public override Image Get()
    {
      UnsafeNativeMethods.mp_Packet__GetImage(mpPtr, out var imagePtr).Assert();

      GC.KeepAlive(this);
      return new Image(imagePtr, false);
    }

    public override Image Consume()
    {
      UnsafeNativeMethods.mp_Packet__ConsumeImage(mpPtr, out var statusPtr, out var imagePtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
      return new Image(imagePtr, true);
    }

    public override void ValidateAsType()
    {
      UnsafeNativeMethods.mp_Packet__ValidateAsImage(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      AssertStatusOk(statusPtr);
    }
  }
}
