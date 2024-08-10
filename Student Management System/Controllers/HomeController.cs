using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Data;
using Student_Management_System.Models;
using Student_Management_System.Models.Entities;
using System.Diagnostics;

namespace Student_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDBContext bdContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDBContext bdContext, ILogger<HomeController> logger)
        {
            this.bdContext = bdContext;
            _logger = logger;
        }
        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    UserType = "Admin",
                    Email = model.Email,
                    Student = new Student // Assign a single Student object instead of a list
                    {
                        FirstName = string.Empty,
                        LastName = string.Empty,
                        FatherName = string.Empty,
                        MotherName = string.Empty,
                        Phone = string.Empty,
                        JoiningDate = DateTime.Now,
                        DateOfBirth = DateTime.Now,
                        TuitionFee = 0.0m,
                        Address = string.Empty
                    }
                };

                bdContext.Add(user);
                await bdContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public IActionResult Index()
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user != null)
            {
                return RedirectToAction("List", "Students");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserViewModel model)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user != null)
            {
                return RedirectToAction("List", "Students");
            }

            var obj = bdContext.Users.Where(x => x.UserName.Equals(model.UserName) &&  x.Password.Equals(model.Password)).FirstOrDefault();
            if (obj != null)
            {
                // Store user data in session
                HttpContext.Session.SetObject("currentUser", obj);
                return RedirectToAction("List", "Students");
            }
            return View(model);
        }


        public IActionResult Logout()
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Clear the session
            HttpContext.Session.Remove("currentUser");
            // Optionally, you can redirect to a different action after clearing the session
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    UserType = "Student",
                    Email = model.Email,
                    Student = new Student // Assign a single Student object instead of a list
                    {
                        FirstName = string.Empty,
                        LastName = string.Empty,
                        FatherName = string.Empty,
                        MotherName = string.Empty,
                        Phone = string.Empty,
                        JoiningDate = DateTime.Now,
                        DateOfBirth = DateTime.Now,
                        TuitionFee = 0.0m,
                        Address = string.Empty
                    }
                };

                bdContext.Add(user);
                await bdContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        public IActionResult Privacy()
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
