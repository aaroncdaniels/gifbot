using gifbot.core;
using gifbot.core.gifs;
using Xunit;

namespace gifbot.tests
{
	public class TermFormatterTests
	{
		[Theory]
		[InlineData(new string[] { }, "", "")]
		[InlineData(new[] { "term1" }, "term1", "term1")]
		[InlineData(new[] { "term1", "term2" }, "term1+term2", "term1 term2")]
		[InlineData(new[] { "term1", "term2", "term3&" }, "term1+term2+term3%26", "term1 term2 term3&")]
		[InlineData(new[] { "hello+world" }, "hello+world", "hello world")]
		[InlineData(new[] { "hello%20world" }, "hello+world", "hello world")]
		[InlineData(new[] { "good+bye%20cruel+world%20forever" }, "good+bye+cruel+world+forever", "good bye cruel world forever")]
		public void TermsFormatCorrectly(string[] terms, string expectedUrlTerm, string expectedReadableTerm)
		{
			var sut = new TermFormatter();

			var actual = sut.Format(terms);

			Assert.Equal(expectedUrlTerm, actual.UrlTerm);
			Assert.Equal(expectedReadableTerm, actual.ReadableTerm);
		}
	}
}