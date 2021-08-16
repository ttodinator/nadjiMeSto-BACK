using API.Dto;
using API.Extensions;
using AutoMapper;
using Data.UnitOfWork;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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


        [HttpPut("upload")]
        public async Task<ActionResult> UploadImage(IFormFile files)
        {
            var userId = User.GetUserId();

            string path= @"C:\Fakultet\nadjiMeSto-FRONT\nadjiMeSto\src\assets\";

            AppUser user= await unitOfWork.RepositoryUser.GetUser(userId);

            if (files.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(path + userId))
                    {
                        Directory.CreateDirectory(path + userId);
                    }

                    int numberOfPhotos = Directory.GetFiles(path+userId).Length +1 ;
                    string[] splits = files.FileName.Split('.');
                    string imgFormat = splits[splits.Length - 1];
                    string profilePhotoUrl = path + userId + @"\" + numberOfPhotos + "." + imgFormat;

                    using (FileStream filestream = System.IO.File.Create(profilePhotoUrl)) 
                    {
                        await files.CopyToAsync(filestream);
                        int position = profilePhotoUrl.IndexOf("assets");
                        filestream.Flush();
                        profilePhotoUrl = profilePhotoUrl.Substring(position);
                        profilePhotoUrl = profilePhotoUrl.Replace("\\", "/");
                        user.ProfilePhotoUrl = profilePhotoUrl;
                        if (await unitOfWork.Complete()) return NoContent();
                        return BadRequest();

                    }
                }
                catch (Exception)
                {

                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetProfilePhotoString()
        {
            var userId = User.GetUserId();
            AppUser user= await unitOfWork.RepositoryUser.GetUser(userId);
            return new UserDto
            {
                Username = user.UserName,
                ProfilePhotoUrl = user.ProfilePhotoUrl
            };
        }

    }
}
