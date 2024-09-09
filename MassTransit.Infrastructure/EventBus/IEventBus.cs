namespace MassTransit.Infrastructure;

public interface IEventBus
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class;

    Task SendAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : class;
}
