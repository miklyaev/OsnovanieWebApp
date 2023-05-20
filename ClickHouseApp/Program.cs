using ClickHouseApp;
using ClickHouseApp.DbService;


IConfigurationRoot? configuration = null;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.Sources.Clear();
#if DEBUG
        config
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
#else
        config
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);        
#endif

        configuration = config.Build();

    })
    .ConfigureServices(services =>
    {        
        services.AddSingleton<IClickHouseService, ClickHouseService>();
        services.AddSingleton<IValueGenerator, ValueGenerator>();
        services.AddHostedService<Worker>()
         .Configure<ClickhouseOptions>(configuration?.GetSection("Clickhouse"));

    })
    .Build();

await host.RunAsync();
