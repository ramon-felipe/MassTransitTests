using MassTransit;
using Publisher.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Consumers;
public class GettingStartedConsumer : IConsumer<HelloMessage>
{
    private readonly ILogger<GettingStartedConsumer> _logger;

    public GettingStartedConsumer(ILogger<GettingStartedConsumer> logger)
    {
        this._logger = logger;
    }

    public Task Consume(ConsumeContext<HelloMessage> context)
    {
        var message = context.Message;

        this._logger.LogInformation("Message consumed! {Message}", message.MyProperty);

        return Task.CompletedTask;
    }
}
