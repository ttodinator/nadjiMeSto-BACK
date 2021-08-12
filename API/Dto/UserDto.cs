using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string CellphoneNumber { get; set; }

        public string UserEmail { get; set; }
        public List<int> Likes { get; set; }
    }
}
