using API.Dto;
using API.Extensions;
using API.Helpers.Interfaces;
using AutoMapper;
using Data.UnitOfWork;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class RestaurantController : BaseApiController
    {
        private IUnitOfWork unitOfWork;
        UserManager<AppUser> userManager;
        SignInManager<AppUser> signInManager;
        private IMapper mapper;


        public RestaurantController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.mapper = mapper;
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
            var distTables = await unitOfWork.RepositoryRestaurant.FilterTables(restaurantId);
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
            return await unitOfWork.RepositoryRestaurant.GetTablesCount(dto.RestaurantId, dto.Seating);
        }

        [Authorize(Policy = "RequireRestaurantRole")]
        [HttpPost("upload")]
        public async Task<ActionResult<string>> AddRestaurantPhoto(IFormFile file)
        {
            var userId = User.GetUserId();

            string path = @"C:\Fakultet\nadjiMeSto-FRONT\nadjiMeSto\src\assets\restaurantPhotos\";

            AppUser user = await unitOfWork.RepositoryUser.GetUser(userId);
            string restaurantName = char.ToUpper(user.UserName[0]) + user.UserName.Substring(1);
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
                        bool isMain = false;
                        if (numberOfPhotos == 1)
                        {
                            isMain = true;
                        }
                        if (restaurant.Photos == null || restaurant.Photos.Count < 1)
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

        [Authorize(Policy = "RequireRestaurantRole")]
        [HttpPut("{PhotoNumber}")]
        public async Task<ActionResult> ChangeProfilePhoto(string PhotoNumber)
        {
            if (PhotoNumber == null || PhotoNumber == "") return BadRequest();

            var userId = User.GetUserId();
            AppUser user = await unitOfWork.RepositoryUser.GetUser(userId);
            string restaurantName = char.ToUpper(user.UserName[0]) + user.UserName.Substring(1);
            Restaurant restaurant = await unitOfWork.RepositoryRestaurant.GetRestaurantByName(restaurantName);
            try
            {
                RestaurantPhoto photo = restaurant.Photos.Where(x => x.IsMain == true).FirstOrDefault();
                photo.IsMain = false;
                foreach (var item in restaurant.Photos)
                {
                    string[] splits = item.Url.Split('.');
                    string number = splits[0].Substring(splits[0].Length - 1);

                    if (number == PhotoNumber)
                    {
                        item.IsMain = true;
                        break;
                    }
                }
                if (await unitOfWork.Complete()) return NoContent();
            }
            catch (Exception)
            {

                return BadRequest();

            }

            return BadRequest();

        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("create")]
        public async Task<ActionResult> CreateRestaurant(CreateRestaurantDto dto)
        {
            if (await UserExists(dto.Name)) return BadRequest("Username is taken");
            var user = new AppUser
            {
                UserName = dto.Name
            };



            user.UserName = dto.Name.ToLower();


            try
            {
                var result = await userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded) return BadRequest(result.Errors);

                var roleResult = await userManager.AddToRoleAsync(user, "RESTAURANT");
                if (!roleResult.Succeeded) return BadRequest(result.Errors);

                Restaurant restaurant = new Restaurant
                {
                    Adress = dto.Adress,
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Description=dto.Description
                };
                int maxTableId = await unitOfWork.RepositoryRestaurant.GetMaxTableId() + 1;
                int tableNumber = 1;
                restaurant.Tables = new List<RestaurantTable>();
                for (int i = 0; i < dto.SeatingFor2; i++)
                {
                    RestaurantTable restaurantTable = new RestaurantTable
                    {
                        RestaurantTableId = maxTableId,
                        Seating = 2,
                        TableNumber = tableNumber
                    };
                    restaurant.Tables.Add(restaurantTable);
                    maxTableId++;
                    tableNumber++;
                }
                for (int i = 0; i < dto.SeatingFor4; i++)
                {
                    RestaurantTable restaurantTable = new RestaurantTable
                    {
                        RestaurantTableId = maxTableId,
                        Seating = 4,
                        TableNumber = tableNumber
                    };
                    restaurant.Tables.Add(restaurantTable);
                    maxTableId++;
                    tableNumber++;
                }
                for (int i = 0; i < dto.SeatingFor6; i++)
                {
                    RestaurantTable restaurantTable = new RestaurantTable
                    {
                        RestaurantTableId = maxTableId,
                        Seating = 6,
                        TableNumber = tableNumber
                    };
                    restaurant.Tables.Add(restaurantTable);
                    maxTableId++;
                    tableNumber++;
                }
                for (int i = 0; i < dto.SeatingFor8; i++)
                {
                    RestaurantTable restaurantTable = new RestaurantTable
                    {
                        RestaurantTableId = maxTableId,
                        Seating = 8,
                        TableNumber = tableNumber
                    };
                    restaurant.Tables.Add(restaurantTable);
                    maxTableId++;
                    tableNumber++;
                }
                for (int i = 0; i < dto.SeatingFor10; i++)
                {
                    RestaurantTable restaurantTable = new RestaurantTable
                    {
                        RestaurantTableId = maxTableId,
                        Seating = 10,
                        TableNumber = tableNumber
                    };
                    restaurant.Tables.Add(restaurantTable);
                    maxTableId++;
                    tableNumber++;
                }
                for (int i = 0; i < dto.SeatingFor12; i++)
                {
                    RestaurantTable restaurantTable = new RestaurantTable
                    {
                        RestaurantTableId = maxTableId,
                        Seating = 12,
                        TableNumber = tableNumber
                    };
                    restaurant.Tables.Add(restaurantTable);
                    maxTableId++;
                    tableNumber++;
                }
                unitOfWork.RepositoryRestaurant.Save(restaurant);
                if (await unitOfWork.Complete()) return Ok();
                return BadRequest();
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}
