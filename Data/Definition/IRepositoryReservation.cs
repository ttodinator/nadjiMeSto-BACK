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
        Task<int> GetMaxid();
        Task<List<Reservation>> GetAll();
        Task<List<Reservation>> GetAllDailyReservation(int restaurantId, DateTime date);
        Task<int> GetReservedTablesCount(int restaurantId, string timeOfTheDay, DateTime date, int seating,bool occupied);

        Task<int> GenerateTableId(int restaurantId, string timeOfTheDay, DateTime date, int seating, bool occupied);
        void Save(Reservation reservation);
        Task<bool> Reserve();
        Task<List<Reservation>> GetReservationsForUser(int id);
    }
}
