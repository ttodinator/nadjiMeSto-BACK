using API.Dto;
using API.Extensions;
using Data.UnitOfWork;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ReservationController:BaseApiController
    {
        private IUnitOfWork unitOfWork;

        public ReservationController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /*public async Task<int> ReservedTablesCount(ReserveTableDto dto)
        {
            return await unitOfWork.RepositoryReservation.GetReservedTablesCount(dto.re);
        }*/

        [HttpGet]
        public async Task<List<Reservation>> ccc(int userId)
        {
            var Id = User.GetUserId();
            return await unitOfWork.RepositoryReservation.GetReservationsForUser(Id);

        }

        [HttpGet("daily")]
        public async Task<List<Reservation>> GetAllDailyReservations([FromQuery] int restaurantId,[FromQuery] DateTime date)
        {
            var Id = User.GetUserId();

            return await unitOfWork.RepositoryReservation.GetAllDailyReservation(restaurantId,date);

        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> Reserve(ReserveTableDto dto)
        {
            int maxTableCount = await unitOfWork.RepositoryRestaurant.GetTablesCount(dto.RestaurantId, dto.Seating);
            int reservedTablesCount = await unitOfWork.RepositoryReservation.GetReservedTablesCount(
                dto.RestaurantId, dto.TimeOfTheDay, dto.Date, dto.Seating,true);
            int m = maxTableCount - reservedTablesCount;
            var userId = User.GetUserId();
            var x = await unitOfWork.RepositoryReservation.GetMaxid() + 1;
            if (m > 0)
            {
                Reservation reservation = new Reservation
                {
                    RestaurantId = dto.RestaurantId,
                    RestaurantName=dto.RestaurantName,
                    RestaurantTableId = reservedTablesCount+1,
                    ReservationId = x,
                    AppUserId = userId,
                    TimeOfTheDay = dto.TimeOfTheDay,
                    Date = dto.Date,
                    Occupied = true
                };

                unitOfWork.RepositoryReservation.Save(reservation);
                if (await unitOfWork.Complete()) return Ok(reservation);
                return BadRequest("Failed to save reservation");
            }

            return BadRequest("Failed to save reservation");
        }

    }
}
