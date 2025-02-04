using Moq;

public enum TestMode
{
    Unit,
    Integration
}

public static class TestSetupFactory
{
    public static IServiceProvider CreateServiceProvider(TestMode mode)
    {
        var services = new ServiceCollection();

        switch (mode)
        {
            case TestMode.Unit:
                var sampleData = SeedData.CreateSamplePlayers().ToList();
                var mockRepo = new Mock<IPlayerRepository>();
                mockRepo.Setup(r => r.GetAllPlayersAsync()).ReturnsAsync(sampleData);
                services.AddSingleton(mockRepo.Object);
                break;

            case TestMode.Integration:
                var context = EfCoreSqliteSetup.CreateInMemoryContext();
                services.AddSingleton(context);
                services.AddSingleton<IPlayerRepository, EfPlayerRepository>();
                break;
        }

        services.AddTransient<IPlayerQueryService, PlayerQueryService>();
        services.AddTransient<IPlayerCommandService, PlayerCommandService>();
        services.AddTransient<IPlayerDomainService, PlayerDomainService>();
        services.AddTransient<IPlayerApplicationService, PlayerApplicationService>();

        return services.BuildServiceProvider();
    }
}