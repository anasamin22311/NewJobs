using Jobs.Data;
using Jobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Jobs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(Contact contact)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("anasamin2211@gmail.com","8264");
            mail.Subject = contact.Subject;
            string body;
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                mail.From = new MailAddress(await _userManager.GetEmailAsync(user));
                string Name = await _userManager.GetUserNameAsync(user);
                mail.To.Add(new MailAddress("anasamin22311@gmail.com"));
                body = "إسم المرسل: " + Name + "<br>" +
                     "بريد المرسل: " + contact.Email + "<br>" +
                     "عنوان الرسالة: " + contact.Subject + "<br>" +
                     "نص الرسالة: <b>" + contact.Message + "</b>";

            }
            else
            {
                string Name = contact.Name;

                mail.From = new MailAddress(contact.Email);
                mail.To.Add(new MailAddress("anasamin22311@gmail.com"));
                body = "إسم المرسل: " + Name + "<br>" +
                     "بريد المرسل: " + contact.Email + "<br>" +
                     "عنوان الرسالة: " + contact.Subject + "<br>" +
                     "نص الرسالة: <b>" + contact.Message + "</b>";
            }

            mail.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail);
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.Include(x=>x.Jobs).ToListAsync());
        }
        public async Task <IActionResult> Details(int id)
        {
            var job = await _context.Jobs
               .FirstOrDefaultAsync(m => m.Id == id);

            if (job == null)
            {
                return NotFound();
            }
            return View(job);
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}