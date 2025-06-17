using GyakorlatiFeladat.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Dtos
{
    public class ShoppingItemDto
    {
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public bool IsNeeded { get; set; }

        public List<ShoppingItemVote> Votes { get; set; }
    }

    public class ShoppingItemCreateDto
    {
        public string Name { get; set; }
        public int? Quantity { get; set; }

    }
}
