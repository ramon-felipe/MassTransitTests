using MassTransit;

namespace Publisher.Consumers;

public class GettingStartedConsumerConfiguration : ConsumerDefinition<GettingStartedConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<GettingStartedConsumer> consumerConfigurator, IRegistrationContext context)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}