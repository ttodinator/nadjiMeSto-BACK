using API.Dto;
using API.Extensions;
using Data.UnitOfWork;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
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

        [Authorize(Policy = "RequireRestaurantRole")]
        [HttpGet("daily")]
        public async Task<List<Reservation>> GetAllDailyReservations([FromQuery] int restaurantId,[FromQuery] DateTime date)
        {
            var Id = User.GetUserId();
            AppUser user = await unitOfWork.RepositoryUser.GetUser(Id);
            Restaurant restaurant = await unitOfWork.RepositoryRestaurant.GetRestaurantByNameToLowe(user.UserName);
            return await unitOfWork.RepositoryReservation.GetAllDailyReservation(restaurant.RestaurantId,date);

        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> Reserve(ReserveTableDto dto)
        {
            int maxTableCount = await unitOfWork.RepositoryRestaurant.GetTablesCount(dto.RestaurantId, dto.Seating);
            int reservedTablesCount = await unitOfWork.RepositoryReservation.GetReservedTablesCount(
                dto.RestaurantId, dto.TimeOfTheDay, dto.Date, dto.Seating,true);
            int m = maxTableCount - reservedTablesCount;
            var userId = User.GetUserId();
            Restaurant restaurant = await unitOfWork.RepositoryRestaurant.GetRestaurantTables(dto.RestaurantId,dto.Seating);
            var x = await unitOfWork.RepositoryReservation.GetMaxid() + 1;
            if (m > 0)
            {
                RestaurantTable table = restaurant.Tables[reservedTablesCount];
                Reservation reservation = new Reservation
                {
                    RestaurantId = dto.RestaurantId,
                    RestaurantName=dto.RestaurantName,
                    RestaurantTableId = table.RestaurantTableId,
                    ReservationId = x,
                    AppUserId = userId,
                    TimeOfTheDay = dto.TimeOfTheDay,
                    Date = dto.Date.Date,
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
