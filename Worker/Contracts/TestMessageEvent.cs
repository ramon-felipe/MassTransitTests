using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRabbit.Contracts;
public class TestMessageEvent
{
    public int Id { get; set; }
    public string MyProperty { get; set; } = string.Empty;
}

public class SubmitOrderEvent
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
}