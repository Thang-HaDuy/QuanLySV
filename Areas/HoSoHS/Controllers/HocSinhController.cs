using System;
using App.Data;
using App.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using App.Areas.HoSoHS.Models;
using Microsoft.AspNetCore.Identity;

namespace App.Areas.HoSoHS.Controllers
{
    [Authorize]
    [Area("HoSoHS")]
    [Route("/HocSinh/[action]/{id?}")]
    public class HocSinhController : Controller
    {
        private readonly DataDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HocSinhController
        (     
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            DataDbContext context
        )
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        // GET: HoSoHS/HocSinh
        public async Task<IActionResult> Index([FromQuery(Name = "p")]int currentPage, [FromQuery(Name = "size")]int pagesize)
        {
            var hocSinh = _context.Users.Where(u => u.HoSoHS.Any());

            int totalLopHoc = await hocSinh.CountAsync();  
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

            var lopHocInPage = await hocSinh.Skip((currentPage - 1) * pagesize)
                             .Take(pagesize)
                             .ToListAsync();   
                        
            return View(lopHocInPage);
        }

        // GET: HoSoHS/HocSinh/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var HocSinh = _context.Users
                            .Include(u => u.HoSoHS)
                            .ThenInclude(h => h.LopHoc);
            if (id == null || HocSinh == null)
            {
                return NotFound();
            }

            var appUser = await HocSinh.FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }


        // GET: HoSoHS/HocSinh/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: HoSoHS/HocSinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SDT,Gender,Email")] AppUser appUser)
        {

            // if (ModelState.IsValid)
            // {
                try
                {
                    var users = await _context.Users.FindAsync(id);
                    if (users != null)
                    {
                        users.SDT = appUser.SDT;
                        users.Gender = appUser.Gender;
                        users.Email = appUser.Email;                       
                        await _userManager.UpdateAsync(users);
                        await _signInManager.RefreshSignInAsync(users);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(appUser.Id))
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
            return View(appUser);
        }


        private bool AppUserExists(string id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
