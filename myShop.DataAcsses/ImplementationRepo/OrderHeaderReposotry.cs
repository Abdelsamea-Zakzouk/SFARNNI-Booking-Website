using System;
using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repositories;

namespace myShop.DataAccess.ImplementationRepo;

public class OrderHeaderReposotry : GenericReposotory<OrderHeader>, IOrderHeaderReposotry
{
    private readonly AppDbContext _context;
    public OrderHeaderReposotry(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(OrderHeader orderHeader)
    {
        _context.OrderHeaders.Update(orderHeader);
    }

    public void UpdateStatus(int id, string? OrderStatus, string? PaymentStatus)
    {
        var orderfromDB = _context.OrderHeaders.FirstOrDefault(x => x.Id == id);
        if (orderfromDB != null)
        {
            orderfromDB.OrderStatus = OrderStatus;

            if (PaymentStatus != null)
            {
                orderfromDB.PaymentStatus = PaymentStatus;
            }
        }
    }
}
