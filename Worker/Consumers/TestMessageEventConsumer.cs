using MassTransit;
using System.Diagnostics;
using System.Text.Json;
using TestRabbit.Contracts;

namespace TestRabbit.Consumers;
public sealed class TestMessageEventConsumer : IConsumer<TestMessageEvent>
{
    private readonly ILogger<TestMessageEventConsumer> _logger;

    public TestMessageEventConsumer(ILogger<TestMessageEventConsumer> logger)
    {
        this._logger = logger;
    }

    public async Task Consume(ConsumeContext<TestMessageEvent> context)
    {
        var sw = new Stopwatch();
        sw.Start();
        this._logger.LogInformation("Message received by {Consumer}! {Message}", this.GetType().Name, JsonSerializer.Serialize(context.Message));

        await Task.Delay(3000);
        this._logger.LogInformation("Processment by {Consumer} concluded! {Message} - time spent: {Time} (ms)", this.GetType().Name, JsonSerializer.Serialize(context.Message), sw.ElapsedMilliseconds);
        
        sw.Stop();
    }
}
