using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public List<Like> Likes { get; set; }
        public List<RestaurantTable> Tables { get; set; }
        public List<RestaurantPhoto> Photos { get; set; }

    }
}
