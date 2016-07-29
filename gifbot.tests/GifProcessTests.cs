using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gifbot.core;
using gifbot.core.gifs;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;

namespace gifbot.tests
{
	public class GifProcessTests
	{
		[Fact]
		public async void WhenRandomFunctionRandomStoreIsInvokedLimitTimes()
		{
			var fixture = ConfiguredFixture();

			const int limit = 3;

			var parser = fixture.Freeze<Mock<IParser>>();
			parser
				.Setup(p => p.ParseInput(It.IsAny<string>()))
				.Returns(new Input(Function.Random, new[] { "term" }, limit, Size.FixedHeight));
			
			var gifStore = fixture.Freeze<Mock<IGifStore>>();
			gifStore
				.SetupSequence(gs => gs.RandomGifAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(new Gif()))
				.Returns(Task.FromResult(new Gif()))
				.Returns(Task.FromResult(new Gif()));

			var sut = fixture.Create<GifProcess>();
			await sut.ProcessAsync("input");

			gifStore.Verify(gs => gs.RandomGifAsync(It.IsAny<string>()), Times.Exactly(limit));
		}

		[Fact]
		public async void WhenTranslateFunctionTranslateStoreIsInvokedLimitTimes()
		{
			var fixture = ConfiguredFixture();

			const int limit = 3;

			var parser = fixture.Freeze<Mock<IParser>>();
			parser
				.Setup(p => p.ParseInput(It.IsAny<string>()))
				.Returns(new Input(Function.Translate, new[] { "term" }, limit, Size.FixedHeight));

			var gifStore = fixture.Freeze<Mock<IGifStore>>();
			gifStore
				.SetupSequence(gs => gs.TranslateGifAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(new Gif()))
				.Returns(Task.FromResult(new Gif()))
				.Returns(Task.FromResult(new Gif()));

			var sut = fixture.Create<GifProcess>();
			await sut.ProcessAsync("input");

			gifStore.Verify(gs => gs.TranslateGifAsync(It.IsAny<string>()), Times.Exactly(limit));
		}

		[Fact]
		public async void WhenSearchFunctionSearchStoreIsInvokedOnce()
		{
			var fixture = ConfiguredFixture();

			var parser = fixture.Freeze<Mock<IParser>>();
			parser
				.Setup(p => p.ParseInput(It.IsAny<string>()))
				.Returns(new Input(Function.Search, new[] { "term" }, 5, Size.FixedHeight));

			var gifStore = fixture.Freeze<Mock<IGifStore>>();

			var sut = fixture.Create<GifProcess>();
			await sut.ProcessAsync("input");

			gifStore.Verify(gs => gs.SearchGifsAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
		}

		[Fact]
		public async void WhenTrendingFunctionTrendingStoreIsInvokedOnce()
		{
			var fixture = ConfiguredFixture();

			var parser = fixture.Freeze<Mock<IParser>>();
			parser
				.Setup(p => p.ParseInput(It.IsAny<string>()))
				.Returns(new Input(Function.Trending, new[] { "term" }, 3, Size.FixedHeight));

			var gifStore = fixture.Freeze<Mock<IGifStore>>();

			var sut = fixture.Create<GifProcess>();
			await sut.ProcessAsync("input");

			gifStore.Verify(gs => gs.TrendingGifsAsync(It.IsAny<int>()), Times.Once);
		}

		[Fact]
		public async void WhenNoGifsAreReturnedThenOneResultIsReturnedIndicatingSo()
		{
			var fixture = ConfiguredFixture();

			var parser = fixture.Freeze<Mock<IParser>>();
			parser
				.Setup(p => p.ParseInput(It.IsAny<string>()))
				.Returns(new Input(Function.Search, new[] { "term" }, 3, Size.FixedHeight));

			var gifStore = fixture.Freeze<Mock<IGifStore>>();
			gifStore
				.Setup(gs => gs.SearchGifsAsync(It.IsAny<string>(), It.IsAny<int>()))
				.Returns(Task.FromResult(Enumerable.Empty<Gif>()));

			var sut = fixture.Create<GifProcess>();
			var result = (await sut.ProcessAsync("input")).ToList();

			Assert.Equal(1, result.Count);
			Assert.True(result[0].LastIndexOf("returned 0 results.", StringComparison.Ordinal) > 0);
		}

		[Fact]
		public async void WhenResultsReturnedThenTheyAreNumbered()
		{
			var fixture = ConfiguredFixture();

			const int limit = 3;

			var parser = fixture.Freeze<Mock<IParser>>();
			parser
				.Setup(p => p.ParseInput(It.IsAny<string>()))
				.Returns(new Input(Function.Translate, new[] { "term" }, limit, Size.FixedHeight));

			var gifStore = fixture.Freeze<Mock<IGifStore>>();
			gifStore
				.Setup(gs => gs.TranslateGifAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(new Gif()));

			var sut = fixture.Create<GifProcess>();
			var result = (await sut.ProcessAsync("input")).ToList();

			Assert.Equal(limit, result.Count);
			Assert.True(result[0].LastIndexOf($"1 of {limit}", StringComparison.Ordinal) > 0);
		}

		[Fact]
		public async void WhenLessThanLimitResultsAreReturnedThenOutputMessageIndicateActualCount()
		{
			var fixture = ConfiguredFixture();

			var parser = fixture.Freeze<Mock<IParser>>();
			parser
				.Setup(p => p.ParseInput(It.IsAny<string>()))
				.Returns(new Input(Function.Search, new[] { "term" }, 3, Size.FixedHeight));

			var gifStore = fixture.Freeze<Mock<IGifStore>>();
			gifStore
				.Setup(gs => gs.SearchGifsAsync(It.IsAny<string>(), It.IsAny<int>()))
				.Returns(Task.FromResult(new List<Gif> {new Gif(), new Gif() }.AsEnumerable()));

			var sut = fixture.Create<GifProcess>();
			var result = (await sut.ProcessAsync("input")).ToList();

			Assert.Equal(2, result.Count);
			Assert.True(result[0].LastIndexOf("1 of 2", StringComparison.Ordinal) > 0);
		}

		[Fact]
		public async Task WhenTranslateReturnsNullBeforeLimitIsReachedThenStoreIsNotInvokedForRemainder()
		{
			var fixture = ConfiguredFixture();

			var parser = fixture.Freeze<Mock<IParser>>();
			parser
				.Setup(p => p.ParseInput(It.IsAny<string>()))
				.Returns(new Input(Function.Translate, new[] { "term" }, 7, Size.FixedHeight));

			var gifStore = fixture.Freeze<Mock<IGifStore>>();
			gifStore
				.SetupSequence(gs => gs.TranslateGifAsync(It.IsAny<string>()))
				.Returns(Task.FromResult(new Gif()))
				.Returns(Task.FromResult<Gif>(null));

			var sut = fixture.Create<GifProcess>();
			await sut.ProcessAsync("input");

			gifStore.Verify(gs => gs.TranslateGifAsync(It.IsAny<string>()), Times.Exactly(2));
		}

		[Fact]
		public async Task WhenFunctionIsHelpThenHelpMessageIsReturned()
		{
			var fixture = ConfiguredFixture();

			var parser = fixture.Freeze<Mock<IParser>>();
			parser
				.Setup(p => p.ParseInput(It.IsAny<string>()))
				.Returns(new Input(Function.Help, new string[0], 1, Size.FixedHeight));

			var configuration = fixture.Freeze<Mock<IConfiguration>>();
			
			var sut = fixture.Create<GifProcess>();
			await sut.ProcessAsync("input");

			configuration.Verify(gs => gs.HelpMessage);
		}

		private static Fixture ConfiguredFixture()
		{
			var fixture = new Fixture();
			fixture.Customize(new AutoMoqCustomization());
			fixture.Customize(new TermCustomization());

			return fixture;
		}

		private class TermCustomization : ICustomization
		{
			public void Customize(IFixture fixture)
			{
				var termFormatter = fixture.Freeze<Mock<ITermFormatter>>();
				termFormatter
					.Setup(tf => tf.Format(It.IsAny<IEnumerable<string>>()))
					.Returns(new Term("term", "term"));
			}
		}
	}
}