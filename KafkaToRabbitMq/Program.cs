using KafkaToRabbitMq;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.EventLog;
using KafkaLibNetCore;

IHost host = (IHost)Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.Sources.Clear();
        configuration
            .AddJsonFile("kafka_config.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<ICustomConsumer<string, string>, Consumer<string, string>>();
        services.AddSingleton<IKafkaReceiverService, KafkaReceiverService>();
        services.AddSingleton<IRabbitMq, RabbitMq>();
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
    .UseSerilog((hostContext, services, configuration) => {
        configuration.ReadFrom.Services(services);
        configuration.WriteTo.Console();
        configuration.WriteTo.File(
                 Environment.CurrentDirectory + @"\logs\KafkaToRabbitMq.log");
    })
    .UseWindowsService()
    .Build();

await host.RunAsync();

