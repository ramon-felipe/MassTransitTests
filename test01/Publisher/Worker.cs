
using MassTransit;
using Publisher.Contracts;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Publisher;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IBus bus;

    public Worker(ILogger<Worker> logger, IBus bus)
    {
        this._logger = logger;
        this.bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = new HelloMessage { MyProperty = "hi!" };

            _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            _logger.LogInformation("Publishing message! {Message}", JsonSerializer.Serialize(message));

            await this.bus.Publish(message, stoppingToken);
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}
