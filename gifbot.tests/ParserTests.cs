using System.Linq;
using gifbot.core;
using gifbot.core.gifs;
using Xunit;

namespace gifbot.tests
{
	public class ParserTests
	{
		[Fact]
		public void EmptyInputReturnsRandomFixedHeight()
		{
			var sut = new Parser();
			var result = sut.ParseInput(null);

			Assert.Equal(Function.Random, result.Function);
			Assert.Equal(Size.FixedHeight, result.Size);
		}

		[Theory]
		[InlineData("random", Function.Random, new string[] {}, 1, Size.FixedHeight)]
		[InlineData("translate", Function.Translate, new[] { "[blank]" }, 1, Size.FixedHeight)]
		[InlineData("translate star", Function.Translate, new[] {"star"}, 1, Size.FixedHeight)]
		[InlineData("translate star wars", Function.Translate, new[] { "star", "wars" }, 1, Size.FixedHeight)]
		[InlineData("--random", Function.Translate, new[] { "--random" }, 1, Size.FixedHeight)]
		[InlineData("RANDOM random", Function.Random, new[] { "random" }, 1, Size.FixedHeight)]
		[InlineData("-- random", Function.Translate, new[] { "random" }, 1, Size.FixedHeight)]
		[InlineData("search 2 foo bar", Function.Search, new[] { "foo", "bar" }, 2, Size.FixedHeight)]
		[InlineData("search -- 2 foo bar", Function.Search, new[] { "2", "foo", "bar" }, 1, Size.FixedHeight)]
		[InlineData("search -w 2 foo bar", Function.Search, new[] { "foo", "bar" }, 2, Size.FixedWidth)]
		[InlineData("search 2 -o -w", Function.Search, new[] { "-w" }, 2, Size.Original)]
		[InlineData("search 6 6 6 foo bar", Function.Search, new[] { "6", "6", "foo", "bar" }, 6, Size.FixedHeight)]
		[InlineData("trending", Function.Trending, new string[] { }, 1, Size.FixedHeight)]
		[InlineData("random 6 6 6 foo bar -o", Function.Random, new[] { "6", "6", "foo", "bar" }, 6, Size.Original)]
		[InlineData("random 6 -o 6 foo bar", Function.Random, new[] { "6", "foo", "bar" }, 6, Size.Original)]
		[InlineData("random foo  bar", Function.Random, new[] { "foo", "bar" }, 1, Size.FixedHeight)]
		[InlineData("trending 10 foo bar -w", Function.Trending, new[] { "foo", "bar" }, 10, Size.FixedWidth)]
		[InlineData("trending foo 10 bar -w    baz", Function.Trending, new[] { "foo", "bar", "baz" }, 10, Size.FixedWidth)]
		[InlineData("translate that's what I'm talking about!?!", Function.Translate, new[] { "that's", "what", "I'm", "talking", "about!?!" }, 1, Size.FixedHeight)]
		[InlineData("translate wt-h", Function.Translate, new[] { "wt-h" }, 1, Size.FixedHeight)]
		[InlineData("random -- -o", Function.Random, new[] { "-o" }, 1, Size.FixedHeight)]
		[InlineData("random -o --", Function.Random, new[] { "--" }, 1, Size.Original)]
		[InlineData("translate hello+world!", Function.Translate, new[] { "hello+world!" }, 1, Size.FixedHeight)]
		[InlineData("translate hello%20world!", Function.Translate, new[] { "hello%20world!" }, 1, Size.FixedHeight)]
		[InlineData("trending 100", Function.Trending, new string[] { }, 10, Size.FixedHeight)]
		[InlineData("translate foo 2", Function.Translate, new[] { "foo" }, 2, Size.FixedHeight)]
		[InlineData("help", Function.Help, new string[] { }, 1, Size.FixedHeight)]
		[InlineData("help other stuff", Function.Help, new string[] { }, 1, Size.FixedHeight)]
		public void InputParsesCorrectly(
			string input, 
			Function expectedFunction,
			string[] expectedTerms,
			int expectedLimit,
			Size expectedSize)
		{
			var sut = new Parser();
			var result = sut.ParseInput(input);

			Assert.Equal(expectedFunction, result.Function);
			Assert.Equal(expectedTerms, result.Terms);
			Assert.Equal(expectedLimit, result.Limit);
			Assert.Equal(expectedSize, result.Size);
		}
	}
}