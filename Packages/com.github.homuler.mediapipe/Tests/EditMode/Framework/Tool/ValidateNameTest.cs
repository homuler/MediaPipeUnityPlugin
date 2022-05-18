// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using NUnit.Framework;
using System;

namespace Mediapipe.Tests
{
  public class ValidateNameTest
  {
    #region .ValidateName
    [TestCase("humphrey")]
    [TestCase("humphrey_bogart")]
    [TestCase("humphrey_bogart_1899")]
    [TestCase("aa")]
    [TestCase("b1")]
    [TestCase("_1")]
    public void ValidateName_ShouldNotThrow_WhenNameIsValid(string name)
    {
      Assert.DoesNotThrow(() => { Tool.ValidateName(name); });
    }

    [TestCase("")]
    [TestCase("humphrey bogart")]
    [TestCase("humphreyBogart")]
    [TestCase("humphrey-bogart")]
    [TestCase("humphrey/bogart")]
    [TestCase("humphrey.bogart")]
    [TestCase("humphrey:bogart")]
    [TestCase("1ST")]
    [TestCase("7_ELEVEN")]
    [TestCase("401K")]
    [TestCase("0")]
    [TestCase("1")]
    [TestCase("11")]
    [TestCase("92091")]
    [TestCase("1st")]
    [TestCase("7_eleven")]
    [TestCase("401k")]
    [TestCase("\0contains_escapes\t")]
    public void ValidateName_ShouldThrow_WhenNameIsInvalid(string name)
    {
#pragma warning disable IDE0058
      Assert.Throws<ArgumentException>(() => { Tool.ValidateName(name); });
#pragma warning restore IDE0058
    }
    #endregion

    #region .ValidateNumber
    [TestCase("0")]
    [TestCase("10")]
    [TestCase("1234567890")]
    public void ValidateNumber_ShouldNotThrow_WhenNumberIsValid(string number)
    {
      Assert.DoesNotThrow(() => { Tool.ValidateNumber(number); });
    }

    [TestCase("01")]
    [TestCase("1a")]
    public void ValidateNumber_ShouldThrow_WhenNumberIsInvalid(string number)
    {
#pragma warning disable IDE0058
      Assert.Throws<ArgumentException>(() => { Tool.ValidateNumber(number); });
#pragma warning restore IDE0058
    }
    #endregion

    #region .ValidateTag
    [TestCase("MALE")]
    [TestCase("MALE_ACTOR")]
    [TestCase("ACTOR_1899")]
    [TestCase("AA")]
    [TestCase("B1")]
    [TestCase("_1")]
    public void ValidateTag_ShouldNotThrow_WhenTagIsValid(string tag)
    {
      Assert.DoesNotThrow(() => { Tool.ValidateTag(tag); });
    }

    [TestCase("")]
    [TestCase("MALE ACTOR")]
    [TestCase("MALEaCTOR")]
    [TestCase("MALE-ACTOR")]
    [TestCase("MALE/ACTOR")]
    [TestCase("MALE.ACTOR")]
    [TestCase("MALE:ACTOR")]
    [TestCase("0")]
    [TestCase("1")]
    [TestCase("11")]
    [TestCase("92091")]
    [TestCase("1ST")]
    [TestCase("7_ELEVEN")]
    [TestCase("401K")]
    [TestCase("\0CONTAINS_ESCAPES\t")]
    public void ValidateTag_ShouldThrow_WhenTagIsInvalid(string tag)
    {
#pragma warning disable IDE0058
      Assert.Throws<ArgumentException>(() => { Tool.ValidateTag(tag); });
#pragma warning restore IDE0058
    }
    #endregion

    #region .ParseTagAndName
    [TestCase("MALE:humphrey", "MALE", "humphrey")]
    [TestCase("ACTOR:humphrey_bogart", "ACTOR", "humphrey_bogart")]
    [TestCase("ACTOR_1899:humphrey_1899", "ACTOR_1899", "humphrey_1899")]
    [TestCase("humphrey_bogart", "", "humphrey_bogart")]
    [TestCase("ACTOR:humphrey", "ACTOR", "humphrey")]
    public void ParseTagAndName_ShouldParseInput_When_InputIsValid(string input, string expectedTag, string expectedName)
    {
      Tool.ParseTagAndName(input, out var tag, out var name);

      Assert.AreEqual(expectedTag, tag);
      Assert.AreEqual(expectedName, name);
    }

    [TestCase(":humphrey")]
    [TestCase("humphrey bogart")]
    [TestCase("actor:humphrey")]
    [TestCase("actor:humphrey")]
    [TestCase("ACTOR:HUMPHREY")]
    public void ParseTagAndName_ShouldThrow_When_InputIsInvalid(string input)
    {
      var tag = "UNTOUCHED";
      var name = "untouched";

#pragma warning disable IDE0058
      Assert.Throws<ArgumentException>(() => { Tool.ParseTagAndName(input, out tag, out name); });
      Assert.AreEqual("UNTOUCHED", tag);
      Assert.AreEqual("untouched", name);
#pragma warning restore IDE0058
    }

    [Test]
    public void ParseTagAndName_ShouldThrow_When_InputIncludesBadCharacters(
      [Values(' ', '-', '/', '.', ':')] char ch,
      [Values("MALE$0ACTOR:humphrey", "ACTOR:humphrey$0:bogart")] string str
    )
    {
      ParseTagAndName_ShouldThrow_When_InputIsInvalid(str.Replace("$0", $"{ch}"));
    }
    #endregion

    #region .ParseTagIndexName
    [TestCase("MALE:humphrey", "MALE", 0, "humphrey")]
    [TestCase("ACTOR:humphrey_bogart", "ACTOR", 0, "humphrey_bogart")]
    [TestCase("ACTOR_1899:humphrey_1899", "ACTOR_1899", 0, "humphrey_1899")]
    [TestCase("humphrey_bogart", "", -1, "humphrey_bogart")]
    [TestCase("ACTRESS:3:mieko_harada", "ACTRESS", 3, "mieko_harada")]
    [TestCase("ACTRESS:0:mieko_harada", "ACTRESS", 0, "mieko_harada")]
    [TestCase("A1:100:mieko1", "A1", 100, "mieko1")]
    [TestCase("A1:10000:mieko1", "A1", 10000, "mieko1")]
    public void ParseTagIndexName_ShouldParseInput_When_InputIsValid(string input, string expectedTag, int expectedIndex, string expectedName)
    {
      Tool.ParseTagIndexName(input, out var tag, out var index, out var name);

      Assert.AreEqual(expectedTag, tag);
      Assert.AreEqual(expectedIndex, index);
      Assert.AreEqual(expectedName, name);
    }

    [TestCase("")]
    [TestCase("A")]
    [TestCase("Aa")]
    [TestCase("aA")]
    [TestCase("1a")]
    [TestCase("1")]
    [TestCase(":name")]
    [TestCase("A:")]
    [TestCase("a:name")]
    [TestCase("Aa:name")]
    [TestCase("aA:name")]
    [TestCase("1A:name")]
    [TestCase("1:name")]
    [TestCase(":1:name")]
    [TestCase("A:1:")]
    [TestCase("A::name")]
    [TestCase("a:1:name")]
    [TestCase("Aa:1:name")]
    [TestCase("aA:1:name")]
    [TestCase("1A:1:name")]
    [TestCase("1:1:name")]
    [TestCase("A:1:N")]
    [TestCase("A:1:nN")]
    [TestCase("A:1:Nn")]
    [TestCase("A:1:1name")]
    [TestCase("A:1:1")]
    [TestCase("A:-0:name")]
    [TestCase("A:-1:name")]
    [TestCase("A:01:name")]
    [TestCase("A:00:name")]
    [TestCase("A:10001:a")]
    [TestCase("A:1:a:")]
    [TestCase(":A:1:a")]
    [TestCase("A:1:a:a")]
    [TestCase("A:1:a:A")]
    [TestCase("A:1:a:1")]
    public void ParseTagIndexName_ShouldThrow_When_InputIsInvalid(string input)
    {
      var tag = "UNTOUCHED";
      var index = -1;
      var name = "untouched";

#pragma warning disable IDE0058
      Assert.Throws<ArgumentException>(() => { Tool.ParseTagIndexName(input, out tag, out index, out name); });
      Assert.AreEqual("UNTOUCHED", tag);
      Assert.AreEqual(-1, index);
      Assert.AreEqual("untouched", name);
#pragma warning restore IDE0058
    }

    [Test]
    public void ParseTagIndexName_ShouldThrow_When_InputIncludesBadCharacters(
      [Values('!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '{', '}', '[', ']',
              '/', '=', '?', '+', '\\', '|', '-', ';', ':', '\'', '"', '<', '.', '>')] char ch,
      [Values("$0", "$0a", "a$0", "$0:a", "A$0:a", "$0A:a", "A:$0:a", "A:$01:a", "A:1$0:a", "A:1:a$0", "$0A:1:a")] string str
    )
    {
      ParseTagIndexName_ShouldThrow_When_InputIsInvalid(str.Replace("$0", $"{ch}"));
    }
    #endregion

    #region .ParseTagIndex
    [TestCase("", "", 0)]
    [TestCase("VIDEO:0", "VIDEO", 0)]
    [TestCase("VIDEO:1", "VIDEO", 1)]
    [TestCase("AUDIO:2", "AUDIO", 2)]
    [TestCase(":0", "", 0)]
    [TestCase(":1", "", 1)]
    [TestCase(":100", "", 100)]
    [TestCase("VIDEO:10000", "VIDEO", 10000)]
    public void ParseTagIndex_ShouldParseInput_When_InputIsValid(string input, string expectedTag, int expectedIndex)
    {
      Tool.ParseTagIndex(input, out var tag, out var index);

      Assert.AreEqual(expectedTag, tag);
      Assert.AreEqual(expectedIndex, index);
    }

    [TestCase("a")]
    [TestCase("Aa")]
    [TestCase("aA")]
    [TestCase("1A")]
    [TestCase("1")]
    [TestCase(":")]
    [TestCase(":a")]
    [TestCase(":A")]
    [TestCase(":-0")]
    [TestCase(":-1")]
    [TestCase(":01")]
    [TestCase(":00")]
    [TestCase("A:")]
    [TestCase("A:a")]
    [TestCase("A:A")]
    [TestCase("A:-0")]
    [TestCase("A:-1")]
    [TestCase("A:01")]
    [TestCase("A:00")]
    [TestCase("A:10001")]
    [TestCase("A:1:")]
    [TestCase(":A:1")]
    [TestCase("A:1:2")]
    [TestCase("A:A:1")]
    public void ParseTagIndex_ShouldThrow_When_InputIsInvalid(string input)
    {
      var tag = "UNTOUCHED";
      var index = -1;

#pragma warning disable IDE0058
      Assert.Throws<ArgumentException>(() => { Tool.ParseTagIndex(input, out tag, out index); });
      Assert.AreEqual("UNTOUCHED", tag);
      Assert.AreEqual(-1, index);
#pragma warning restore IDE0058
    }

    [Test]
    public void ParseTagIndex_ShouldThrow_When_InputIncludesBadCharacters(
      [Values('!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '{', '}', '[', ']',
              '/', '=', '?', '+', '\\', '|', '-', ';', ':', '\'', '"', '<', '.', '>')] char ch,
      [Values("$0", "$0A", "A$0", "$0:1", "A$0:1", "$0A:1", "A:1$0", "A:$01")] string str
    )
    {
      ParseTagIndex_ShouldThrow_When_InputIsInvalid(str.Replace("$0", $"{ch}"));
    }
    #endregion
  }
}
