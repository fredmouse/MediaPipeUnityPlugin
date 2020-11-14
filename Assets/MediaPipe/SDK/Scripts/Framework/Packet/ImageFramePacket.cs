using System;

namespace Mediapipe {
  public class ImageFramePacket : Packet<ImageFrame> {
    public ImageFramePacket() : base() {}

    public ImageFramePacket(IntPtr ptr, bool isOwner = true) : base(ptr, isOwner) {}

    public ImageFramePacket(ImageFrame imageFrame) : base() {
      UnsafeNativeMethods.mp__MakeImageFramePacket__Pif(imageFrame.mpPtr, out var ptr).Assert();
      imageFrame.Dispose(); // respect move semantics
      this.ptr = ptr;
    }

    public ImageFramePacket(ImageFrame imageFrame, Timestamp timestamp) : base() {
      UnsafeNativeMethods.mp__MakeImageFramePacket_At__Pif_Rtimestamp(imageFrame.mpPtr, timestamp.mpPtr, out var ptr).Assert();
      GC.KeepAlive(timestamp); // respect move semantics
      imageFrame.Dispose();
      this.ptr = ptr;
    }

    public override ImageFrame Get() {
      throw new NotSupportedException();
    }

    public override StatusOr<ImageFrame> Consume() {
      UnsafeNativeMethods.mp_Packet__ConsumeImageFrame(mpPtr, out var statusOrImageFramePtr).Assert();

      return new StatusOrImageFrame(statusOrImageFramePtr);
    }

    public override Status ValidateAsType() {
      UnsafeNativeMethods.mp_Packet__ValidateAsImageFrame(mpPtr, out var statusPtr).Assert();

      GC.KeepAlive(this);
      return new Status(statusPtr);
    }
  }
}
