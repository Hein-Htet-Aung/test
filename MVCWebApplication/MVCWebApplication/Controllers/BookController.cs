using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCWebApplication.Models;

namespace MVCWebApplication.Controllers
{
    [Route("api/Book")]
    public class BookController : Controller
    {
        private ApplicationDbContext _db;

        [BindProperty]
        public Book Book { get; set; }

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            if(id != null)
            {
                var BookFromDb = await _db.Book.FirstOrDefaultAsync(u=>u.Id == id);
                if(BookFromDb == null)
                {
                    NotFound();
                }

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Book.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var BookFromDb = await _db.Book.FirstOrDefaultAsync(u=>u.Id == id);
            if(BookFromDb == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }
            _db.Book.Remove(BookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, Message = "Delete Successful" });

        }
    }
}
