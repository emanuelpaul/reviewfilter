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
            var moderationClient = new OpenAIModerationClient("api-key");

            // Act
            var result = await moderationClient.ModerateContentAsync("Test content");

            // Assert
            Assert.That(result != null, Is.True);
            Assert.That(result!.results![0].categories?.hate, Is.False);
            Assert.That(result.results[0].flagged, Is.False);
        }
    }
}
