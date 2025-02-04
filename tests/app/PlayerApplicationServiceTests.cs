[TestClass]
public class PlayerApplicationServiceTests
{
    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetVeteranPlayers(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var appService = provider.GetRequiredService<IPlayerApplicationService>();

        var veterans = (await appService.GetVeteranPlayersAsync()).ToList();

        var expectedVeterans = new List<string> { "Cristiano Ronaldo", "Lionel Messi", "Ángel Di María" };

        foreach (var expected in expectedVeterans)
        {
            Assert.IsTrue(veterans.Any(p => p.Name == expected),
                $"Expected veteran player '{expected}' to be returned.");
        }
    }
}