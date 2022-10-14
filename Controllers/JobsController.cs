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

namespace Jobs.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment HostEnvironment;
        public JobsController(ApplicationDbContext context, IHostEnvironment hostingEnvironment)
        {
            _context = context;
            this.HostEnvironment = hostingEnvironment;
        }
        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Job.Include(j => j.Category);
            return View(await applicationDbContext.ToListAsync());
        }
        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }
            var job = await _context.Job
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Description");
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
                    string uploadsFolder = Path.Combine(HostEnvironment.ContentRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
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
                    Image = uniqueFileName
                };
                var newJob = await _context.Job.AddAsync(job);
                if (newJob.Entity != null)
                {
                    int x = await _context.SaveChangesAsync();
                    if (x != 0) {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Description", model.CategoryId);
            return View(model);
        }
        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }
            
            var job = await _context.Job.FindAsync(id);
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Description", job.CategoryId);
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
                    var job = await _context.Job
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
        ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Description", model.CategoryId);
            return View(model);
    } 
        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Job == null)
            {
                return NotFound();
            }
            var job = await _context.Job
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
            if (_context.Job == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Job'  is null.");
            }
            var job = await _context.Job.FindAsync(id);
            if (job != null)
            {
                _context.Job.Remove(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool JobExists(int id)
        {
            return _context.Job.Any(e => e.Id == id);
        }
    }
}
