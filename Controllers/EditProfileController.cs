using Jobs.Data;
using Jobs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Jobs.Areas.Identity.Pages.Account.RegisterModel;

namespace Jobs.Controllers
{
    public class EditProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IWebHostEnvironment HostEnvironment;
        public EditProfileController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            EditProfile profile = new()
            {
                Name=user.UserName,
                Email=user.Email,
            };
            return View(profile);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfile Profile)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null) return NotFound();
            if (!await _userManager.CheckPasswordAsync(user, Profile.CurrentPassword))
            {
            ViewBag.message = "كلمة السر الحالية غير صحيحة";
            }
            else
            {
                var NewPasswordHash = _userManager.PasswordHasher.HashPassword(user,Profile.NewPassword);
                user.UserName = Profile.Name;
                user.Email = Profile.Email;
                user.PasswordHash = NewPasswordHash;
            }
            return View();
        }
    }
}
