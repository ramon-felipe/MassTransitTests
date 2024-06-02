// See https://aka.ms/new-console-template for more information

using MassTransit;
using Publisher;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();

    cfg.AddConsumers(typeof(Program).Assembly);

    cfg.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();