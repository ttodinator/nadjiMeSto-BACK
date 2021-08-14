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
        public async Task<ActionResult<List<RestaurantTable>>> GetFilteredTables(int restaurantId)
        {
            return await unitOfWork.RepositoryRestaurant.FilterTables(restaurantId);
        }

        [HttpGet("aaa")]
        public async Task<ActionResult<int>> GetFilteredTablesaa()
        {
            return await unitOfWork.RepositoryRestaurant.GetTablesCount(1, 5);
        }
    }
}
