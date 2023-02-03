using AutoWrapper;
using OsnovanieService;
using OsnovanieWebApp.Model;
using Serilog;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("custom.json", false, true);
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        //.WriteTo.File(
        //            GetLogFilePath(),
        //            restrictedToMinimumLevel: LogEventLevel.Information, //уровень логирования
        //            outputTemplate: hostingContext.Configuration["outputTemplate"],//выходной шаблон сообщения       
        //            fileSizeLimitBytes: Convert.ToInt32(hostingContext.Configuration["fileSizeLimitBytes"]), //размер лог файлов в байтах
        //            rollOnFileSizeLimit: Convert.ToBoolean(hostingContext.Configuration["rollOnFileSizeLimit"]),    //делать ролинг файлов
        //            retainedFileCountLimit: Convert.ToInt16(hostingContext.Configuration["retainedFileCountLimit"])) //кол-во файлов, после которого делается ролинг
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
        .Enrich.WithProperty("Environment", hostingContext.HostingEnvironment);

#if DEBUG
    // Used to filter out potentially bad data due debugging.
    // Very useful when doing Seq dashboards and want to remove logs under debugging session.
    loggerConfiguration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
#endif
});

builder.Services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
builder.Services.AddScoped<IMainService, MainService>();
//--------------------------------
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseApiResponseAndExceptionWrapper<MapResponseObject>(); //кастомизания вывода
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.Run();
