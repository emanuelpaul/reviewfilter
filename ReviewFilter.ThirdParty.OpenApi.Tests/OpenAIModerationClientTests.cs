using NUnit.Framework;

namespace ReviewFilter.ThirdParty.OpenApi.Tests
{
    [TestFixture(Description = "Testing OpenAIModeration class.")]
    public class OpenAIModerationClientTests
    {
        [Test(Description = "")]
        public async Task ModerateContentAsync_ReturnsCorrectModerationResponse()
        {
            // Arrange
            var moderationClient = new OpenAIModerationClient("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");

            // Act
            var result = await moderationClient.ModerateContentAsync("Test content");

            // Assert
            Assert.That(result != null, Is.True);
            Assert.That(result!.results![0].categories?.hate, Is.False);
            Assert.That(result.results[0].flagged, Is.False);
        }
    }
}
