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
    .ConfigureServices((h, s) =>
    {
        s.AddDiscord();
        s.AddHostedService<MimicHostedService>();
    })
    .Build().Run();