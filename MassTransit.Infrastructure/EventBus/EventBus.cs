namespace MassTransit.Infrastructure;

public sealed class EventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISendEndpointProvider _sendEndpoint;

    public EventBus(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpoint)
    {
        this._publishEndpoint = publishEndpoint;
        this._sendEndpoint = sendEndpoint;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class
        => _publishEndpoint.Publish(message, cancellationToken);

    public Task SendAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class
        => this._sendEndpoint.Send(message, cancellationToken);
    
}