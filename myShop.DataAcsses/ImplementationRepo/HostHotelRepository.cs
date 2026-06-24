using System;
using System.Security.Cryptography.X509Certificates;
using myShop.DataAccess.Data;
using myShop.Entities.Models;
using myShop.Entities.Repositories;
using myShop.Entities.ViewModels;

namespace myShop.DataAccess.ImplementationRepo;

public class HostHotelRepository : GenericReposotory<HostHotel>, IHostHotelRepository
{
    private readonly AppDbContext _context;
    public HostHotelRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public int DecreaseCount(HostHotel hostHotel, int count)
    {
        hostHotel.Count -= count;
        return hostHotel.Count;
    }

    public int IncreaseCount(HostHotel hostHotel, int count)
    {
        hostHotel.Count += count;
        return hostHotel.Count;
    }
}
