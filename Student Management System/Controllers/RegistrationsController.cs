using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Data;
using Student_Management_System.Models;
using Student_Management_System.Models.Entities;

namespace Student_Management_System.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public RegistrationsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Registrations
        public async Task<IActionResult> Index()
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
                 return RedirectToAction("Index", "Home");
            }
            if(user.UserType == "Admin")
            {
                var applicationDBContext = _context.Registration.Include(r => r.Course).Include(r => r.Student);
                return View(await applicationDBContext.ToListAsync());
            }
            else
            {
                var applicationDBContext = _context.Registration.Include(r => r.Course).Include(r => r.Student).Where(r => r.Student.UserId == user.Id);

                return View(await applicationDBContext.ToListAsync());
            }
           
        }

        public async Task<IActionResult> Details(int? id)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
                return Unauthorized();
            }
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registration
                .Include(r => r.Course)
                .Include(r => r.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // GET: Registrations/Create
        public IActionResult Create()
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "Title"); // Using 'Title' for display
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId"); // Using 'FirstName' for display (you can combine first and last name)
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,StudentId")] RegistrationViewModel viewModel)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var reg = new Registration
                {
                    CourseId = viewModel.CourseId,
                    StudentId = viewModel.StudentId
                };             
                _context.Add(reg);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", viewModel.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", viewModel.StudentId);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {

                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registration.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", registration.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", registration.StudentId);
            return View(registration);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,StudentId")] Registration registration)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {

                return RedirectToAction("Index", "Home");
            }
            if (id != registration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", registration.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", registration.StudentId);
            return View(registration);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {

                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registration
                .Include(r => r.Course)
                .Include(r => r.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {

                return RedirectToAction("Index", "Home");
            }
            var registration = await _context.Registration.FindAsync(id);
            if (registration != null && user.UserType == "Admin")
            {
                _context.Registration.Remove(registration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(int id)
        {
            return _context.Registration.Any(e => e.Id == id);
        }
    }
}
