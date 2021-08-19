using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class SearchReservationsDailyDto
    {
        public int RestaurantId { get; set; }
        public DateTime Date { get; set; }
    }
}
