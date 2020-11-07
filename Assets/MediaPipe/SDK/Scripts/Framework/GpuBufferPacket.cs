using System;
using System.Runtime.InteropServices;

namespace Mediapipe {
  public class GpuBufferPacket : Packet<GpuBuffer> {
    private bool _disposed = false;
    private GCHandle valueHandle;
    public GpuBufferPacket() : base() {}

    public GpuBufferPacket(GpuBuffer gpuBuffer, int timestamp) {
      // TODO: implement
      // base(UnsafeNativeMethods.MpMakeGpuBufferPacketAt(gpuBuffer.GetPtr(), timestamp)) {
      // gpuBuffer.ReleaseOwnership();
      // valueHandle = GCHandle.Alloc(gpuBuffer);
    }

    protected override void Dispose(bool disposing) {
      if (_disposed) return;

      base.Dispose(disposing);

      if (valueHandle != null && valueHandle.IsAllocated) {
        valueHandle.Free();
      }

      _disposed = true;
    }

    public override GpuBuffer Get() {
      return new GpuBuffer(UnsafeNativeMethods.MpPacketGetGpuBuffer(ptr), false);
    }

    public override GpuBuffer Consume() {
      if (!OwnsResource()) {
        throw new InvalidOperationException("Not owns resouces to be consumed");
      }

      return new StatusOrGpuBuffer(UnsafeNativeMethods.MpPacketConsumeGpuBuffer(mpPtr)).ConsumeValueOrDie();
    }
  }
}