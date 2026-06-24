using System;
using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repositories;

namespace myShop.DataAccess.ImplementationRepo;

public class OrderDetailReposotry : GenericReposotory<OrderDetail>, IOrderDetailReposotry
{
    private readonly AppDbContext _context;
    public OrderDetailReposotry(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(OrderDetail orderDetail)
    {
        _context.OrderDetailHotels.Update(orderDetail);
    }
}
