using KafkaToRabbitMqConsole;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Logging;

IHost host = (IHost)Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.Sources.Clear();
        configuration
            .AddJsonFile("kafka_config.json", optional: true, reloadOnChange: true);
    })
    .UseSerilog((hostContext, services, configuration) => {
        configuration.ReadFrom.Services(services);
        configuration.WriteTo.Console();
        configuration.WriteTo.File(
                 Environment.CurrentDirectory + @"logs\KafkaToRabbitMq.log");
    })    
    .ConfigureServices((context, services) =>
    {

        //IConfiguration config = context.Configuration;
        //services.AddSingleton(logProviders);
        //services.AddSingleton<ICustomConsumer<string, string>, Consumer<string, string>>();
        //services.AddSingleton<IReceiverService, ReceiverService>();
        services.AddHostedService<Worker>();

        if (OperatingSystem.IsWindows())
        {
            services.Configure<EventLogSettings>(config =>
            {
                config.LogName = "KafkaToRabbitMqService";
                config.SourceName = "KafkaToRabbitMqService";
            });
        }
    })
    .UseWindowsService()
    .Build();

await host.RunAsync();
