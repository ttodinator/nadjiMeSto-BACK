﻿using Data.Definition;
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
            return await context.Restaurant.ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantByName(string name)
        {
            var query = context.Restaurant.Include(x => x.Tables);
            return await query.FirstAsync(x => x.Name == name);
        }

        public async Task<int> GetTablesCount(int restaurantId, int seating)
        {
            return await context.RestaurantTable.Where(x => x.RestaurantId == restaurantId && x.Seating == seating).CountAsync();
        }
    }
}
