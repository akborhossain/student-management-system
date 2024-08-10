using System.ComponentModel.DataAnnotations.Schema;

namespace Student_Management_System.Models.Entities
{
    public class Registration
    {
        public int Id { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
