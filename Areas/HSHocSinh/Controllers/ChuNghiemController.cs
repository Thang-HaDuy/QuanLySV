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
    public class ChuNghiemController : Controller
    {
        private readonly DataDbContext _context;

        public ChuNghiemController(DataDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        // GET: HSHocSinh/ChuNghiem
        public async Task<IActionResult> Index([FromQuery(Name = "p")]int currentPage, [FromQuery(Name = "size")]int pagesize)
        {
            //   return _context.ChuNghiem != null ? 
            //               View(await _context.ChuNghiem.ToListAsync()) :
            //               Problem("Entity set 'DataDbContext.ChuNghiem'  is null.");
            var chuNghiem = _context.ChuNghiems
                        .OrderBy(h => h.name);

            int totalChuNghiem = await chuNghiem.CountAsync();  
            if (pagesize <= 0) pagesize = 5;
            int countPages = (int)Math.Ceiling((double)totalChuNghiem / pagesize);
 
 
            if (currentPage > countPages) currentPage = countPages;     
            if (currentPage < 1) currentPage = 1; 

            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new {
                    p =  pageNumber,
                    size = pagesize
                })
            };

            ViewBag.pagingModel = pagingModel;
            ViewBag.totalChuNghiem = totalChuNghiem;

            ViewBag.ChuNghiemIndex = (currentPage - 1) * pagesize;

            var chuNghiemInPage = await chuNghiem.Skip((currentPage - 1) * pagesize)
                             .Include(h => h.LopHocs)
                             .Take(pagesize)
                             .ToListAsync();   
                        
            return View(chuNghiemInPage);
        }

        // GET: HSHocSinh/ChuNghiem/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ChuNghiems == null)
            {
                return NotFound();
            }

            var chuNghiem = await _context.ChuNghiems
                            .Include(c => c.LopHocs)
                            .FirstOrDefaultAsync(m => m.Id == id);
            if (chuNghiem == null)
            {
                return NotFound();
            }

            return View(chuNghiem);
        }

        // GET: HSHocSinh/ChuNghiem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HSHocSinh/ChuNghiem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name,HomeAdress,BirthDate,SDT,Gender")] ChuNghiem chuNghiem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chuNghiem);
                await _context.SaveChangesAsync();
                StatusMessage = "Vừa thêm một Giáo Viên mới";
                return RedirectToAction(nameof(Index));
            }
            return View(chuNghiem);
        }

        // GET: HSHocSinh/ChuNghiem/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ChuNghiems == null)
            {
                return NotFound();
            }

            var chuNghiem = await _context.ChuNghiems.FindAsync(id);
            if (chuNghiem == null)
            {
                return NotFound();
            }
            return View(chuNghiem);
        }

        // POST: HSHocSinh/ChuNghiem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("name,HomeAdress,BirthDate,SDT,Gender")] ChuNghiem chuNghiem)
        {
            
            if (ModelState.IsValid)
            {
                var user = await _context.ChuNghiems.FindAsync(id);
                if (user != null)
                {
                    user.name = chuNghiem.name;
                    user.HomeAdress = chuNghiem.HomeAdress;
                    user.BirthDate = chuNghiem.BirthDate;
                    user.SDT = chuNghiem.SDT;
                    user.Gender = chuNghiem.Gender;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(chuNghiem);
            }
            return View(chuNghiem);
        }

        // GET: HSHocSinh/ChuNghiem/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ChuNghiems == null)
            {
                return NotFound();
            }

            var chuNghiem = await _context.ChuNghiems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chuNghiem == null)
            {
                return NotFound();
            }

            return View(chuNghiem);
        }

        // POST: HSHocSinh/ChuNghiem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ChuNghiems == null)
            {
                return Problem("Entity set 'DataDbContext.ChuNghiem'  is null.");
            }
            var chuNghiem = await _context.ChuNghiems.FindAsync(id);
            if (chuNghiem != null)
            {
                _context.ChuNghiems.Remove(chuNghiem);
            }
            
            await _context.SaveChangesAsync();
            StatusMessage = "Vừa xóa một Giáo Viên";
            return RedirectToAction(nameof(Index));
        }

        private bool ChuNghiemExists(Guid id)
        {
          return (_context.ChuNghiems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
