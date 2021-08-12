using AutoMapper;
using Data.Definition;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implementation
{
    public class RepositoryUser : IRepositoryUser
    {
        private readonly Context context;
        private IMapper mapper;

        public RepositoryUser(Context context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public void DeleteLike(Like l)
        {
            context.Remove(l);
        }

        public Like GetLike(int userId, int restaurantId)
        {
            var likes = context.Likes.AsQueryable();
            Like l = likes.FirstOrDefault(l => l.AppUserId == userId && l.RestaurantId == restaurantId);
            return l;
        }

        public async Task<AppUser> GetUserWithLikesAsync(int id)
        {
            return await context.Users.Where(u=>u.Id==id).Include(u => u.Likes).FirstOrDefaultAsync();
        }
    }
}
