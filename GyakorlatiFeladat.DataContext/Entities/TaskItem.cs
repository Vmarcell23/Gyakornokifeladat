namespace GyakorlatiFeladat.DataContext.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string TaskName { get; set; }   
        public string TaskDesc { get; set; }  //Description
        public DateTime DueDate { get; set; }
        public bool IsDone { get; set; }

        public List<User> Users { get; set; }
    }
}