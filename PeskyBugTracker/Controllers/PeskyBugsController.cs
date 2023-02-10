using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PeskyBugTracker.Data;
using PeskyBugTracker.Models;

namespace PeskyBugTracker.Controllers
{
    public class PeskyBugsController : Controller
    {
        private readonly PeskyBugTrackerContext _context;

        public PeskyBugsController(PeskyBugTrackerContext context)
        {
            _context = context;
        }

        // GET: PeskyBugs
        public async Task<IActionResult> Index()
        {

            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login","Home");
            }

            var bugs = await _context.PeskyBugs.Where(b => b.State != PeskyBug.eState.Closed).ToListAsync();
            foreach (var item in bugs)
            {
                if (item.AssignedTo == null) {
                    item.AssignedToName = "";
                }
                else
                {
                    item.AssignedToName = _context.Agents.Where(a => a.Id == item.AssignedTo).FirstOrDefault().FullName();
                }
            }
            ViewBag.BugsList = "Open";
            return View(bugs);
        }

        public async Task<IActionResult> IndexAllClosed()
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.BugsList = "Closed";
            return View("Index", await _context.PeskyBugs.Where(b => b.State == PeskyBug.eState.Closed).ToListAsync());
        }

        // GET: PeskyBugs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null || _context.PeskyBugs == null)
            {
                return NotFound();
            }

            var peskyBug = await _context.PeskyBugs
                .FirstOrDefaultAsync(m => m.ID == id);

            foreach (var item in _context.Notes.Where(m => m.PeskyBugID == id).ToList())
            {
                item.AgentAuthor = _context.Agents.Where(a => a.Id == item.AgentID).FirstOrDefault();
            }

            peskyBug.Notes = _context.Notes.Where(m => m.PeskyBugID == id).OrderByDescending(d => d.TimeStamp).ToList();
            if (peskyBug == null)
            {
                return NotFound();
            }

            ViewBag.raisedByName = _context.Agents.Where(a => a.Id == peskyBug.RaisedBy).FirstOrDefault().FullName();

            if (peskyBug.AssignedTo == null) {
                ViewBag.assignedToName = "";
            }
            else
            {
                ViewBag.assignedToName = _context.Agents.Where(a => a.Id == peskyBug.AssignedTo).FirstOrDefault().FullName();
            }

            return View(peskyBug);
        }

        // GET: PeskyBugs/Create
        public IActionResult Create()
        {

            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            var enumStateData = from PeskyBug.eState e in Enum.GetValues(typeof(PeskyBug.eState)) select new { ID = (int)e, Name = e.ToString() };
            var enumSeverityData = from PeskyBug.eSeverity e in Enum.GetValues(typeof(PeskyBug.eSeverity)) select new { ID = (int)e, Name = e.ToString() };

            var assigneeOptionsList = _context.Agents.Where(a => a.CanFixBugs).ToList();

            var raisedByAgentList = _context.Agents.Where(a => a.CanRaiseBugs).ToList();

            ViewBag.enumStateData = new SelectList(enumStateData,"ID","Name");
            ViewBag.enumSeverityData = new SelectList(enumSeverityData, "ID", "Name");

            //ViewBag.assigneeOptionsList = new SelectList(assigneeOptionsList, "Id", "Forename");

            var agentList = new List<selectAgent>();
            foreach (var agent in assigneeOptionsList)
            {
                agentList.Add(new selectAgent(agent.Id, agent.Forename + " " + agent.SurnameName));
            }
            ViewBag.assigneeOptionsList = new SelectList(agentList, "Id", "Fullname");

            var raisedByList = new List<selectAgent>();
            foreach (var agent in raisedByAgentList)
            {
                raisedByList.Add(new selectAgent(agent.Id, agent.Forename + " " + agent.SurnameName));
            }
            ViewBag.raisedByList = new SelectList(raisedByList, "Id", "Fullname");

            return View();
        }

        // POST: PeskyBugs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,RaisedByName,RaisedBy,RaisedAt,State,DuplicateId,AssignedTo")] PeskyBug peskyBug)
        {

            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                peskyBug.ID = Guid.NewGuid();
                _context.Add(peskyBug);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(peskyBug);
        }


        [HttpPost]
        public async Task<IActionResult> AddNote([Bind("ID,NoteText")]NewNote newNoteText)
        {

            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid) {

                if ( _context.PeskyBugs == null)
                {
                    return NotFound();
                }

                var peskyBug = await _context.PeskyBugs
                    .FirstOrDefaultAsync(m => m.ID == newNoteText.ID);
                peskyBug.Notes = _context.Notes.Where(m => m.PeskyBugID== newNoteText.ID).ToList();
                if (peskyBug == null)
                {
                    return NotFound();
                }
                else {

                    var agentID = new Guid();
                    if (!string.IsNullOrEmpty(HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID)))
                    {
                     
                        if (str == null || str == "")
                        {
                            agentID = new Guid("00000000-0000-0000-0000-000000000000");
                        }
                        else
                        {
                            agentID = Guid.Parse(str);
                            
                        }
                    }
                    

                    NoteEntry newNote = new NoteEntry(newNoteText.NoteText, agentID);
                    _context.Add(newNote);
                    peskyBug.Notes.Add(newNote);
                    try
                    {
                        _context.Update(peskyBug);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!PeskyBugExists(peskyBug.ID))
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
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: PeskyBugs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {

            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null || _context.PeskyBugs == null)
            {
                return NotFound();
            }

            var peskyBug = await _context.PeskyBugs.FindAsync(id);
            if (peskyBug == null)
            {
                return NotFound();
            }

            var enumStateData = from PeskyBug.eState e in Enum.GetValues(typeof(PeskyBug.eState)) select new { ID = (int)e, Name = e.ToString() };
            ViewBag.enumStateData = new SelectList(enumStateData, "ID", "Name");

            var assigneeOptionsList = _context.Agents.Where(a => a.CanFixBugs).ToList();
            ViewBag.raisedByName = _context.Agents.Where(a => a.Id == peskyBug.RaisedBy).FirstOrDefault().FullName();


            var raisedByAgentList = _context.Agents.Where(a => a.CanRaiseBugs).ToList();
            var raisedByList = new List<selectAgent>();
            foreach (var agent in raisedByAgentList)
            {
                raisedByList.Add(new selectAgent(agent.Id, agent.Forename + " " + agent.SurnameName));
            }
            ViewBag.raisedByList = new SelectList(raisedByList, "Id", "Fullname");

            var agentList = new List<selectAgent>();
            foreach (var agent in assigneeOptionsList)
            {
                agentList.Add(new selectAgent(agent.Id, agent.Forename + " " + agent.SurnameName));
            }
            ViewBag.assigneeOptionsList = new SelectList(agentList, "Id", "Fullname");

            return View(peskyBug);
        }

        // GET: PeskyBugs/Edit/5
        public async Task<IActionResult> UpdateBug(Guid? id)
        {

            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null || _context.PeskyBugs == null)
            {
                return NotFound();
            }

            var peskyBug = await _context.PeskyBugs.FindAsync(id);
            if (peskyBug == null)
            {
                return NotFound();
            }
            return View(peskyBug);
        }

        // POST: PeskyBugs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Title,Description,RaisedByName,RaisedBy,RaisedAt,State,DuplicateId,AssignedTo")] PeskyBug peskyBug)
        {

            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            if (id != peskyBug.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(peskyBug);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeskyBugExists(peskyBug.ID))
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
            return View(peskyBug);
        }

        // POST: PeskyBugs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBug(Guid id, [Bind("ID,Title,Description,RaisedByName,RaisedBy,RaisedAt,State,DuplicateId,AssignedTo")] PeskyBug peskyBug, string? newNote)
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            if (id != peskyBug.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(peskyBug);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeskyBugExists(peskyBug.ID))
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
            return View(peskyBug);
        }

        // GET: PeskyBugs/Close/5
        public async Task<IActionResult> Close(Guid? id)
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null || _context.PeskyBugs == null)
            {
                return NotFound();
            }

            var peskyBug = await _context.PeskyBugs
                .FirstOrDefaultAsync(m => m.ID == id);
            if (peskyBug == null)
            {
                return NotFound();
            }

            return View(peskyBug);
        }

        // POST: PeskyBugs/Close/5
        [HttpPost, ActionName("Close")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseConfirmed(Guid id)
        {
            //Ensure the User is logged in
            var str = HttpContext.Session.GetString(PeskyBugTracker.Models.PageModel.SessionKeyUserID);
            if (str == null || str == "")
            {
                return RedirectToAction("Login", "Home");
            }

            if (_context.PeskyBugs == null)
            {
                return Problem("Entity set 'PeskyBugTrackerContext.PeskyBug'  is null.");
            }
            var peskyBug = await _context.PeskyBugs.FindAsync(id);
            if (peskyBug != null)
            {
                //_context.PeskyBugs.Remove(peskyBug);

                peskyBug.State = PeskyBug.eState.Closed;
                _context.Update(peskyBug);
                //await _context.SaveChangesAsync();
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeskyBugExists(Guid id)
        {
            

            return _context.PeskyBugs.Any(e => e.ID == id);
        }
    }
}
