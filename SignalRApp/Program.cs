using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using SignalRApp;

var builder = WebApplication.CreateBuilder(args);

// подключем сервисы SignalR
//Глобальная настройка хабов:
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
});

builder.Services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();

//Настройка только для хаба ChatHub:
//builder.Services.AddSignalR().AddHubOptions<ChatHub>(options =>
//{
//    options.EnableDetailedErrors = true;
//    options.KeepAliveInterval = System.TimeSpan.FromMinutes(1);
//});

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
