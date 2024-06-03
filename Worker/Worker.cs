using System.Text.Json;
using TestRabbit.Contracts;

namespace TestRabbit;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IEventBus _eventBus;

    public Worker(ILogger<Worker> logger, IEventBus eventBus)
    {
        _logger = logger;
        this._eventBus = eventBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = new TestMessageEvent { Id = 1, MyProperty = DateTimeOffset.Now.ToString() };

            _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            _logger.LogInformation("Publishing message! {Message}", JsonSerializer.Serialize(message));

            await this._eventBus.PublishAsync(message);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
