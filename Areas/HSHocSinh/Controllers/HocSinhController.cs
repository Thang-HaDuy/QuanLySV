using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Areas.HSHocSinh.Models;
using App.Data;

namespace QuanLySV.Areas.HSHocSinh.Controllers
{
    [Area("HSHocSinh")]
    public class HocSinhController : Controller
    {
        private readonly DataDbContext _context;

        public HocSinhController(DataDbContext context)
        {
            _context = context;
        }

        // GET: HSHocSinh/HocSinh
        public async Task<IActionResult> Index()
        {
            var dataDbContext = _context.HocSinhs.Include(h => h.LopHoc);
            return View(await dataDbContext.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,name,HomeAdress,BirthDate,SDT,Gender,LopHocId")] HocSinh hocSinh)
        {
            if (ModelState.IsValid)
            {
                hocSinh.Id = Guid.NewGuid();
                _context.Add(hocSinh);
                await _context.SaveChangesAsync();
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
            if (id != hocSinh.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hocSinh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HocSinhExists(hocSinh.Id))
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
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HocSinhExists(Guid id)
        {
          return (_context.HocSinhs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
