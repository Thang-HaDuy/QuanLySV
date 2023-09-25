using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.HoSoHS.Models;
using App.Data;
using Microsoft.AspNetCore.Authorization;
using App.Utilities;
using App.Models;

namespace App.Areas.HoSoHS.Controllers
{
    [Authorize]
    [Area("HoSoHS")]
    [Route("/QuanLy/[action]/{id?}")]
    public class HoSoHSController : Controller
    {
        private readonly DataDbContext _context;

        public HoSoHSController(DataDbContext context)
        {
            _context = context;
        }
        
        [TempData]
        public string StatusMessage { get; set; }

        // GET: HoSoHS/HoSoHS
        public async Task<IActionResult> Index([FromQuery(Name = "p")]int currentPage, [FromQuery(Name = "size")]int pagesize)
        {
            // var dataDbContext = _context.HoSoHs.Include(h => h.HocSinh).Include(h => h.LopHoc);
            var HoSo = _context.HoSoHs;

            int totalLopHoc = await HoSo.CountAsync();  
            if (pagesize <=0) pagesize = 5;
            int countPages = (int)Math.Ceiling((double)totalLopHoc / pagesize);
 
 
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
            ViewBag.totalLopHoc = totalLopHoc;

            ViewBag.postIndex = (currentPage - 1) * pagesize;

            var hoSoInPage = await HoSo.Skip((currentPage - 1) * pagesize)
                             .Take(pagesize)
                             .Include(h => h.HocSinh)
                             .Include(h => h.LopHoc)
                             .ToListAsync();   
                        
            return View(hoSoInPage);
        }

        // GET: HoSoHS/HoSoHS/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.HoSoHs == null)
            {
                return NotFound();
            }

            var hoSoHS = await _context.HoSoHs
                .Include(h => h.HocSinh)
                .Include(h => h.LopHoc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hoSoHS == null)
            {
                return NotFound();
            }

            return View(hoSoHS);
        }

        // GET: HoSoHS/HoSoHS/Create
        public IActionResult Create()
        {
            ViewData["HocSinhId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["LopHocId"] = new SelectList(_context.LopHocs, "Id", "name");
            return View();
        }

        // POST: HoSoHS/HoSoHS/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LopHocId,HocSinhId,Slug")] Models.HoSoHS hoSoHS)
        {
            hoSoHS.NgayNhapHoc = DateTime.UtcNow;
            if (hoSoHS.Slug == null)
            {
                hoSoHS.Slug = AppUtilities.GenerateSlug(hoSoHS.HocSinhId);
            }

            if(await _context.HoSoHs.AnyAsync(p => p.Slug == hoSoHS.Slug))
            {
                ModelState.AddModelError("Slug", "Nhập chuỗi Url khác");
                return View(hoSoHS);
            }

            // if (true)
            // if (ModelState.IsValid)
            // {
                _context.Add(hoSoHS);
                await _context.SaveChangesAsync();
                StatusMessage = "Vừa tạo Hồ Sơ mới";
                return RedirectToAction(nameof(Index));
            // }
            ViewData["HocSinhId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["LopHocId"] = new SelectList(_context.LopHocs, "Id", "name");
            return View(hoSoHS);
        }

        // GET: HoSoHS/HoSoHS/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.HoSoHs == null)
            {
                return NotFound();
            }

            var hoSoHS = await _context.HoSoHs.FirstOrDefaultAsync(c => c.Id == id);
            if (hoSoHS == null)
            {
                return NotFound();
            }
            return View(hoSoHS);
        }

        // POST: HoSoHS/HoSoHS/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("NgayNhapHoc,DiemTongKet,Slug")] Models.HoSoHS hoSoHS)
        {

            // if (ModelState.IsValid)
            // {
                try
                {
                    var data = await _context.HoSoHs.FirstOrDefaultAsync(c => c.Id == id);
                    if (data != null) {
                        data.NgayNhapHoc = hoSoHS.NgayNhapHoc;
                        data.Slug = hoSoHS.Slug;
                        data.DiemTongKet = hoSoHS.DiemTongKet;
                        _context.Update(data);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoSoHSExists(hoSoHS.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            // }
            return View(hoSoHS);
        }

        // GET: HoSoHS/HoSoHS/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.HoSoHs == null)
            {
                return NotFound();
            }

            var hoSoHS = await _context.HoSoHs
                .Include(h => h.HocSinh)
                .Include(h => h.LopHoc)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hoSoHS == null)
            {
                return NotFound();
            }

            return View(hoSoHS);
        }

        // POST: HoSoHS/HoSoHS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.HoSoHs == null)
            {
                return Problem("Entity set 'DataDbContext.HoSoHs'  is null.");
            }
            var hoSoHS = await _context.HoSoHs.FirstOrDefaultAsync(c => c.Id == id);
            if (hoSoHS != null)
            {
                _context.HoSoHs.Remove(hoSoHS);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoSoHSExists(Guid id)
        {
          return (_context.HoSoHs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
