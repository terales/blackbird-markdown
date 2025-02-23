using MarkdownApp.Connections;
using Blackbird.Applications.Sdk.Common.Authentication;
using MarkdownTests.Base;

namespace MarkdownTests;

[TestClass]
public class ValidatorTests : TestBase
{
    [TestMethod]
    public async Task ValidatesCorrectConnection()
    {
        var validator = new ConnectionValidator();

        var result = await validator.ValidateConnection(Creds, CancellationToken.None);
        Assert.IsTrue(result.IsValid);
    }
}