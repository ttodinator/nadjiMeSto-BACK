using API.Dto;
using API.Extensions;
using Data.UnitOfWork;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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

        [HttpPost("upload")]
        public async Task<ActionResult<string>> AddRestaurantPhoto(IFormFile file)
        {
            var userId = User.GetUserId();

            string path = @"C:\Fakultet\nadjiMeSto-FRONT\nadjiMeSto\src\assets\restaurantPhotos\";

            AppUser user = await unitOfWork.RepositoryUser.GetUser(userId);
            string restaurantName= char.ToUpper(user.UserName[0]) + user.UserName.Substring(1);
            Restaurant restaurant = await unitOfWork.RepositoryRestaurant.GetRestaurantByName(restaurantName);


            if (file.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(path + restaurantName))
                    {
                        Directory.CreateDirectory(path + restaurantName);
                    }

                    int numberOfPhotos = Directory.GetFiles(path + restaurantName).Length + 1;
                    if (numberOfPhotos == 6)
                    {
                        return BadRequest();
                    }
                    string[] splits = file.FileName.Split('.');
                    string imgFormat = splits[splits.Length - 1];
                    string profilePhotoUrl = path + restaurantName + @"\" + numberOfPhotos + "." + imgFormat;

                    using (FileStream filestream = System.IO.File.Create(profilePhotoUrl))
                    {
                        await file.CopyToAsync(filestream);
                        int position = profilePhotoUrl.IndexOf("assets");
                        filestream.Flush();
                        profilePhotoUrl = profilePhotoUrl.Substring(position);
                        profilePhotoUrl = profilePhotoUrl.Replace("\\", "/");
                        bool isMain=false;
                        if (numberOfPhotos == 1)
                        {
                            isMain = true;
                        }
                        if(restaurant.Photos==null || restaurant.Photos.Count < 1)
                        {
                            List<RestaurantPhoto> photos = new List<RestaurantPhoto>();
                            restaurant.Photos = photos;
                        }
                        RestaurantPhoto photo = new RestaurantPhoto
                        {
                            RestaurantId = restaurant.RestaurantId,
                            IsMain = isMain,
                            Url = profilePhotoUrl
                        };
                        restaurant.Photos.Add(photo);
                        if (await unitOfWork.Complete()) return Ok();
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

        [HttpPut("{PhotoNumber}")]
        public async Task<ActionResult> ChangeProfilePhoto(string PhotoNumber)
        {
            if (PhotoNumber == null || PhotoNumber == "" ) return BadRequest();

            var userId = User.GetUserId();
            AppUser user = await unitOfWork.RepositoryUser.GetUser(userId);
            string restaurantName = char.ToUpper(user.UserName[0]) + user.UserName.Substring(1);
            Restaurant restaurant = await unitOfWork.RepositoryRestaurant.GetRestaurantByName(restaurantName);
            RestaurantPhoto photo = restaurant.Photos.Where(x => x.IsMain == true).FirstOrDefault();
            photo.IsMain = false;
            foreach (var item in restaurant.Photos)
            {
                string[] splits = item.Url.Split('.');
                string number = splits[0].Substring(splits[0].Length-1);

                if (number == PhotoNumber)
                {
                    item.IsMain = true;
                    break;
                }
            }
            if (await unitOfWork.Complete()) return NoContent();

            return BadRequest();

        }

    }
}
