using Mediapipe;
using NUnit.Framework;

namespace Tests {
  public class TimestampTest {
    #region #isDisposed
    [Test]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet() {
      var timestamp = new Timestamp(1);

      Assert.False(timestamp.isDisposed);
    }

    [Test]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed() {
      var timestamp = new Timestamp(1);
      timestamp.Dispose();

      Assert.True(timestamp.isDisposed);
    }
    #endregion

    #region #Value
    [Test]
    public void Value_ShouldReturnValue() {
      var timestamp = new Timestamp(10);

      Assert.AreEqual(timestamp.Value(), 10);
    }
    #endregion

    #region #Seconds
    [Test]
    public void Seconds_ShouldReturnValueInSeconds() {
      var timestamp = new Timestamp(1_000_000);

      Assert.AreEqual(timestamp.Seconds(), 1d, 1e-9);
    }
    #endregion

    #region #Microseconds
    [Test]
    public void Microseconds_ShouldReturnValueInMicroseconds() {
      var timestamp = new Timestamp(1_000_000);

      Assert.AreEqual(timestamp.Microseconds(), 1_000_000);
    }
    #endregion

    #region #IsSpecialValue
    [Test]
    public void IsSpecialValue_ShouldReturnFalse_When_ValueIsInRange() {
      var timestamp = new Timestamp(1);

      Assert.False(timestamp.IsSpecialValue());
    }

    [Test]
    public void IsSpecialValue_ShouldReturnTrue_When_TimestampIsUnset() {
      var timestamp = Timestamp.Unset();

      Assert.True(timestamp.IsSpecialValue());
    }

    [Test]
    public void IsSpecialValue_ShouldReturnTrue_When_TimestampIsUnstarted() {
      var timestamp = Timestamp.Unstarted();

      Assert.True(timestamp.IsSpecialValue());
    }
    #endregion

    #region #IsRangeValue
    [Test]
    public void IsRangeValue_ShouldReturnTrue_When_ValueIsInRange() {
      var timestamp = new Timestamp(1);

      Assert.True(timestamp.IsRangeValue());
    }

    [Test]
    public void IsRangeValue_ShouldReturnFalse_When_TimestampIsPreStream() {
      var timestamp = Timestamp.PreStream();

      Assert.False(timestamp.IsRangeValue());
    }

    [Test]
    public void IsRangeValue_ShouldReturnFalse_When_TimestampIsPostStream() {
      var timestamp = Timestamp.PostStream();

      Assert.False(timestamp.IsRangeValue());
    }

    [Test]
    public void IsRangeValue_ShouldReturnTrue_When_TimestampIsMin() {
      var timestamp = Timestamp.Min();

      Assert.True(timestamp.IsRangeValue());
    }

    [Test]
    public void IsRangeValue_ShouldReturnTrue_When_TimestampIsMax() {
      var timestamp = Timestamp.Max();

      Assert.True(timestamp.IsRangeValue());
    }
    #endregion

    #region #IsAllowedInStream
    [Test]
    public void IsAllowedInStream_ShouldReturnTrue_When_ValueIsInRange() {
      var timestamp = new Timestamp(1);

      Assert.True(timestamp.IsAllowedInStream());
    }

    [Test]
    public void IsAllowedInStream_ShouldReturnFalse_When_TimestampIsOneOverPostStream() {
      var timestamp = Timestamp.OneOverPostStream();

      Assert.False(timestamp.IsAllowedInStream());
    }

    [Test]
    public void IsAllowedInStream_ShouldReturnFalse_When_TimestampIsDone() {
      var timestamp = Timestamp.Done();

      Assert.False(timestamp.IsAllowedInStream());
    }
    #endregion

    #region #DebugString
    [Test]
    public void DebugString_ShouldReturnDebugString() {
      var timestamp = new Timestamp(1);

      Assert.AreEqual(timestamp.DebugString(), "1");
    }

    [Test]
    public void DebugString_ShouldReturnDebugString_When_TimestampIsUnset() {
      var timestamp = Timestamp.Unset();

      Assert.AreEqual(timestamp.DebugString(), "Timestamp::Unset()");
    }
    #endregion

    #region #NextAllowedInStream
    [Test]
    public void NextAllowedInStream_ShouldReturnNextTimestamp_When_ValueIsInRange() {
      var timestamp = new Timestamp(1);
      var nextTimestamp = timestamp.NextAllowedInStream();

      Assert.AreEqual(nextTimestamp.Microseconds(), 2);
    }

    [Test]
    public void NextAllowedInStream_ShouldReturnOneOverPostStream_When_TimestampIsPostStream() {
      var timestamp = Timestamp.PostStream();
      var nextTimestamp = timestamp.NextAllowedInStream();

      Assert.AreEqual(nextTimestamp, Timestamp.OneOverPostStream());
    }
    #endregion

    #region #PreviousAllowedInStream
    [Test]
    public void PreviousAllowedInStream_ShouldReturnPreviousTimestamp_When_ValueIsInRange() {
      var timestamp = new Timestamp(1);
      var nextTimestamp = timestamp.PreviousAllowedInStream();

      Assert.AreEqual(nextTimestamp.Microseconds(), 0);
    }

    [Test]
    public void PreviousAllowedInStream_ShouldReturnUnstarted_When_TimestampIsPreStream() {
      var timestamp = Timestamp.PreStream();
      var nextTimestamp = timestamp.PreviousAllowedInStream();

      Assert.AreEqual(nextTimestamp, Timestamp.Unstarted());
    }
    #endregion

    #region #FromSeconds
    [Test]
    public void FromSeconds_ShouldReturnTimestamp() {
      var timestamp = Timestamp.FromSeconds(1d);

      Assert.AreEqual(timestamp.Microseconds(), 1_000_000);
    }
    #endregion
  }
}
