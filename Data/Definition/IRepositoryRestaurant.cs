using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Definition
{
    public interface IRepositoryRestaurant
    {
        Task<List<Restaurant>> GetAll();
        Task<int> GetTablesCount(int restaurantId,int seating);
        Task<Restaurant> GetRestaurantByName(string name);
        Task<List<RestaurantTable>> FilterTables(int restaurantId);
    }
}
