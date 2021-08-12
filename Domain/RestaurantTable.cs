using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RestaurantTable
    {
        public int RestaurantTableId { get; set; }
        public int TableNumber { get; set; }
        public int Seating { get; set; }
        public Restaurant Restaurant { get; set; }
        public int RestaurantId { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
