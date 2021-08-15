using API.Dto;
using API.Helpers.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        UserManager<AppUser> userManager;
        SignInManager<AppUser> signInManager;
        private ITokenService tokenService;
        private IMapper mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");
            var user = mapper.Map<AppUser>(registerDto);



            user.UserName = registerDto.Username.ToLower();


            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await userManager.AddToRoleAsync(user, "APPUSER");
            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            List<int> likes = new List<int>();


            return new UserDto
            {
                Username = user.UserName,
                Token = await tokenService.CreateToken(user),
                Likes=likes
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await userManager.Users
                .Include(u => u.Likes)
                //.Include(p => p.Photos) #NE ZABORAVI NA OVAJ DEO SA FOTKAMA I U REPOZITORIJUMU !!!!
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null) return Unauthorized("Invalid username");



            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized();

            List<int> likes = new List<int>();
            foreach (Like like in user.Likes)
            {
                likes.Add(like.RestaurantId);
            }


            return new UserDto
            {
                Username = user.UserName,
                ProfilePhotoUrl=user.ProfilePhotoUrl,
                Token = await tokenService.CreateToken(user),
                Likes=likes
            };
        }


        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }


    }
}