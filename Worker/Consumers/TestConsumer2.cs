using MassTransit;
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

    public Task Consume(ConsumeContext<TestMessageEvent> context)
    {
        this._logger.LogInformation("Message received by consumer2! {Message}", JsonSerializer.Serialize(context.Message));

        return Task.CompletedTask;
    }
}