using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GyakorlatiFeladat.DataContext.Entities;

namespace GyakorlatiFeladat.DataContext.Dtos
{
    public class RecipeDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Ingredients { get; set; }
        public string? Instructions { get; set; }
        public string? Link { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int CreatorId { get; set; }
    }
    public class RecipeInMenuDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Ingredients { get; set; }
        public string? Instructions { get; set; }
        public string? Link { get; set; }
    }


    public class RecipeCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<string>? Ingredients { get; set; }
        public string? Instructions { get; set; }
        public string? Link { get; set; }
    }
}
