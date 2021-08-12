using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Like
    {
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
        public Restaurant Restaurant { get; set; }
        public int RestaurantId { get; set; }
    }
}
