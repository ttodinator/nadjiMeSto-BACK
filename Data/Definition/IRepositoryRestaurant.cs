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
        Task<int> GetMaxId(int restaurantId);
        Task<int> GetTablesCount(int restaurantId,int seating);
        Task<Restaurant> GetRestaurantByName(string name);
        Task<List<RestaurantTable>> FilterTables(int restaurantId);
        Task<List<Restaurant>> GetAllLikedRestaurantsByUser(int id);

        Task<int> GetMaxTableId();
        void Save(Restaurant restaurant);
    }
}
