// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System;

namespace Mediapipe.Tests
{
  public class NameUtilTest
  {
    [TestCase("{}", "base", "base")]
    [TestCase(@"{""node"":[{""name"":""a""}]}", "base", "base")]
    [TestCase(@"{""node"":[{},{}]}", "", "")]
    [TestCase(@"{""node"":[{""name"":""base""},{""name"":""base_02""}]}", "base", "base_03")]
    public void GetUnusedNodeName_ShouldReturnUniqueName(string configJson, string nameBase, string uniqueName)
    {
      var config = CalculatorGraphConfig.Parser.ParseJson(configJson);
      Assert.AreEqual(uniqueName, Tool.GetUnusedNodeName(config, nameBase));
    }

    [TestCase("{}", "base", "base")]
    [TestCase(@"{""node"":[{""input_side_packet"":[""a""]}]}", "base", "base")]
    [TestCase(@"{""node"":[{},{""input_side_packet"":[]}]}", "", "")]
    [TestCase(@"{""node"":[{""input_side_packet"":[""base""]},{""input_side_packet"":[""TAG:base_02""]}]}", "base", "base_03")]
    public void GetUnusedSidePacketName_ShouldReturnUniqueName(string configJson, string nameBase, string uniqueName)
    {
      var config = CalculatorGraphConfig.Parser.ParseJson(configJson);
      Assert.AreEqual(uniqueName, Tool.GetUnusedSidePacketName(config, nameBase));
    }

    [TestCase(@"{""node"":[{""name"":""x""}]}", 0, "x")]
    [TestCase(@"{""node"":[{""name"":""x""},{""name"":""x""},{""name"":""y""},{""name"":""x""}]}", 0, "x_1")]
    [TestCase(@"{""node"":[{""name"":""x""},{""name"":""x""},{""name"":""y""},{""name"":""x""}]}", 1, "x_2")]
    [TestCase(@"{""node"":[{""name"":""x""},{""name"":""x""},{""name"":""y""},{""name"":""x""}]}", 2, "y")]
    [TestCase(@"{""node"":[{""name"":""x""},{""name"":""x""},{""name"":""y""},{""name"":""x""}]}", 3, "x_3")]
    [TestCase(@"{""node"":[{""calculator"":""x""},{""name"":""x""}]}", 0, "x_1")]
    [TestCase(@"{""node"":[{""calculator"":""x""},{""name"":""x""}]}", 1, "x_2")]
    [TestCase(@"{""node"":[{""name"":""x""},{""calculator"":""x""}]}", 0, "x_1")]
    [TestCase(@"{""node"":[{""name"":""x""},{""calculator"":""x""}]}", 1, "x_2")]
    public void CanonicalNodeName_ShouldReturnCanonicalNodeName_When_NodeIdIsValid(string configJson, int nodeId, string name)
    {
      var config = CalculatorGraphConfig.Parser.ParseJson(configJson);
      Assert.AreEqual(name, Tool.CanonicalNodeName(config, nodeId));
    }

    [Test]
    public void CanonicalNodeName_ShouldThrow_When_NodeIdIsNegative()
    {
      var config = CalculatorGraphConfig.Parser.ParseJson(@"{""node"":[{""name"":""name""}]}");
#pragma warning disable IDE0058
      Assert.Throws<ArgumentOutOfRangeException>(() => { Tool.CanonicalNodeName(config, -1); });
#pragma warning restore IDE0058
    }

    [Test]
    public void CanonicalNodeName_ShouldThrow_When_NodeIdIsInvalid()
    {
      var config = CalculatorGraphConfig.Parser.ParseJson(@"{""node"":[{""name"":""name""}]}");
#pragma warning disable IDE0058
      Assert.Throws<ArgumentOutOfRangeException>(() => { Tool.CanonicalNodeName(config, 1); });
#pragma warning restore IDE0058
    }

    [Test]
    public void CanonicalNodeName_ShouldThrow_When_NodeIsEmpty()
    {
      var config = CalculatorGraphConfig.Parser.ParseJson("{}");
#pragma warning disable IDE0058
      Assert.Throws<ArgumentOutOfRangeException>(() => { Tool.CanonicalNodeName(config, 0); });
#pragma warning restore IDE0058
    }

    [TestCase("stream", "stream")]
    [TestCase("TAG:x", "x")]
    [TestCase("TAG:1:x", "x")]
    public void ParseNameFromStream_ShouldReturnName_When_InputIsValid(string stream, string name)
    {
      Assert.AreEqual(name, Tool.ParseNameFromStream(stream));
    }

    [TestCase(":stream")]
    [TestCase("TAG::stream")]
    [TestCase("TAG:1:")]
    public void ParseNameFromStream_ShouldThrow_When_InputIsInvalid(string stream)
    {
#pragma warning disable IDE0058
      Assert.Throws<ArgumentException>(() => { Tool.ParseNameFromStream(stream); });
#pragma warning restore IDE0058
    }

    [TestCase("", "", 0)]
    [TestCase("TAG", "TAG", 0)]
    [TestCase(":1", "", 1)]
    [TestCase("TAG:1", "TAG", 1)]
    public void ParseTagIndex_ShouldReturnTagIndexPair_When_InputIsValid(string tagIndex, string tag, int index)
    {
      var output = Tool.ParseTagIndex(tagIndex);

      Assert.AreEqual(tag, output.Item1);
      Assert.AreEqual(index, output.Item2);
    }

    [TestCase("tag")]
    [TestCase(":")]
    [TestCase("TAG:")]
    [TestCase("1")]
    public void ParseTagIndex_ShouldThrow_When_InputIsInvalid(string tagIndex)
    {
#pragma warning disable IDE0058
      Assert.Throws<ArgumentException>(() => { Tool.ParseTagIndex(tagIndex); });
#pragma warning restore IDE0058
    }

    [TestCase("stream", "", -1)]
    [TestCase("TAG:x", "TAG", 0)]
    [TestCase("TAG:1:x", "TAG", 1)]
    public void ParseTagIndexFromStream_ShouldReturnTagIndexPair_When_InputIsValid(string stream, string tag, int index)
    {
      var output = Tool.ParseTagIndexFromStream(stream);

      Assert.AreEqual(tag, output.Item1);
      Assert.AreEqual(index, output.Item2);
    }

    [TestCase(":stream")]
    [TestCase("TAG::stream")]
    [TestCase("TAG:1:")]
    public void ParseTagIndexFromStream_ShouldThrow_When_InputIsInvalid(string stream)
    {
#pragma warning disable IDE0058
      Assert.Throws<ArgumentException>(() => { Tool.ParseTagIndexFromStream(stream); });
#pragma warning restore IDE0058
    }

    [TestCase("", -1, "")]
    [TestCase("", 1, "")]
    [TestCase("TAG", -1, "TAG")]
    [TestCase("TAG", 1, "TAG:1")]
    public void CatTag_ShouldReturnTag(string tag, int index, string output)
    {
      Assert.AreEqual(output, Tool.CatTag(tag, index));
    }

    [TestCase("", -1, "x", "x")]
    [TestCase("", 1, "x", "x")]
    [TestCase("TAG", -1, "x", "TAG:x")]
    [TestCase("TAG", 1, "x", "TAG:1:x")]
    public void CatStream_ShouldReturnStream(string tag, int index, string name, string output)
    {
      Assert.AreEqual(output, Tool.CatStream((tag, index), name));
    }
  }
}
