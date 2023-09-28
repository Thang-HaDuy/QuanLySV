using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.HSHocSinh.Models;
using App.Data;
using Microsoft.AspNetCore.Authorization;
using App.Models;

namespace App.Areas.HSHocSinh.Controllers
{
    [Authorize]
    [Area("HSHocSinh")]
    [Route("/[controller]/[action]/{id?}")]
    public class HocSinhController : Controller
    {
        private readonly DataDbContext _context;

        public HocSinhController(DataDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        // GET: HSHocSinh/HocSinh
        public async Task<IActionResult> Index([FromQuery(Name = "p")]int currentPage, [FromQuery(Name = "size")]int pagesize, [FromQuery(Name = "key")]string keyword)
        {
            var hocSinh = _context.HocSinhs
                        .OrderBy(h => h.name);

            if (keyword != null) {
                hocSinh = _context.HocSinhs
                        .Where(h => h.name.Contains(keyword))
                        .OrderBy(h => h.name);
            }

            int totalHocSinh = await hocSinh.CountAsync();  
            if (pagesize <= 0) pagesize = 5;
            int countPages = (int)Math.Ceiling((double)totalHocSinh / pagesize);
 
 
            if (currentPage > countPages) currentPage = countPages;     
            if (currentPage < 1) currentPage = 1; 

            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new {
                    key = keyword,
                    p =  pageNumber,
                    size = pagesize
                })
            };

            var searchModel = new SearchModel()
            {
                action = "Index",
                Controller = "HocSinh",
                name = "key",
                value = keyword
            };

            ViewBag.searchModel = searchModel;
            ViewBag.pagingModel = pagingModel;
            ViewBag.totalHocSinh = totalHocSinh;

            ViewBag.hocSinhIndex = (currentPage - 1) * pagesize;

            var hocSinhInPage = await hocSinh.Skip((currentPage - 1) * pagesize)
                             .Include(h => h.LopHoc)
                             .Take(pagesize)
                             .ToListAsync();   
                        
            return View(hocSinhInPage);
        }

        // GET: HSHocSinh/HocSinh/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.HocSinhs == null)
            {
                return NotFound();
            }

            var hocSinh = await _context.HocSinhs
                .Include(h => h.LopHoc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hocSinh == null)
            {
                return NotFound();
            }

            return View(hocSinh);
        }

        // GET: HSHocSinh/HocSinh/Create
        public IActionResult Create()
        {
            ViewData["LopHocId"] = new SelectList(_context.LopHocs, "Id", "name");
            return View();
        }

        // POST: HSHocSinh/HocSinh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name,HomeAdress,BirthDate,SDT,Gender,LopHocId")] HocSinh hocSinh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hocSinh);
                await _context.SaveChangesAsync();
                StatusMessage = "Vừa thêm một Học sinh mới";
                return RedirectToAction(nameof(Index));
            }
            ViewData["LopHocId"] = new SelectList(_context.LopHocs, "Id", "name", hocSinh.LopHocId);
            return View(hocSinh);
        }

        // GET: HSHocSinh/HocSinh/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.HocSinhs == null)
            {
                return NotFound();
            }

            var hocSinh = await _context.HocSinhs.FindAsync(id);
            if (hocSinh == null)
            {
                return NotFound();
            }
            ViewData["LopHocId"] = new SelectList(_context.LopHocs, "Id", "name", hocSinh.LopHocId);
            return View(hocSinh);
        }

        // POST: HSHocSinh/HocSinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,name,HomeAdress,BirthDate,SDT,Gender,LopHocId")] HocSinh hocSinh)
        {
            
            if (ModelState.IsValid)
            {
                var user = await _context.HocSinhs.FindAsync(id);
                if (user != null)
                {
                    user.name = hocSinh.name;
                    user.HomeAdress = hocSinh.HomeAdress;
                    user.BirthDate = hocSinh.BirthDate;
                    user.SDT = hocSinh.SDT;
                    user.Gender = hocSinh.Gender;
                    user.LopHocId = hocSinh.LopHocId;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(hocSinh);
            }
            ViewData["LopHocId"] = new SelectList(_context.LopHocs, "Id", "name", hocSinh.LopHocId);
            return View(hocSinh);
        }

        // GET: HSHocSinh/HocSinh/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.HocSinhs == null)
            {
                return NotFound();
            }

            var hocSinh = await _context.HocSinhs
                .Include(h => h.LopHoc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hocSinh == null)
            {
                return NotFound();
            }

            return View(hocSinh);
        }

        // POST: HSHocSinh/HocSinh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.HocSinhs == null)
            {
                return Problem("Entity set 'DataDbContext.HocSinhs'  is null.");
            }
            var hocSinh = await _context.HocSinhs.FindAsync(id);
            if (hocSinh != null)
            {
                _context.HocSinhs.Remove(hocSinh);
                await _context.SaveChangesAsync();
                StatusMessage = "Vừa xóa một Học sinh";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool HocSinhExists(Guid id)
        {
          return (_context.HocSinhs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
