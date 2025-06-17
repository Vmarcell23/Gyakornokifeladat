using System.Diagnostics.Contracts;

namespace GyakorlatiFeladat.DataContext.Entities
{

    public enum Roles
    { 
        Owner,Admin,FamilyMember    
    }
    public class FamilyUsers
    {
        public int Id { get; set; } 
        public int FamilyId { get; set; }   
        public Family Family { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public Roles Role { get; set; }
    }
}