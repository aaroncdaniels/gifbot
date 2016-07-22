using System.Collections.Generic;
using gifbot.core;
using gifbot.Core;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;

namespace gifbot.tests
{
	public class ProcessTests
	{
		[Fact]
		public async void WhenRandomFunctionRandomStoreIsInvoked()
		{
			var fixture = new Fixture();
			fixture.Customize(new AutoMoqCustomization());

			var parser = fixture.Freeze<Mock<IParser>>();
			parser
				.Setup(p => p.ParseInput(It.IsAny<string>()))
				.Returns(new Input(Function.Random, new[] {"term"}, 1, Size.FixedHeight));

			var termFormatter = fixture.Freeze<Mock<ITermFormatter>>();
			termFormatter
				.Setup(tf => tf.Format(It.IsAny<IEnumerable<string>>()))
				.Returns(new Term("term", "term"));

			var gifStore = fixture.Freeze<Mock<IGifStore>>();

			var sut = fixture.Create<Process>();
			await sut.ProcessAsync("input");

			gifStore.Verify(gs => gs.RandomGifAsync("term"));
		}
	}
}