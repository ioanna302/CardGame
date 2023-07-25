using Console;
using ioanna.cardGame.Application;
using ioanna.cardGame.Application.Interfaces;
using ioanna.cardGame.Application.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();// Add services to the container.
services.AddApplicationServices();
services.AddSingleton<GameFlowService>();
services.AddSingleton<IInteractionService, InteractionService>();

var serviceProvider = services.BuildServiceProvider();


var application = serviceProvider.GetRequiredService<GameFlowService>();
await application.StartApplication();