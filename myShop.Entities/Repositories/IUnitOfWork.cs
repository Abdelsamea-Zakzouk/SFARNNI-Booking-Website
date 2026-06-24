using System;

namespace myShop.Entities.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICategoryReposotry category { get; }
    IProductReposotry product { get; }
    IFlightProductRepository flightProduct { get; }

    IHostHotelRepository HostHotel { get; }
    IHostFlightRepository HostFlight { get; }

    IOrderHeaderReposotry orderHeader { get; }
    IOrderDetailReposotry orderDetailHotel { get; }
    IOrderDetailFlightReposotry orderDetailFlight { get; }

    IApplicationUsersRepostory applicationUsers { get; }
    int Complete();
}
