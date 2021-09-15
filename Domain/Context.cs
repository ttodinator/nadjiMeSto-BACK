using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Context: IdentityDbContext<AppUser, AppRole, int,
                IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DbSet<Restaurant> Restaurant { get; set; }
        public DbSet<Reservation> Reservation { get; set; }

        public DbSet<RestaurantTable> RestaurantTable { get; set; }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<Proba> ProbnaTabela { get; set; }
        public DbSet<Like> Likes { get; set; }




        public Context(DbContextOptions options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RestaurantTable>().HasKey(k => new { k.RestaurantTableId, k.RestaurantId });
            //builder.Entity<Restaurant>().HasMany(x => x.Tables).WithOne(x => x.Restaurant);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<Like>().HasKey(l => new { l.AppUserId, l.RestaurantId });

            builder.Entity<Like>().HasOne(l => l.AppUser)
                 .WithMany(u => u.Likes)
                 .HasForeignKey(l => l.AppUserId)
                 .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Like>().HasOne(l => l.Restaurant)
     .           WithMany(u => u.Likes)
                .HasForeignKey(l => l.RestaurantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Reservation>().HasKey(x => new { x.ReservationId, x.RestaurantId, x.RestaurantTableId, x.TimeOfTheDay, x.Date, x.AppUserId });

            builder.Entity<RestaurantTable>().HasMany(x => x.Reservations).WithOne(x => x.RestaurantTable).HasForeignKey(x => new { x.RestaurantTableId,x.RestaurantId, }).OnDelete(DeleteBehavior.NoAction);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;
                                            Database=nadjiMeSto");
        }



    }
}
