using MassTransit.Domain;

namespace TestRabbit.Contracts;

public class CreateWorkItem
{
    public WorkItem WorkItem { get; set; } = new();
}