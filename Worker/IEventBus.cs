using MassTransit;

namespace TestRabbit;

public interface IEventBus
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class;
}

public class EventBus : IEventBus
{
    private readonly IPublishEndpoint publishEndpoint;

    public EventBus(IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) 
        where T : class 
        => this.publishEndpoint.Publish(message, cancellationToken);
    
}