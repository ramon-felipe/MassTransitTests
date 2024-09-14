using MassTransit;
using MassTransit.Domain;
using System.Diagnostics;
using System.Text.Json;
using TestRabbit.Contracts;

namespace TestRabbit.Consumers;

public sealed class TestMessageEventConsumer2 : IConsumer<TestMessageEvent>
{
    private readonly ILogger<TestMessageEventConsumer2> _logger;

    public TestMessageEventConsumer2(ILogger<TestMessageEventConsumer2> logger)
    {
        this._logger = logger;
    }

    public async Task Consume(ConsumeContext<TestMessageEvent> context)
    {
        var sw = new Stopwatch();
        sw.Start();
        this._logger.LogInformation("Message received by consumer {Consumer}! {Message}", this.GetType().Name, JsonSerializer.Serialize(context.Message));

        var msg = new CreateWorkItem { WorkItem = new WorkItem { Id = 2, Name = "t" } };
        await context.Send(new Uri("exchange:x-test-create-work-item"), msg);

        await Task.Delay(1000);
        this._logger.LogInformation("Processment by consumer {Consumer} concluded! {Message} - time spent: {Time} (ms)", this.GetType().Name, JsonSerializer.Serialize(context.Message), sw.ElapsedMilliseconds);       

        sw.Stop();
    }
}
