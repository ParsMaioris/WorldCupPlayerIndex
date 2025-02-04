[TestClass]
public class PlayerDomainServiceTests
{
    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetVeteranPlayers_AllPlayersAreVeterans(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var domainService = provider.GetRequiredService<IPlayerDomainService>();

        var veterans = (await domainService.GetVeteranPlayersAsync()).ToList();

        Assert.IsTrue(veterans.All(p => p.Age >= 35),
            "One or more players in the result do not meet the veteran criteria (Age >= 35).");
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetVeteranPlayers_ContainsExpectedVeteranNames(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var domainService = provider.GetRequiredService<IPlayerDomainService>();

        var veterans = (await domainService.GetVeteranPlayersAsync()).ToList();

        var expectedVeterans = new List<string>
        {
            "Cristiano Ronaldo",
            "Lionel Messi",
            "Ángel Di María"
        };

        foreach (var expected in expectedVeterans)
        {
            Assert.IsTrue(veterans.Any(p => p.Name == expected),
                $"Expected veteran '{expected}' was not found in the returned list.");
        }
    }
}