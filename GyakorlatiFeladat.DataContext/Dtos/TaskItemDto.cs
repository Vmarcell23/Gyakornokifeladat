using GyakorlatiFeladat.DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GyakorlatiFeladat.DataContext.Dtos
{
    public class TaskItemDto
    {   
        public string TaskName { get; set; }
        public string TaskDesc { get; set; }  //Description
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsDone { get; set; }

        public List<UserDto> Users { get; set; }

    }
    public class TaskItemCreateDto
    {
        public string TaskName { get; set; }
        public string TaskDesc { get; set; }  //Description
        public DateTime DueDate { get; set; }

        public List<int> UserIds { get; set; } // List of User IDs to associate with the task

    }
}
