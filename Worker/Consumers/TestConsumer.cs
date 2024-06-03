using MassTransit;
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

    public Task Consume(ConsumeContext<TestMessageEvent> context)
    {
        this._logger.LogInformation("Message received! {Message}", JsonSerializer.Serialize(context.Message));

        return Task.CompletedTask;
    }
}
