using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Entities
{
    public class MenuRecipe
    {
        public int MenuId { get; set; }
        public Menu Menu { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
