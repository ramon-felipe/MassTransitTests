using MassTransit;

namespace TestRabbit;

public static class MassTransitCfg
{
    public static IServiceCollection ConfigMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();

            cfg.AddConsumers(typeof(Program).Assembly);

            cfg.UsingRabbitMq((ctx, cfg) =>
            {
                var settings = ctx.GetRequiredService<MessageBrokerSettings>();

                cfg.Host(settings.Host, settings.VirtualHost, h =>
                {
                    h.Username(settings.User);
                    h.Password(settings.Password);
                });

                cfg.ConfigureEndpoints(ctx);
                cfg.ConfigureNewDirectExchange();
            });
        });

        return services;
    }

    private static void ConfigureNewDirectExchange(this IRabbitMqBusFactoryConfigurator cfg)
    {
        cfg.ReceiveEndpoint("direct-input-queue", e =>
        {
            e.Bind("new-direct-exchange", x =>
            {
                x.Durable = false;
                x.AutoDelete = true;
                x.ExchangeType = "direct";
                x.RoutingKey = "dog";
            });
        });
    }
}
