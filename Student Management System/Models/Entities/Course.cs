namespace Student_Management_System.Models.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }

        public ICollection<Registration> Registrations { get; set; }
    }
}
