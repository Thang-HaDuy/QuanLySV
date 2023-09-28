using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.HSHocSinh.Models;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Authorization;

namespace QuanLySV.Areas.HSHocSinh.Controllers
{
    [Authorize]
    [Area("HSHocSinh")]
    [Route("/[controller]/[action]/{id?}")]
    public class LopHocController : Controller
    {
        private readonly DataDbContext _context;

        public LopHocController(DataDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: HSHocSinh/LopHoc
        public async Task<IActionResult> Index([FromQuery(Name = "p")]int currentPage, [FromQuery(Name = "size")]int pagesize, [FromQuery(Name = "key")]string keyword)
        {
            var lopHoc = _context.LopHocs
                        .Include(l => l.ChuNghiem)
                        .Include(l => l.hocSinhs)
                        .OrderBy(l => l.name);

            if (keyword != null) {
                lopHoc = _context.LopHocs
                        .Include(l => l.ChuNghiem)
                        .Include(l => l.hocSinhs)
                        .Where(h => h.name.Contains(keyword))
                        .OrderBy(l => l.name);
            }           

            int totalLopHoc = await lopHoc.CountAsync();  
            if (pagesize <=0) pagesize = 5;
            int countPages = (int)Math.Ceiling((double)totalLopHoc / pagesize);
 
 
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
                Controller = "LopHoc",
                name = "key",
                value = keyword
            };

            ViewBag.searchModel = searchModel;
            ViewBag.pagingModel = pagingModel;
            ViewBag.totalLopHoc = totalLopHoc;

            ViewBag.lopHocIndex = (currentPage - 1) * pagesize;

            var lopHocInPage = await lopHoc.Skip((currentPage - 1) * pagesize)
                             .Take(pagesize)
                             .ToListAsync();   
                        
            return View(lopHocInPage);
        }

        // GET: HSHocSinh/LopHoc/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.LopHocs == null)
            {
                return NotFound();
            }

            var lopHoc = await _context.LopHocs
                .Include(l => l.hocSinhs)
                .Include(l => l.ChuNghiem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lopHoc == null)
            {
                return NotFound();
            }

            return View(lopHoc);
        }

        // GET: HSHocSinh/LopHoc/Create
        public IActionResult Create()
        {
            ViewData["ChuNghiemId"] = new SelectList(_context.ChuNghiems, "Id", "Gender");
            return View();
        }

        // POST: HSHocSinh/LopHoc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name,ChuNghiemId")] LopHoc lopHoc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lopHoc);
                await _context.SaveChangesAsync();
                StatusMessage = "Vừa tạo Lớp Học mới";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChuNghiemId"] = new SelectList(_context.ChuNghiems, "Id", "Gender", lopHoc.ChuNghiemId);
            return View(lopHoc);
        }

        // GET: HSHocSinh/LopHoc/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.LopHocs == null)
            {
                return NotFound();
            }

            var lopHoc = await _context.LopHocs.FindAsync(id);
            if (lopHoc == null)
            {
                return NotFound();
            }
            ViewData["ChuNghiemId"] = new SelectList(_context.ChuNghiems, "Id", "Gender", lopHoc.ChuNghiemId);
            return View(lopHoc);
        }

        // POST: HSHocSinh/LopHoc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,name,ChuNghiemId")] LopHoc lopHoc)
        {
            if (ModelState.IsValid)
            {
                var lopH = await _context.LopHocs.FindAsync(id);
                if (lopH != null)
                {
                    lopH.name = lopHoc.name;
                    lopH.ChuNghiemId = lopHoc.ChuNghiemId;
                    _context.Update(lopH);
                    await _context.SaveChangesAsync();
                    StatusMessage = "Vừa sửa Lớp Học";
                    return RedirectToAction(nameof(Index));
                }
                return View(lopHoc);
            }
            ViewData["ChuNghiemId"] = new SelectList(_context.ChuNghiems, "Id", "Gender", lopHoc.ChuNghiemId);
            return View(lopHoc);
        }

        // GET: HSHocSinh/LopHoc/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.LopHocs == null)
            {
                return NotFound();
            }

            var lopHoc = await _context.LopHocs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lopHoc == null)
            {
                return NotFound();
            }

            return View(lopHoc);
        }

        // POST: HSHocSinh/LopHoc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.LopHocs == null)
            {
                return Problem("Entity set 'DataDbContext.LopHocs'  is null.");
            }
            var lopHoc = await _context.LopHocs.FindAsync(id);
            if (lopHoc != null)
            {
                StatusMessage = "Vừa xóa một lớp Học";
                _context.LopHocs.Remove(lopHoc);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LopHocExists(Guid id)
        {
          return (_context.LopHocs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
