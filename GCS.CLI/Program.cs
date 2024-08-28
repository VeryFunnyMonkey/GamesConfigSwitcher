using Cocona;
using GCS.CLI;
using GCS.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = CoconaApp.CreateBuilder();

builder.Logging.ClearProviders();
// TODO: Add file logging
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