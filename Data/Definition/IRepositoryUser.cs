using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Definition
{
    public interface IRepositoryUser
    {
        Task<AppUser> GetUserWithLikesAsync(int id);
        Task<AppUser> GetUser(int id);
        public void UpdateProfilePhoto(AppUser user);
        Task<AppUser> GetReservationForUser(int id);

        Like GetLike(int i, int j);

        void DeleteLike(Like l);
        void Update(AppUser user);
    }
}
