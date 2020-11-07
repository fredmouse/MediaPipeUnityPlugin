using System;
using MpStatusOrPoller = System.IntPtr;

namespace Mediapipe {
  public class StatusOrPoller<T> : StatusOr<OutputStreamPoller<T>> {
    public StatusOrPoller(MpStatusOrPoller ptr) : base(ptr) {}

    protected override void DisposeUnmanaged() {
      if (OwnsResource()) {
        UnsafeNativeMethods.mp_StatusOrPoller__delete(ptr);
      }
      base.DisposeUnmanaged();
    }

    public override bool ok {
      get { return SafeNativeMethods.mp_StatusOrPoller__ok(mpPtr); }
    }

    public override Status status {
      get {
        UnsafeNativeMethods.mp_StatusOrPoller__status(mpPtr, out var statusPtr);

        GC.KeepAlive(this);
        return new Status(statusPtr);
      }
    }

    public override OutputStreamPoller<T> ConsumeValueOrDie() {
      EnsureOk();
      UnsafeNativeMethods.mp_StatusOrPoller__ConsumeValueOrDie(mpPtr, out var pollerPtr);
      Dispose();

      GC.KeepAlive(this);
      return new OutputStreamPoller<T>(pollerPtr);
    }
  }
}