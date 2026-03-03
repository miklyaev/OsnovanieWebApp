using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SignalRApp;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: true);
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
        .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);

#if DEBUG
    // Used to filter out potentially bad data due debugging.
    // Very useful when doing Seq dashboards and want to remove logs under debugging session.
    loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
});
// подключем сервисы SignalR
//глобальная настройка хабов:
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
});


builder.Services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();
builder.Services.AddSingleton<IChat, ChatHub>();
builder.Services.AddHostedService<ReceiverService>();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();


//app.MapHub<ChatHub>("/chat");   // ChatHub будет обрабатывать запросы по пути /chat
//вариант с настройками
app.MapHub<ChatHub>("/chat",
    options => {
        options.ApplicationMaxBufferSize = 128;
        options.TransportMaxBufferSize = 128;
        options.LongPolling.PollTimeout = TimeSpan.FromMinutes(1);
        options.Transports = HttpTransportType.LongPolling | HttpTransportType.WebSockets;
    });

app.Run();
