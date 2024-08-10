using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Data;
using Student_Management_System.Helpers;
using Student_Management_System.Models;
using Student_Management_System.Models.Entities;
using System.Diagnostics;
using System.Numerics;
namespace Student_Management_System.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDBContext bdContext;
        
        public StudentsController(ApplicationDBContext bdContext)
        {
            this.bdContext = bdContext;
        }
        [HttpGet]
        public IActionResult Add()
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
               
                return Unauthorized();
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add( StudentViewModel viewModel)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
               
                return Unauthorized(); 
            }
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var student = new Student
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                FatherName = viewModel.FatherName,
                MotherName = viewModel.MotherName,
                Phone = viewModel.Phone,
                JoiningDate = viewModel.JoiningDate,
                DateOfBirth = viewModel.DateOfBirth,
                TuitionFee = viewModel.TuitionFee,
                Address= viewModel.Address
            };
            await bdContext.AddAsync(student);
            await bdContext.SaveChangesAsync();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
                // Handle the case where user is not found in session
                return Unauthorized(); // or another appropriate result
            }
            var student = await bdContext.Students.FindAsync(id);
            return View(student);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Student viewModel)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
                // Handle the case where user is not found in session
                return Unauthorized(); // or another appropriate result
            }

            var student = await bdContext.Students.FindAsync(viewModel.StudentId);
            if (student is not null)
            {
                student.FirstName = viewModel.FirstName;
                student.LastName = viewModel.LastName;
                student.FatherName = viewModel.FatherName;
                student.MotherName = viewModel.MotherName;
                student.Phone = viewModel.Phone;
                student.JoiningDate = viewModel.JoiningDate;
                student.DateOfBirth = viewModel.DateOfBirth;
                student.TuitionFee = viewModel.TuitionFee;
                student.Address = viewModel.Address;
                await bdContext.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View(student);


        }

        [HttpGet]
        public async Task<IActionResult> list(int? pageNumber, DateTime? from, DateTime? to)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
                // Handle the case where user is not found in session
                return Unauthorized(); // or another appropriate result
            }
            if (user.UserType == "Admin")
            {
                var students = bdContext.Students.AsNoTracking();

                if (from.HasValue)
                {
                    students = students.Where(s => s.JoiningDate >= from.Value);
                }

                if (to.HasValue)
                {
                    students = students.Where(s => s.JoiningDate <= to.Value);
                }
                int pageSize = 10;
                return View(await PaginatedList<Student>.CreateAsync(students, pageNumber ?? 1, pageSize));
            }
            else
            {
                var student = await bdContext.Students
                .Where(s => s.UserId == user.Id)
                .FirstOrDefaultAsync();
                if(student is not null)
                {
                    var id = student.StudentId;
                    return RedirectToAction("Detail", new { id });

                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error occurred.");


        }
        [HttpGet]
        public async Task<IActionResult> Detail([FromRoute] int id) {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {

                return Unauthorized();
            }

            var student = await bdContext.Students.FindAsync(id);
            return View(student);

        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {

                return Unauthorized();
            }
            var student = await bdContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StudentId == id);
            if (student is not null)
            {
                bdContext.Students.Remove(student);
                await bdContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
