using MassTransit;
using System.Diagnostics;
using System.Text.Json;
using TestRabbit.Contracts;

namespace TestRabbit.Consumers;
public sealed class TestConsumer : IConsumer<TestMessageEvent>
{
    private readonly ILogger<TestConsumer> _logger;

    public TestConsumer(ILogger<TestConsumer> logger)
    {
        this._logger = logger;
    }

    public async Task Consume(ConsumeContext<TestMessageEvent> context)
    {
        var sw = new Stopwatch();
        sw.Start();
        this._logger.LogInformation("Message received! {Message}", JsonSerializer.Serialize(context.Message));

        await Task.Delay(3000);
        this._logger.LogInformation("Processment concluded! {Message} - time spent: {Time} (ms)", JsonSerializer.Serialize(context.Message), sw.ElapsedMilliseconds);
        
        sw.Stop();
    }
}
