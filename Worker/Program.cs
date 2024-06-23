
using MassTransit;
using MassTransit.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TestRabbit;
using TestRabbit.Consumers;
using TestRabbit.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBrokerSettings"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);


builder.Services.AddMassTransit(cfg =>
{
    cfg.SetKebabCaseEndpointNameFormatter();

    cfg.AddConsumers(typeof(Program).Assembly);

    // cfg.AddConsumer<TestConsumer>();

    cfg.UsingRabbitMq((ctx, cfg) =>
    {
        var settings = ctx.GetRequiredService<MessageBrokerSettings>();

        cfg.Host(settings.Host, settings.VirtualHost, h =>
        {
            h.Username(settings.User);
            h.Password(settings.Password);
        });

        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddTransient<IEventBus, EventBus>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/publish", async ([FromServices] IEventBus bus) =>
{
    var msg = new TestMessageEvent { Id = 1, MyProperty = DateTimeOffset.Now.ToString() };
    await bus.PublishAsync(msg);

    return msg;
})
.WithName("Publish")
// .WithOpenApi()
;

app.Run();

//builder.Services.AddHostedService<Worker>();

//builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBrokerSettings"));

//builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

//builder.Services.AddMassTransit(cfg =>
//{
//    cfg.SetKebabCaseEndpointNameFormatter();

//    cfg.AddConsumers(typeof(Program).Assembly);

//    cfg.UsingRabbitMq((ctx, cfg) =>
//    {
//        var settings = ctx.GetRequiredService<MessageBrokerSettings>();

//        cfg.Host(settings.Host, settings.VirtualHost, h =>
//        {
//            h.Username(settings.User);
//            h.Password(settings.Password);
//        });
//    });
//});

//builder.Services.AddTransient<IEventBus, EventBus>();

//var host = builder.Build();
//host.Run();
