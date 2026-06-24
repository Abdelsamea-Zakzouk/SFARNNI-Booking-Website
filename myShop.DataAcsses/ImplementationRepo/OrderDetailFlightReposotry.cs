using System;
using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repositories;

namespace myShop.DataAccess.ImplementationRepo;

public class OrderDetailFlightReposotry : GenericReposotory<OrderDetailFlight>, IOrderDetailFlightReposotry
{
    private readonly AppDbContext _context;
    public OrderDetailFlightReposotry(AppDbContext context) : base(context)
    {
        _context = context;
    }


    public void Update(OrderDetailFlight orderDetailFlight)
    {
         _context.OrderDetailFlights.Update(orderDetailFlight);
    }
}
