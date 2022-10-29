using Jobs.Data;
using Jobs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Jobs.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHostEnvironment HostEnvironment;
        public SearchController(ApplicationDbContext context, IHostEnvironment hostingEnvironment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.HostEnvironment = hostingEnvironment;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Search(string searchName)
        {
            var result =await _context.Jobs.Where(a=>a.Title.Contains(searchName)
            ||a.Content.Contains(searchName)
            ||a.User.UserName.Contains(searchName)
            ||a.Category.Name.Contains(searchName)
            ||a.Category.Description.Contains(searchName)).ToListAsync();
            return View(result);
        }

    }
}
