using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeskyBugTracker.Data;
using PeskyBugTracker.Models;

namespace PeskyBugTracker.Controllers
{
    public class AgentsController : Controller
    {
        private readonly PeskyBugTrackerContext _context;

        public AgentsController(PeskyBugTrackerContext context)
        {
            _context = context;
        }

        // GET: Agents
        public async Task<IActionResult> Index()
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            return View(await _context.Agents.ToListAsync());
        }

        // GET: Agents/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null || _context.Agents == null)
            {
                return NotFound();
            }

            var agent = await _context.Agents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        // GET: Agents/Create
        public IActionResult Create()
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            var organisationList = _context.Organisations.ToList();
            
            ViewBag.orgList = new SelectList(organisationList, "Id", "Name");

            return View();
        }

        // POST: Agents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Forename,SurnameName,UserName,Password,CanRaiseBugs,CanFixBugs,CanLogin")] Agent agent)//, string orgId)
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            var organisation = _context.Organisations.Where(o => o.Id == agent.OrganisationId).FirstOrDefault();
            
            ModelState.Remove("Organisation");
            
            if (ModelState.IsValid)
            {
                agent.Id = Guid.NewGuid();
                _context.Add(agent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(agent);
        }

        // GET: Agents/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }
            
            if (id == null || _context.Agents == null)
            {
                return NotFound();
            }

            var agent = await _context.Agents.FindAsync(id);
            if (agent == null)
            {
                return NotFound();
            }
            var organisationList = _context.Organisations.ToList();

            ViewBag.orgList = new SelectList(organisationList, "Id", "Name");
            
            return View(agent);
        }

        // POST: Agents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Forename,SurnameName,UserName,Password,CanRaiseBugs,CanFixBugs,CanLogin")] Agent agent,string orgId )
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }
            
            if (id != agent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgentExists(agent.Id))
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
            return View(agent);
        }

        // GET: Agents/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }
            
            if (id == null || _context.Agents == null)
            {
                return NotFound();
            }

            var agent = await _context.Agents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        // POST: Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }
            
            if (_context.Agents == null)
            {
                return Problem("Entity set 'PeskyBugTrackerContext.Agents'  is null.");
            }
            var agent = await _context.Agents.FindAsync(id);
            if (agent != null)
            {
                _context.Agents.Remove(agent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgentExists(Guid id)
        {
          return _context.Agents.Any(e => e.Id == id);
        }
    }
}
