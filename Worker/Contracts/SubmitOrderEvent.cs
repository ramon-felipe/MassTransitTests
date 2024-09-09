namespace TestRabbit.Contracts;

public class SubmitOrderEvent
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
}