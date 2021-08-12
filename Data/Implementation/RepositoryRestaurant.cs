using Data.Definition;
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

        public async Task<List<Restaurant>> GetAll()
        {
            return await context.Restaurant.ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantByName(string name)
        {
            var query = context.Restaurant.Include(x => x.Tables);
            return await query.FirstAsync(x => x.Name == name);
        }
    }
}
