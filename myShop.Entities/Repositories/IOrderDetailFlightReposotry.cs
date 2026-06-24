using System;
using myShop.Entities.Models;

namespace myShop.Entities.Repositories;

public interface IOrderDetailFlightReposotry : IGenericRepostory<OrderDetailFlight>
{
    void Update(OrderDetailFlight orderDetailFlight);

}
