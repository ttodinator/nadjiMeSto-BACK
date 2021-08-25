using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class CreateRestaurantDto
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public int SeatingFor2 { get; set; }
        public int SeatingFor4 { get; set; }

        public int SeatingFor6 { get; set; }
        public int SeatingFor8 { get; set; }
        public int SeatingFor10 { get; set; }
        public int SeatingFor12 { get; set; }
        public string Description { get; set; }






    }
}
