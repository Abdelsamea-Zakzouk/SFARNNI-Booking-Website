using System;
using myShop.Entities.Models;
using myShop.Entities.ViewModels;

namespace myShop.Entities.Repositories;

public interface IHostHotelRepository : IGenericRepostory<HostHotel>
{
    int IncreaseCount(HostHotel hostHotel, int count);
    int DecreaseCount(HostHotel hostHotel, int count);

}
