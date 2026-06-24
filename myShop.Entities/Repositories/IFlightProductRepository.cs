using myShop.Entities.Models;

namespace myShop.Entities.Repositories
{
    public interface IFlightProductRepository : IGenericRepostory<FlightProduct>
    {
        void Update(FlightProduct flightProduct);
    }
}
