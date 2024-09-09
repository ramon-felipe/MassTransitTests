namespace MassTransit.Domain;

public sealed class WorkItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
