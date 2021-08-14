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
    public class RestaurantController:BaseApiController
    {
        private IUnitOfWork unitOfWork;

        public RestaurantController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<List<Restaurant>>> GetRestaurants()
        {
            return await unitOfWork.RepositoryRestaurant.GetAll();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(string name)
        {
            return await unitOfWork.RepositoryRestaurant.GetRestaurantByName(name);
        }

        [HttpGet("filter/{restaurantId}")]
        public async Task<ActionResult<List<int>>> GetFilteredTables(int restaurantId)
        {
            var distTables= await unitOfWork.RepositoryRestaurant.FilterTables(restaurantId);
            List<RestaurantTable> distTablesList = distTables.ToList();
            List<int> listOfTableSeatings = new List<int>();
            foreach (RestaurantTable item in distTablesList)
            {
                listOfTableSeatings.Add(item.Seating);
            }

             return Ok(listOfTableSeatings);
        }

        [HttpGet("count-tables")]
        public async Task<ActionResult<int>> GetFilteredTablesaa(CountTablesDto dto)
        {
            return await unitOfWork.RepositoryRestaurant.GetTablesCount(dto.RestaurantId,dto.Seating);
        }
    }
}
