using API.Extensions;
using AutoMapper;
using Data.UnitOfWork;
using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UserController:BaseApiController
    {
        IUnitOfWork unitOfWork;
        IMapper mapper;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpPost("like/{restaurantId}")]
        public async Task<ActionResult> UserLikes(int restaurantId)
        {
            var userId = User.GetUserId();


            var user = await unitOfWork.RepositoryUser.GetUserWithLikesAsync(userId);


            Like like = new Like
            {
                AppUserId = user.Id,
                RestaurantId = restaurantId
            };

            user.Likes.Add(like);
            if (await unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to like restaurant");


        }

        [HttpDelete("unlike/{restaurantId}")]
        public async Task<ActionResult> DeleteLike(int restaurantId)
        {
            var userId = User.GetUserId();

            Like l = unitOfWork.RepositoryUser.GetLike(userId, restaurantId);

            unitOfWork.RepositoryUser.DeleteLike(l);

            if (await unitOfWork.Complete()) return Ok();

            return BadRequest("Unable to delete like");

        }

    }
}
