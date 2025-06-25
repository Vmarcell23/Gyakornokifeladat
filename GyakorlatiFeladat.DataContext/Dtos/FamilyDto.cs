using GyakorlatiFeladat.DataContext.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Dtos
{
    public class FamilyDto
    {
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public List<FamilyUserDto> Members { get; set; }

    }

    public class FamilyDeatliedDto
    {
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public List<FamilyUserDto> Members { get; set; }
        public List<ShoppingItemDto> ShoppingItems { get; set; }
        public List<TaskItemDto> Tasks { get; set; }

    }

    public class FamilyCreateDto
    {
        public string Name { get; set; }
    }
    public class FamilyUserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
