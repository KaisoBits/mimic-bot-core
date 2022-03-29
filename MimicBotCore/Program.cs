using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MimicBotCore.DependencyInjection;
using MimicBotCore.Services;

Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(host =>
    {
        host.AddUserSecrets<Program>();
    })
    .ConfigureServices(services =>
    {
        services.AddDiscord();
        services.AddHostedService<MimicHostedService>();
    }).Build().Run();