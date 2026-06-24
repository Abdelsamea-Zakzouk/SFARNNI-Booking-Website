using System;
using myShop.Entities.Models;

namespace myShop.Entities.ViewModels;

public class HostHotelVM
{
    public IEnumerable<HostHotel> CartsList { get; set; }
    public decimal TotalCarts { get; set; }
    public OrderHeader OrderHeader { get; set; }
}
