using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_Management_System.Data;
using Student_Management_System.Migrations;
using Student_Management_System.Models;
using Student_Management_System.Models.Entities;

namespace Student_Management_System.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CoursesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(await _context.Course.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
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

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( CourseViewModel course)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {

                return RedirectToAction("Index", "Home");
            }


            if (ModelState.IsValid)
            {
                if (user.UserType == "Admin")
                {
                    var co = new Course
                    {
                        Title = course.Title
                    };

                    _context.Add(co);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(course);
        }

        // GET: Courses/Edit/5
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

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] Course course)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {

                return RedirectToAction("Index", "Home");
            }
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            return View(course);
        }

        // GET: Courses/Delete/5
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

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UserViewModel user = HttpContext.Session.GetObject<UserViewModel>("currentUser");
            if (user == null)
            {

                return RedirectToAction("Index", "Home");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null && user.UserType=="Admin")
            {
                _context.Course.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {

            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
