using MassTransit;
using MassTransit.Transports.Fabric;
using RabbitMQ.Client;
using TestRabbit.Consumers;
using TestRabbit.Contracts;

namespace TestRabbit;

public static class MassTransitCfg
{
    public static IServiceCollection ConfigMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();

            // cfg.AddConsumers(typeof(Program).Assembly);
            cfg.AddConsumer<TestMessageEventConsumer, TestMessageEventConsumerDefinition>();
            cfg.AddConsumer<TestMessageEventConsumer2>();
            cfg.AddConsumer<TestCreateWorkItemConsumer>();
            // cfg.AddConsumer<TestCreateWorkItemConsumer, TestCreateWorkItemConsumerDefinition>();

            cfg.UsingRabbitMq((ctx, cfg) =>
            {
                var settings = ctx.GetRequiredService<MessageBrokerSettings>();

                cfg.Host(settings.Host, settings.VirtualHost, h =>
                {
                    h.Username(settings.User);
                    h.Password(settings.Password);
                });

                // cfg.ConfigureNewDirectExchange();
                cfg.ConfigureEndpoints(ctx);
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

public static class ConfigureCreateWorkItemExchange2
{
    public static void ConfigureWorkItem2(this IRabbitMqBusFactoryConfigurator cfg, IBusRegistrationContext ctx)
    {
        cfg.ReceiveEndpoint("test-create-work-item1", e =>
        {
            e.ConfigureConsumeTopology = false;

            e.Bind("x-test-create-work-item", x =>
            {
                x.RoutingKey = "test1";
                x.ExchangeType = RabbitMQ.Client.ExchangeType.Direct;
            });
            // e.Consumer<TestCreateWorkItemConsumer>();
        });

        cfg.ReceiveEndpoint("test-create-work-item2", e =>
        {
            e.ConfigureConsumeTopology = false;
            e.Bind("x-test-create-work-item", x =>
            {
                x.RoutingKey = "test1";
                x.ExchangeType = RabbitMQ.Client.ExchangeType.Direct;
            });
            // e.Consumer<TestCreateWorkItemConsumer>();
        });

        //cfg.ReceiveEndpoint("test-create-work-item2", e =>
        //{
        //    e.ConfigureConsumeTopology = false;
        //    e.Bind("x-test-create-work-item", x =>
        //    {
        //        x.RoutingKey = "test2";
        //        x.ExchangeType = RabbitMQ.Client.ExchangeType.Direct;
        //    });
        //    e.Consumer<TestCreateWorkItemConsumer>();
        //});
    }
}