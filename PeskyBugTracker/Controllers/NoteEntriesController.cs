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
    public class NoteEntriesController : Controller
    {
        private readonly PeskyBugTrackerContext _context;

        public NoteEntriesController(PeskyBugTrackerContext context)
        {
            _context = context;
        }

        // GET: NoteEntries
        public async Task<IActionResult> Index()
        {
            var peskyBugTrackerContext = _context.Notes.Include(n => n.AgentAuthor);
            return View(await peskyBugTrackerContext.ToListAsync());
        }

        // GET: NoteEntries/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Notes == null)
            {
                return NotFound();
            }

            var noteEntry = await _context.Notes
                .Include(n => n.AgentAuthor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noteEntry == null)
            {
                return NotFound();
            }

            return View(noteEntry);
        }

        // GET: NoteEntries/Create
        public IActionResult Create()
        {
            ViewData["AgentID"] = new SelectList(_context.Agents, "Id", "Id");
            return View();
        }

        // POST: NoteEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NoteText,TimeStamp,AgentID")] NoteEntry noteEntry)
        {
            if (ModelState.IsValid)
            {
                noteEntry.Id = Guid.NewGuid();
                _context.Add(noteEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AgentID"] = new SelectList(_context.Agents, "Id", "Id", noteEntry.AgentID);
            return View(noteEntry);
        }

        // GET: NoteEntries/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Notes == null)
            {
                return NotFound();
            }

            var noteEntry = await _context.Notes.FindAsync(id);
            if (noteEntry == null)
            {
                return NotFound();
            }
            ViewData["AgentID"] = new SelectList(_context.Agents, "Id", "Id", noteEntry.AgentID);
            return View(noteEntry);
        }

        // POST: NoteEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,NoteText,TimeStamp,AgentID")] NoteEntry noteEntry)
        {
            if (id != noteEntry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noteEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteEntryExists(noteEntry.Id))
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
            ViewData["AgentID"] = new SelectList(_context.Agents, "Id", "Id", noteEntry.AgentID);
            return View(noteEntry);
        }

        // GET: NoteEntries/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Notes == null)
            {
                return NotFound();
            }

            var noteEntry = await _context.Notes
                .Include(n => n.AgentAuthor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noteEntry == null)
            {
                return NotFound();
            }

            return View(noteEntry);
        }

        // POST: NoteEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Notes == null)
            {
                return Problem("Entity set 'PeskyBugTrackerContext.Notes'  is null.");
            }
            var noteEntry = await _context.Notes.FindAsync(id);
            if (noteEntry != null)
            {
                _context.Notes.Remove(noteEntry);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteEntryExists(Guid id)
        {
          return _context.Notes.Any(e => e.Id == id);
        }
    }
}
