using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Jobs.Data;
using Jobs.ViewModels;
using Microsoft.AspNetCore.Identity;
using Jobs.Models;

namespace Jobs.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        // GET: Role
        public async Task<IActionResult> Index()
        {
            return View(await roleManager.Roles.ToListAsync());
            //return View(await _context.RoleViewModel.ToListAsync());
        }

        // GET: Role/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id != null)
            {
                var role = await roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    return View(role);
                }
            }
            return NotFound();
            //var roleViewModel = await _context.RoleViewModel
            //.FirstOrDefaultAsync(m => m.Id == id);
            //if (role == null)
            //{
            //    return NotFound();
            //}
        }

        // GET: Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                //_context.SaveChangesAsync();
                var newRole = await roleManager.CreateAsync(role);
                if (newRole != null)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(role);
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (id == null || role == null)
            {
                return NotFound();
            }
            return View(role);
           

            //var roleViewModel = await _context.RoleViewModel.FindAsync(id);
            //if (roleViewModel == null)
            //{
            //    return NotFound();
            //}
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,[Bind("Id,Name")] IdentityRole role)
        {
            var dbRole = await roleManager.FindByIdAsync(id);
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    if (id == role.Id && dbRole != null)
            {
                if (!await roleManager.RoleExistsAsync(role.Name))
                {
                    if (ModelState.IsValid)
                    {
                        dbRole.Name = role.Name;
                        var result = await roleManager.UpdateAsync(dbRole);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            return BadRequest();
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || roleManager.Roles == null)
            {
                return NotFound();
            }

            var role = await roleManager.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (roleManager.Roles== null)
            {
                
                return Problem("Entity set 'RoleManager.Roles'  is null.");
            }
            var role = await  roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await roleManager.DeleteAsync(role);
            }

            
           // await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
