using System;
using System.Collections.Generic;

namespace IN_bemutato.Models;

public partial class Meat
{
    public int MeatId { get; set; }

    public string? MeatType { get; set; }

    public float? CurrentStock { get; set; }

    public DateOnly? LastArrivalDate { get; set; }

    public decimal? PricePerKg { get; set; }
}
