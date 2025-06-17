using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Dtos
{
    public class FamilyInviteDto
    {
        public int FamilyId { get; set; }
        public int UserId { get; set; }
        public DateTime SentAt { get; set; }
        public bool Accepted { get; set; }
    }
}
