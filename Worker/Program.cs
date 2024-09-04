using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Text.Json;
using TestRabbit;
using TestRabbit.Contracts;
using TestRabbit.EventBus;

var builder = WebApplication.CreateBuilder(args);

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
    var msg = new TestMessageEvent { Id = 1, MyProperty = DateTimeOffset.Now.ToString() };

    app.Logger.LogInformation("Publish Init...");
    await bus.PublishAsync(msg);
    app.Logger.LogInformation("Publish end. time spent: {Time} (ms)", sw.ElapsedMilliseconds);

    sw.Stop();
    return msg;
})
.WithName("Publish")
// .WithOpenApi()
;

app.Run();
