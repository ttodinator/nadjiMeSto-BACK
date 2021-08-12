using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int RestaurantId { get; set; }
        public RestaurantTable RestaurantTable { get; set; }
        public int RestaurantTableId { get; set; }
        public string TimeOfTheDay { get; set; }
        public bool Occupied { get; set; }
        public DateTime Date { get; set; }
    }
}
