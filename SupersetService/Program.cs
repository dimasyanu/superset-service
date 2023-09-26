using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SupersetService;
using SupersetService.Extensions;
using SupersetService.Models;
using SupersetService.Models.Csv;
using System.Reflection;

Serilog.ILogger logger = Log.Logger;

// Build Configuration
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
string currDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? Directory.GetCurrentDirectory();
var config = new ConfigurationBuilder()
    .SetBasePath(currDir)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddJsonFile(Path.Combine(currDir, "appsettings.json"), optional: false, reloadOnChange: true)
    //.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .Build();

// Build Automapper
var mapperConfig = new MapperConfiguration(cfg => {
    cfg.CreateMap<CsvAllSummary, AllSummary>();
    cfg.CreateMap<CsvBrokerSummary, BrokerSummary>();
    cfg.CreateMap<CsvIndexSummary, IndexSummary>();
    cfg.CreateMap<CsvRecapitulation, Recapitulation>();
    cfg.CreateMap<CsvStockSummary, StockSummary>();
});

// Build Host
var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((builder, context) => {
        // Logging service
        var logPath = config.AppSetting("Logging:Path") ?? "Logs";
        logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(
                //formatter: new JsonFormatter(),
                path: logPath + "/.log",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                rollingInterval: RollingInterval.Day
            )
            .CreateLogger();
    })
    .ConfigureServices(services => {
        services.AddHostedService<MainWorker>();
        services.AddSingleton(logger);
        services.AddSingleton(mapperConfig.CreateMapper());
        services.AddTransient<ImportRepository>();

        var connStrings = config.GetConnectionString(name: "DbConnection");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("DbConnection")), ServiceLifetime.Transient);
    })
    .Build();

await host.RunAsync();
