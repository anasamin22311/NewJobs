using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jobs.Data;
using Jobs.Models;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Jobs.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.IO;

namespace Jobs.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IWebHostEnvironment HostEnvironment;
        public JobsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        { 
            _context = context;
            this.HostEnvironment = hostingEnvironment;
            _userManager = userManager;
        }
        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Jobs?.Where(x => x != null).ToListAsync();
            return View(applicationDbContext);
        }
        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }
            var job = await _context.Jobs
                .Include(j => j.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            
            return View(job);
        }
        // GET: Jobs/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description");
            return View();
        }
        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Photo,CategoryId")] JobCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(HostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    MemoryStream memoryStream = new();
                    await model.Photo.OpenReadStream().CopyToAsync(memoryStream);
                    await using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        memoryStream.WriteTo(fs);
                    }
                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    model.Photo.CopyTo(new FileStream(filePath,FileMode.Create));
                    //}
                    //model.ImagePath = filePath;
                }

                Job job = new Job
                {
                    Content = model.Content,
                    Title = model.Title,
                    CategoryId = model.CategoryId,
                    UserId= _userManager.GetUserId(User),
                Image = uniqueFileName
                };
                var newJob = await _context.Jobs.AddAsync(job);
                if (newJob.Entity != null)
                {
                    int x = await _context.SaveChangesAsync();
                    if (x != 0) {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", model.CategoryId);
            return View(model);
        }

      
        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }
            
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            JobCreateViewModel model = new()
            {

                Content = job.Content,
                Title = job.Title,
                CategoryId = job.CategoryId,
                Photo = null
            };


            string path = Path.Combine(HostEnvironment.ContentRootPath, "images", job.Image);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", job.CategoryId);
            return View(model);
        }
        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Photo,CategoryId")] JobCreateViewModel model)

        {
            if (id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                string oldPath = Path.Combine(HostEnvironment.ContentRootPath, "images", model.Photo.FileName);
                if (model != null)
                {
                    System.IO.File.Delete(oldPath);
                    string path = Path.Combine(HostEnvironment.ContentRootPath, "images", model.Photo.FileName);
                    model.Photo.CopyTo(new FileStream(path, FileMode.Create));
                    var job = await _context.Jobs
                .FirstOrDefaultAsync(m => m.Id == id);
            try
            {
                    if (job != null)
                    {
                        job.Content = model.Content;
                        job.Title = model.Title;
                        job.CategoryId = model.CategoryId;
                        job.Image = path;
                    }

            
                _context.Update(job);
                await _context.SaveChangesAsync();
                }
            
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(model.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
                    }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", model.CategoryId);
            return View(model);
    } 
        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }
            var job = await _context.Jobs
                .Include(j => j.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }
        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Jobs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Job'  is null.");
            }
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                _context.Jobs.Remove(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }
    }
}
