using PhotoshopApp.Connections;
using Blackbird.Applications.Sdk.Common.Authentication;
using PhotoshopTests.Base;

namespace PhotoshopTests;

[TestClass]
public class ValidatorTests : TestBase
{
    [TestMethod]
    public async Task ValidatesCorrectConnection()
    {
        var validator = new ConnectionValidator();

        var result = await validator.ValidateConnection(Creds, CancellationToken.None);
        Console.WriteLine(result.Message);
        Assert.IsTrue(result.IsValid);
    }
}