using System.ComponentModel.DataAnnotations;

namespace Student_Management_System.Models.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string FatherName { get; set; }

        [Required]
        [StringLength(50)]
        public string MotherName { get; set; }

        [Phone]
        public string Phone { get; set; }


        [Required]
        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public decimal TuitionFee { get; set; }


        [Required]
        public string Address { get; set; }
    }
}
