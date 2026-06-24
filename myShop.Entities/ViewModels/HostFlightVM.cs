using System;
using myShop.Entities.Models;

namespace myShop.Entities.ViewModels;

public class HostFlightVM
{
    public IEnumerable<HostFlight> CartsList { get; set; }
    public decimal TotalCarts { get; set; }
    public OrderHeader OrderHeader { get; set; }
}
