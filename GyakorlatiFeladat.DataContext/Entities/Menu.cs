using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Entities
{
    public enum MenuType
    {
        Breakfast,
        Lunch,
        Dinner
    }
    public class Menu
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime When { get; set; }
        public MenuType Type { get; set; }
        public bool isNeeded { get; set; }

        public List<MenuRecipe> MenuRecipes { get; set; }
        public List<MenuVote> Votes { get; set; }
        public Family family { get; set; }
        public int FamilyId { get; set; }
        public int CreatorId { get; set; }

    }

}
