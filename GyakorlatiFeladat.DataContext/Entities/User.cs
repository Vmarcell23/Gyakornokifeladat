using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GyakorlatiFeladat.DataContext.Entities
{
 
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }      
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public List<FamilyUsers> FamilyUsers { get; set; } = new();
        public List<TaskItem> TaskItems { get; set; } = new();
    }
}
