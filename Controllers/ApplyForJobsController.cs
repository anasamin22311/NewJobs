using Jobs.Areas.Identity.Pages;
using Jobs.Data;
using Jobs.Models;
using Jobs.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace Jobs.Controllers
{
    public class ApplyForJobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHostEnvironment HostEnvironment;
        public ApplyForJobsController(ApplicationDbContext context, IHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.HostEnvironment = hostingEnvironment;
            _userManager = userManager;
        }
        [BindProperty]
        public ApplytForJob ApplytForJob { get; set; }

        [HttpGet]
        public async Task<IActionResult> Apply(int id)
        {
            TempData["JobId"] = id;
            return View();
        }

        public class ApplyModel
        {
            public string Message { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Apply([Bind("Message")] ApplyModel model)
        {
            ApplytForJob = new();
            var userId = _userManager.GetUserId(User);
            ApplytForJob.UserId = userId;
            ApplytForJob.JobId = (int)TempData["JobId"];
            ApplytForJob.Message = model.Message;
            ApplytForJob.ApplyDate = DateTime.Now;

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userid = HttpContext.User.GetUserId();
            //var userId = await _userManager.GetUserIdAsync();
            //ApplytForJob applytForJob = new ApplytForJob();
            //applytForJob.JobId = JobID;
            //applytForJob.Message = Message;
            var check = await _context.ApplytForJobs.Where(a => a.JobId == ApplytForJob.JobId && a.UserId == ApplytForJob.UserId).ToListAsync();
            if (check.Count == 0)
            {


                var newApplytForJob = await _context.ApplytForJobs.AddAsync(ApplytForJob);
                    if (newApplytForJob != null)
                {
                    var result = await _context.SaveChangesAsync();
                    if (result != 0)
                    {
                        return View();
                    }
                }
            }
            TempData["JobId"] = ApplytForJob.JobId;
            ModelState.AddModelError(string.Empty, "انت قدمت قبل كدة يعم الحج");
            return View();
        }
        public async Task<IActionResult> GetJobsByUser()
        {
            var userId = _userManager.GetUserId(User);
            var jobs = await _context.ApplytForJobs.Where(a => a.UserId == userId).ToListAsync();
            return View(jobs);

        }
        public async Task<IActionResult> GetJobsByPublisher()
        {
            var userId = _userManager.GetUserId(User);
            var jobs = await _context.Jobs.Where(j => j.UserId == userId).Include(j => j.JobApplications).ToListAsync();
            var Model = jobs.Select(x => new JobViewModel() { Title = x.Title, JobApplications = x.JobApplications }).ToList();

            return View(Model);

            //var jobs = await _context.ApplytForJobs.Include(aj => aj.job).Where(a => a.job.UserId == userId).ToListAsync();
            //var jobs2 = from app in _context.ApplytForJobs join Job in _context.Jobs on app.JobId equals Job.Id
            //           where Job.User.Id == userId select app;


            //var grouped = from j in jobs
            //              group j by j.job.Title into gr
            //              select new JobViewModel
            //              {
            //                  Title = gr.Key,
            //                  Items = gr
            //              };

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ApplytForJobs == null)
            {
                return NotFound();
            }
            var job = await _context.ApplytForJobs
                .Include(j => j.JobId)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }
        // GET: ApplyForJob/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ApplytForJobs == null)
            {
                return NotFound();
            }

            var job = await _context.ApplytForJobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }
        // POST: ApplyForJob/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplytForJob job)

        {

            if (ModelState.IsValid)
            {
                if (job != null)
                {
                    job.ApplyDate = DateTime.Now;
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(GetJobsByUser));
                }

            }
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ApplytForJobs == null)
            {
                return NotFound();
            }
            var job = await _context.ApplytForJobs.FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.ApplytForJobs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ApplytForJob'  is null.");
            }
            var job = await _context.ApplytForJobs.FindAsync(id);
            if (job != null)
            {
                _context.ApplytForJobs.Remove(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GetJobsByUser));
        }
        private bool JobExists(int id)
        {
            return _context.ApplytForJobs.Any(e => e.Id == id);
        }
    }
}
