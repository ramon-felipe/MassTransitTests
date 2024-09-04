using MassTransit;
using System.Diagnostics;
using System.Text.Json;
using TestRabbit.Contracts;

namespace TestRabbit.Consumers;

public sealed class TestConsumer2 : IConsumer<TestMessageEvent>
{
    private readonly ILogger<TestConsumer2> _logger;

    public TestConsumer2(ILogger<TestConsumer2> logger)
    {
        this._logger = logger;
    }

    public async Task Consume(ConsumeContext<TestMessageEvent> context)
    {
        var sw = new Stopwatch();
        sw.Start();
        this._logger.LogInformation("Message received by consumer 2! {Message}", JsonSerializer.Serialize(context.Message));

        await Task.Delay(3000);
        this._logger.LogInformation("Processment by consumer 2 concluded! {Message} - time spent: {Time} (ms)", JsonSerializer.Serialize(context.Message), sw.ElapsedMilliseconds);
        
        sw.Stop();
    }
}