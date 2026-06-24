using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace myShop.Entities.Models;

public class OrderDetailFlight
{
    public int Id { get; set; }

    public int OrderHeaderId { get; set; }
    [ValidateNever]
    public virtual OrderHeader OrderHeader { get; set; }

    public int FlightProductId { get; set; }
    [ValidateNever]
    public virtual FlightProduct FlightProduct { get; set; }

    public decimal Price { get; set; }

    public int Count { get; set; }

}
