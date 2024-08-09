using Cocona;
using GCS.CLI;
using GCS.Core;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();

// Register IGameDataManager as a singleton service
builder.Services.AddSingleton<IGameDataManager>(provider =>
{
    // Set the path to the JSON file as needed
    var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gameData.json");
    return new GameDataManager(jsonFilePath);
});

var app = builder.Build();

app.AddCommands<ListCommand>();

app.Run();