using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Dtos
{
    public class UserDto
    { 
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
