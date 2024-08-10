using System.ComponentModel.DataAnnotations.Schema;

namespace Student_Management_System.Models
{
    public class RegistrationViewModel
    {
        public int Id { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }
    }
}
