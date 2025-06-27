using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Entities
{
    public class Family
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ShoppingItem> ShoppingItems { get; set; } = new();
        public List<TaskItem> TaskItems { get; set; } = new();
        public List<FamilyUsers> FamilyUsers { get; set; } = new();
        public List<Recipe> Recipes { get; set; } = new();
        public List<Menu> Menus { get; set; } = new();
    }
}
