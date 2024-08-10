using System.ComponentModel.DataAnnotations;

namespace Student_Management_System.Models.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        public string UserType { get; set; }

        // Navigation property
        public Student Student { get; set; }
    }
}
