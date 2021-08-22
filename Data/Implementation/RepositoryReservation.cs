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
    public class RepositoryReservation:IRepositoryReservation
    {
        private Context context;
        public RepositoryReservation(Context context)
        {
            this.context = context;
        }

        public Task<int> GenerateTableId(int restaurantId, string timeOfTheDay, DateTime date, int seating, bool occupied)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Reservation>> GetAll()
        {
            return await context.Reservation.Include(x => x.RestaurantTable).ToListAsync();
        }

        public async Task<List<Reservation>> GetAllDailyReservation(int restaurantId, DateTime date)
        {
            return await context.Reservation.Where(x => x.RestaurantId == restaurantId && x.Date==date).Include(x=>x.RestaurantTable).Include(x => x.AppUser).ToListAsync();

        }

        public async Task<int> GetMaxid()
        {
            try
            {
                return await context.Reservation.MaxAsync(x => x.ReservationId);
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public async Task<List<Reservation>> GetReservationsForUser(int id)
        {
            var query =  context.Reservation.Include(x=>x.RestaurantTable).AsQueryable();
            query = query.Where(x => x.AppUserId == id);
            return await query.ToListAsync();
        }

        public async Task<int> GetReservedTablesCount(int restaurantId, string timeOfTheDay, DateTime date, int seating, bool occupied)
        {
            var query = context.Reservation.Include(x => x.RestaurantTable).ThenInclude(x=>x.Restaurant).AsQueryable();
            return await query.Where(
                x => x.Date == date && x.TimeOfTheDay == timeOfTheDay && x.RestaurantId == restaurantId
                    && x.RestaurantTable.Seating == seating && x.Occupied == occupied
                ).CountAsync();
        }


        public Task<bool> Reserve()
        {
            throw new NotImplementedException();
        }

        public void Save(Reservation reservation)
        {
            context.Reservation.Add(reservation);
        }
    }
}
