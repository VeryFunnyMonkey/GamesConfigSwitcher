using Cocona;
using GCS.CLI;
using GCS.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = CoconaApp.CreateBuilder();

// Path to the configuration file
var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

// Create the configuration file with default values if it doesn't exist
if (!File.Exists(configFilePath))
{
    CreateDefaultConfigFile(configFilePath);
}

// Build Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Read logging settings from appsettings.json
var loggingEnabled = configuration.GetValue<bool>("Logging:Enabled");
var minimumLevel = configuration.GetValue<Serilog.Events.LogEventLevel>("Logging:minimumLevel");
var filePath = configuration.GetValue<string>("Logging:FilePath");

builder.Logging.ClearProviders();

if (loggingEnabled)
{
    // Configure Serilog
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Is(minimumLevel)
        .WriteTo.File(filePath, rollingInterval: RollingInterval.Day)
        .CreateLogger();
    builder.Logging.AddSerilog(Log.Logger);
}

builder.Services.AddSingleton<ILoggingHandler, ConsoleLogger>();

// Register IGameDataManager as a singleton service
builder.Services.AddSingleton<IGameDataManager>(provider =>
{
    // Set the path to the JSON file as needed
    var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gameData.json");

    var logger = provider.GetRequiredService<ILoggingHandler>();
    return new GameDataManager(jsonFilePath, logger);
});

var app = builder.Build();

app.AddCommands<ListCommand>();
app.AddSubCommand("add", x =>
{
    x.AddCommands<AddGameCommand>();
    x.AddCommands<AddProfileCommand>();
});

app.AddSubCommand("delete", x =>
{
    x.AddCommands<DeleteGameCommand>();
    x.AddCommands<DeleteProfileCommand>();
});

app.AddSubCommand("edit", x =>
{
    x.AddCommands<EditGameCommand>();
    x.AddCommands<EditProfileCommand>();
});

app.AddCommands<UseCommand>();
app.AddCommands<UseAllCommand>();

app.Run();

void CreateDefaultConfigFile(string path)
{
    var defaultConfig = @"
{
  ""Logging"": {
    ""Enabled"": true,
    ""MinimumLevel"": ""Information"",
    ""FilePath"": ""logs/log-.log""
  }
}";
    File.WriteAllText(path, defaultConfig);
}