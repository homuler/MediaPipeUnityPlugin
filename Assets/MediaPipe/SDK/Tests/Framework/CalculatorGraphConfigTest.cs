using Mediapipe;
using NUnit.Framework;
using System.Linq;

namespace Tests {
  public class CalculatorGraphConfigTest {
    private static string validConfigText = @"node {
  calculator: ""PassThroughCalculator""
  input_stream: ""in""
  output_stream: ""out1""
}
node {
  calculator: ""PassThroughCalculator""
  input_stream: ""out1""
  output_stream: ""out""
}
input_stream: ""in""
output_stream: ""out""
";

    private static string invalidConfigText = "Invalid";

    #region .ParseFromString
    [Test]
    public void ParseFromString_ShouldReturnCalculatorGraphConfig_When_ConfigIsValid() {
      var config = CalculatorGraphConfig.ParseFromString(validConfigText);

      Assert.AreEqual(config.DebugString(), validConfigText);
    }

    public void ParseFromString_ShouldThrowFormatException_When_ConfigIsInvalid() {
      Assert.Throws<System.FormatException>(() => { CalculatorGraphConfig.ParseFromString(invalidConfigText); });
    }
    #endregion

    #region #isDisposed
    [Test]
    public void isDisposed_ShouldReturnFalse_When_NotDisposedYet() {
      var config = CalculatorGraphConfig.ParseFromString(validConfigText);

      Assert.False(config.isDisposed);
    }

    [Test]
    public void isDisposed_ShouldReturnTrue_When_AlreadyDisposed() {
      var config = CalculatorGraphConfig.ParseFromString(validConfigText);
      config.Dispose();

      Assert.True(config.isDisposed);
    }
    #endregion

    #region #SerializeAsString
    [Test]
    public void SerializeAsString_ShouldReturnString() {
      var config = CalculatorGraphConfig.ParseFromString(validConfigText);

      Assert.AreEqual(config.SerializeAsString().Length, config.byteSizeLong);
    }
    #endregion
  }
}
