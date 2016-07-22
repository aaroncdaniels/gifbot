using gifbot.core;
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
		
	}
}