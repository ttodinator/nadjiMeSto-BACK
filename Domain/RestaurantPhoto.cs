using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class RestaurantPhoto
    {
        public int RestaurantPhotoId { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public Restaurant Restaurant { get; set; }
        public int RestaurantId { get; set; }
    }
}
