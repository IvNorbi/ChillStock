using System;
using System.Collections.Generic;

namespace IN_bemutato.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? CustomerName { get; set; }

    public string? Address { get; set; }

    public string? ContactNumber { get; set; }

    public DateOnly? LastOrderDate { get; set; }

    public int? TotalOrders { get; set; }
}
