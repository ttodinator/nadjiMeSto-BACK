using Data.Definition;
using Data.ExtensionsData;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementation
{
    public class RepositoryRestaurant : IRepositoryRestaurant
    {
        private Context context;
        public RepositoryRestaurant(Context context)
        {
            this.context = context;
        }

        public async Task<List<RestaurantTable>> FilterTables(int restaurantId)
        {
            var query = context.RestaurantTable.Where(x => x.RestaurantId == restaurantId ).AsQueryable();
            List<RestaurantTable> lista = await query.ToListAsync();
            var a = lista.DistinctBy(x => x.Seating).ToList();

            return a.ToList();
        }


        public async Task<List<Restaurant>> GetAll()
        {
            return await context.Restaurant.Include(x=>x.Photos).ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantByName(string name)
        {
            var query = context.Restaurant.Include(x => x.Tables).Include(x => x.Photos);
            return await query.FirstAsync(x => x.Name == name);
        }

        public async Task<int> GetTablesCount(int restaurantId, int seating)
        {
            return await context.RestaurantTable.Where(x => x.RestaurantId == restaurantId && x.Seating == seating).CountAsync();
        }

        public async Task<int> GetMaxId(int restaurantId)
        {
            return await context.RestaurantTable.Where(x => x.RestaurantId == restaurantId).MaxAsync(x => x.RestaurantTableId);

            //return await context.RestaurantTable.OrderByDescending(u => u.RestaurantTableId).FirstOrDefaultAsync();
        }

        public Task<List<Restaurant>> GetAllLikedRestaurantsByUser(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetMaxTableId()
        {
            try
            {
                return await context.RestaurantTable.MaxAsync(x => x.RestaurantTableId);
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public void Save(Restaurant restaurant)
        {
            context.Add(restaurant);
        }

        public async Task<Restaurant> GetRestaurantByNameToLowe(string name)
        {
            var query = context.Restaurant.Include(x => x.Tables).Include(x => x.Photos);
            return await query.FirstAsync(x => x.Name == name.ToLower());
        }

        public async Task<Restaurant> GetRestaurantTables(int restaurantId,int seating )
        {
            var res= await context.Restaurant.Include(x => x.Tables).FirstOrDefaultAsync(x=>x.RestaurantId==restaurantId );

            Restaurant restaurant = new Restaurant
            {
                RestaurantId = res.RestaurantId,
                Photos = res.Photos,
                Likes = res.Likes,
                Adress = res.Adress,
                PhoneNumber = res.PhoneNumber,
                Description = res.Description,
                Name = res.Name
            };

            restaurant.Tables = new List<RestaurantTable>();

            foreach (var item in res.Tables)
            {
                if (item.Seating == seating) restaurant.Tables.Add(item);
            }

            return  restaurant;

        }
    }
}
