using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class ReservatioDto
    {
        public int ReservationId { get; set; }
        public int AppUserId { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public int RestaurantTableId { get; set; }
        public string TimeOfTheDay { get; set; }
        public bool Occupied { get; set; }
        public int Seating { get; set; }
        public DateTime Date
        {
            get; set;
        }
    }
}
