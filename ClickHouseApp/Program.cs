using ClickHouseApp;
using ClickHouseApp.DbService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IClickHouseService, ClickHouseService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
