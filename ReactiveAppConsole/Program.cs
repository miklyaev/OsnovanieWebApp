using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ReactiveAppConsole;

IHost host = (IHost)Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.Sources.Clear();
        
#if DEBUG
         configuration
            .AddJsonFile("appsettings.Debug.json", optional: true, reloadOnChange: true);
#else
         configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
#endif

    })
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();
        services.AddHostedService<Worker>();

        if (OperatingSystem.IsWindows())
        {
            services.Configure<EventLogSettings>(config =>
            {
                config.LogName = "ReactiveLog";
                config.SourceName = "ReactiveLogSvc";
            });
        }
    })
    .UseWindowsService()
    .Build();

await host.RunAsync();


