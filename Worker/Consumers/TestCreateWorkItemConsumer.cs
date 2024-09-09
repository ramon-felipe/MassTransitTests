using MassTransit;
using System.Diagnostics;
using System.Text.Json;
using TestRabbit.Contracts;

namespace TestRabbit.Consumers;

public sealed class TestCreateWorkItemConsumer : IConsumer<CreateWorkItem>
{
    private readonly ILogger<TestCreateWorkItemConsumer> _logger;

    public TestCreateWorkItemConsumer(ILogger<TestCreateWorkItemConsumer> logger)
    {
        this._logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateWorkItem> context)
    {
        var sw = new Stopwatch();
        sw.Start();
        this._logger.LogInformation("Message received by consumer {Consumer}! {Message}", this.GetType().Name, JsonSerializer.Serialize(context.Message));

        await Task.Delay(3000);
        this._logger.LogInformation("Processment by consumer {Consumer} concluded! {Message} - time spent: {Time} (ms)", this.GetType().Name, JsonSerializer.Serialize(context.Message), sw.ElapsedMilliseconds);

        sw.Stop();
    }
}

public sealed class TestCreateWorkItemConsumerDefinition : ConsumerDefinition<TestCreateWorkItemConsumer>
{
    public TestCreateWorkItemConsumerDefinition()
    {
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<TestCreateWorkItemConsumer> consumerConfigurator, IRegistrationContext context)
    {
        this.Endpoint(_ => _.ConfigureConsumeTopology = false);
    }
}