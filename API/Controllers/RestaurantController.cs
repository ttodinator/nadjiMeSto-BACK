using Data.UnitOfWork;
using Domain;
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

    }
}
