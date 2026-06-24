using System;
using myShop.DataAccess.Data;
using myShop.Entities.Repositories;

namespace myShop.DataAccess.ImplementationRepo
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ICategoryReposotry category { get; private set; }
        public IProductReposotry product { get; private set; }
        public IFlightProductRepository flightProduct { get; private set; }  // Add flightProduct repository

        public IHostHotelRepository HostHotel { get; private set; }

        public IHostFlightRepository HostFlight { get; private set; }

        public IOrderHeaderReposotry orderHeader { get; private set; }

        public IOrderDetailReposotry orderDetailHotel { get; private set; }

        public IOrderDetailFlightReposotry orderDetailFlight { get; private set; }

        public IApplicationUsersRepostory applicationUsers { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            category = new CategoryReposotry(context);
            product = new ProductReposotry(context);
            flightProduct = new FlightProductRepository(context); // Initialize flightProduct
            HostHotel = new HostHotelRepository(context);
            HostFlight = new HostFlightRepository(context);
            orderHeader = new OrderHeaderReposotry(context);
            orderDetailHotel = new OrderDetailReposotry(context);
            orderDetailFlight = new OrderDetailFlightReposotry(context);
            applicationUsers = new ApplicationUsersRepostory(context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
