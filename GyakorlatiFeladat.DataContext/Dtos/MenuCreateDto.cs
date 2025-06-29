using GyakorlatiFeladat.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Dtos
{

    public class MenuDto
    {
        public string? Name { get; set; }
        public List<RecipeInMenuDto> Recipes { get; set; }
        public DateTime When { get; set; }
        public string Type { get; set; }
        public bool IsNeeded { get; set; }
        public int Votes { get; set; }
    }
    public class MenuCreateDto
    {
        public string? Name { get; set; }
        public List<int> RecipeIds { get; set; }
        public DateTime When { get; set; } 
        public MenuType Type { get; set; }
    }
}
