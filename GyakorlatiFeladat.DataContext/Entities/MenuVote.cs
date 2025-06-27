using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Entities
{
    public class MenuVote
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int UserId { get; set; }
    }
}
