using System;
using myShop.Entities.Models;

namespace myShop.Entities.Repositories;

public interface IOrderDetailReposotry : IGenericRepostory<OrderDetail>
{
    void Update(OrderDetail orderDetail);

}
