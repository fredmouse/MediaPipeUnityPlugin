using Mediapipe;
using NUnit.Framework;
using System;

namespace Tests {
  public class ImageFramePacketTest {
    #region Constructor
    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithNoArguments() {
      var packet = new ImageFramePacket();
      var statusOrImageFrame = packet.Consume();

      Assert.AreEqual(packet.ValidateAsType().code, Status.StatusCode.Internal);
      Assert.AreEqual(statusOrImageFrame.status.code, Status.StatusCode.Internal);
      Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValue() {
      var srcImageFrame = new ImageFrame();
      var packet = new ImageFramePacket(srcImageFrame);

      Assert.True(srcImageFrame.isDisposed);
      Assert.True(packet.ValidateAsType().ok);
      Assert.AreEqual(packet.Timestamp(), Timestamp.Unset());

      var statusOrImageFrame = packet.Consume();
      Assert.True(statusOrImageFrame.ok);

      var imageFrame = statusOrImageFrame.ConsumeValueOrDie();
      Assert.AreEqual(imageFrame.Format(), ImageFormat.Format.UNKNOWN);
    }

    [Test]
    public void Ctor_ShouldInstantiatePacket_When_CalledWithValueAndTimestamp() {
      var srcImageFrame = new ImageFrame();
      var timestamp = new Timestamp(1);
      var packet = new ImageFramePacket(srcImageFrame, timestamp);

      Assert.True(srcImageFrame.isDisposed);
      Assert.True(packet.ValidateAsType().ok);

      var statusOrImageFrame = packet.Consume();
      Assert.True(statusOrImageFrame.ok);

      var imageFrame = statusOrImageFrame.ConsumeValueOrDie();
      Assert.AreEqual(imageFrame.Format(), ImageFormat.Format.UNKNOWN);
      Assert.AreEqual(packet.Timestamp(), timestamp);
    }
    #endregion

    #region #isDisposed
    [Test]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet() {
      var packet = new ImageFramePacket();

      Assert.False(packet.isDisposed);
    }

    [Test]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed() {
      var packet = new ImageFramePacket();
      packet.Dispose();

      Assert.True(packet.isDisposed);
    }
    #endregion

    #region #Get
    [Test]
    public void Get_ShouldThrowNotSupportedException() {
      var packet = new ImageFramePacket(new ImageFrame(ImageFormat.Format.SBGRA, 10, 10));
      
      Assert.Throws<NotSupportedException>(() => { packet.Get(); });
    }
    #endregion

    #region #Consume
    [Test]
    public void Consume_ShouldReturnImageFrame() {
      var packet = new ImageFramePacket(new ImageFrame(ImageFormat.Format.SBGRA, 10, 10));
      var statusOrImageFrame = packet.Consume();
      Assert.True(statusOrImageFrame.ok);

      var imageFrame = statusOrImageFrame.ConsumeValueOrDie();
      Assert.AreEqual(imageFrame.Format(), ImageFormat.Format.SBGRA);
      Assert.AreEqual(imageFrame.Width(), 10);
      Assert.AreEqual(imageFrame.Height(), 10);
    }
    #endregion

    #region #DebugTypeName
    [Test]
    public void DebugTypeName_ShouldReturnFloat_When_ValueIsSet() {
      var packet = new ImageFramePacket(new ImageFrame());

      Assert.AreEqual(packet.DebugTypeName(), "mediapipe::ImageFrame");
    }
    #endregion
  }
}
