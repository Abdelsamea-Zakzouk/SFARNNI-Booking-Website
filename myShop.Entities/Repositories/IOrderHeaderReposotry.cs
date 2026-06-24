using System;
using myShop.Entities.Models;

namespace myShop.Entities.Repositories;

public interface IOrderHeaderReposotry : IGenericRepostory<OrderHeader>
{
    void Update(OrderHeader orderHeader);
    void UpdateStatus(int id, string? OrderStatus, string? PaymentStatus);


}
