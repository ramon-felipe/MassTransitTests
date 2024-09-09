using MassTransit;
using MassTransit.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using TestRabbit;
using TestRabbit.Contracts;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddHostedService<Worker>();

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBrokerSettings"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);


builder.Services.ConfigMassTransit();

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
    var sw = new Stopwatch();
    sw.Start();
    // var msg = new TestMessageEvent { Id = 1, MyProperty = DateTimeOffset.Now.ToString() };
    var msg = new CreateWorkItem { WorkItem = new MassTransit.Domain.WorkItem { Id = 1, Name = "test work item" } };

    app.Logger.LogInformation("Publish Init...");

    await bus.PublishAsync(msg);

    app.Logger.LogInformation("Publish end. time spent: {Time} (ms)", sw.ElapsedMilliseconds);

    sw.Stop();
    return msg;
})
.WithName("Publish")
// .WithOpenApi()
;

app.MapGet("/send", async ([FromServices] IEventBus bus, [FromServices] IBus bus2) =>
{
    var sw = new Stopwatch();
    sw.Start();
    var msg = new CreateWorkItem { WorkItem = new MassTransit.Domain.WorkItem { Id = 1, Name = "test work item" } };

    app.Logger.LogInformation("Send Init...");

    var endpoint = await bus2.GetSendEndpoint(new Uri("exchange:test-create-work-item"));
    await endpoint.Send(msg);

    //await bus.SendAsync(msg);
    app.Logger.LogInformation("Send end. time spent: {Time} (ms)", sw.ElapsedMilliseconds);

    sw.Stop();
    return msg;
})
.WithName("Send")
// .WithOpenApi()
;

app.Run();
