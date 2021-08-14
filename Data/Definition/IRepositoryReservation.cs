using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Definition
{
    public interface IRepositoryReservation
    {
        Task<List<Reservation>> GetAll();
        Task<int> GetReservedTablesCount(int restaurantId, string timeOfTheDay, DateTime date, int seating,bool occupied);

        Task<bool> Reserve();
    }
}
