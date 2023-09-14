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
using App.Models;
using App.Utilities;

namespace App.Areas.HoSoHS.Controllers
{
    [Authorize]
    [Area("HoSoHS")]
    [Route("/LopHoc/[action]/{id?}")]
    public class LopHocController : Controller
    {
        private readonly DataDbContext _context;

        public LopHocController(DataDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }
        // GET: HoSoHS/LopHoc
        public async Task<IActionResult> Index([FromQuery(Name = "p")]int currentPage, [FromQuery(Name = "size")]int pagesize)
        {
            var lopHoc = _context.LopHocs;

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
                    p =  pageNumber,
                    size = pagesize
                })
            };

            ViewBag.pagingModel = pagingModel;
            ViewBag.totalLopHoc = totalLopHoc;

            ViewBag.postIndex = (currentPage - 1) * pagesize;

            var lopHocInPage = await lopHoc.Skip((currentPage - 1) * pagesize)
                             .Take(pagesize)
                             .ToListAsync();   
                        
            return View(lopHocInPage);

        }

        // GET: HoSoHS/LopHoc/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var LopHocs = _context.LopHocs
                            .Include(h => h.HoSoHS)
                            .ThenInclude(hs => hs.HocSinh);
            if (id == null || LopHocs == null)
            {
                return NotFound();
            }
            var lopHoc = await LopHocs.FirstOrDefaultAsync(m => m.Id == id);
            if (lopHoc == null)
            {
                return NotFound();
            }

            return View(lopHoc);
        }

        // GET: HoSoHS/LopHoc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HoSoHS/LopHoc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name,Slug")] LopHoc lopHoc)
        {
            if (lopHoc.Slug == null)
            {
                lopHoc.Slug = AppUtilities.GenerateSlug(lopHoc.name);
            }

            if(await _context.LopHocs.AnyAsync(p => p.Slug == lopHoc.Slug))
            {
                ModelState.AddModelError("Slug", "Nhập chuỗi Url khác");
                return View(lopHoc);
            }



            if (ModelState.IsValid)
            {
                _context.Add(lopHoc);
                await _context.SaveChangesAsync();
                StatusMessage = "Vừa tạo Lớp Học mới";
                return RedirectToAction(nameof(Index));
            }
            return View(lopHoc);
        }

        // GET: HoSoHS/LopHoc/Edit/5
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
            return View(lopHoc);
        }

        // POST: HoSoHS/LopHoc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,name,Slug")] LopHoc lopHoc)
        {
            if (id != lopHoc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lopHoc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LopHocExists(lopHoc.Id))
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
            return View(lopHoc);
        }

        // GET: HoSoHS/LopHoc/Delete/5
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

        // POST: HoSoHS/LopHoc/Delete/5
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
