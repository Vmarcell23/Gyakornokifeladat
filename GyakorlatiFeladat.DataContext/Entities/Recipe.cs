using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Ingredients { get; set; }
        public string? Instructions { get; set; } 
        public string? Link { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int CreatorId { get; set; } 
        public Family family { get; set; }
        public int FamilyId { get; set; }
        public List<MenuRecipe> MenuRecipes { get; set; }
       

    }
}
