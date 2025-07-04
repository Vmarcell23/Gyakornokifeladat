﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Entities
{
    public class ShoppingItem
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; }
        public int? Quantity { get; set; }
        public bool IsNeeded { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow ;

        public List<ShoppingItemVote> Votes { get; set; } = new();
        public int FamilyId { get; set; }
        public Family Family { get; set; }
    }
}
