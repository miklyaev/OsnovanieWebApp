using ClickHouseApp;
using ClickHouseApp.DbService;
using Quartz;

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
        services.AddSingleton<IClickHouseConsumer, ClickHouseConsumer>();
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
        });
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        })
        .Configure<ClickhouseOptions>(configuration?.GetSection("Clickhouse"));
        // services.AddHostedService<Worker>()
        // .Configure<ClickhouseOptions>(configuration?.GetSection("Clickhouse"));

    })
    .Build();

var schedulerFactory = host.Services.GetRequiredService<ISchedulerFactory>();
var scheduler = await schedulerFactory.GetScheduler();

// define the job and tie it to our HelloJob class
var job = JobBuilder.Create<ValueGenerator>()
    .WithIdentity("myJob", "group1")
    .Build();

// Trigger the job to run now, and then every 40 seconds
var trigger = TriggerBuilder.Create()
    .WithIdentity("myTrigger", "group1")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(10)
        .RepeatForever())
    .Build();

await scheduler.ScheduleJob(job, trigger);

var job2 = JobBuilder.Create<ClickHouseConsumer>()
    .WithIdentity("myJob2", "group2")
    .Build();

// Trigger the job to run now, and then every 40 seconds
var trigger2 = TriggerBuilder.Create()
    .WithIdentity("myTrigger2", "group2")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(20)
        .RepeatForever())
    .Build();

await scheduler.ScheduleJob(job2, trigger2);

await host.RunAsync();
