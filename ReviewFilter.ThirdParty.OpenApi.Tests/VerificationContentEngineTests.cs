using NUnit.Framework;
using ReviewFilter.ThirdParty.OpenApi.Engines;

namespace ReviewFilter.ThirdParty.OpenApi.Tests
{
    [TestFixture(Description = "Testing VerificationContentEngine class.")]
    public class VerificationContentEngineTests
    {
        [Test(Description = "Verifies that the content moderation system returns a valid response with a successful result for a given input.")]
        public async Task ModerateContentAsync_ReturnsModerationResponse()
        {
            // Arrange
            var verificationContentEngine = new VerificationContentEngine("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");

            // Act
            var verificationResult = await verificationContentEngine.Verify("Test content");

            // Assert
            Assert.That(verificationResult != null, Is.True);
            Assert.That(verificationResult!.Success, Is.True);
        }
    }
}
