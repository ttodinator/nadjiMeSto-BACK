using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class ReserveTableDto
    {
        public int RestaurantId { get; set; }
        public int Seating { get; set; }
        public DateTime Date { get; set; }
        public string TimeOfTheDay { get; set; }
        public string RestaurantName { get; set; }
    }
}
