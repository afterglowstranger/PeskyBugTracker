using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeskyBugTracker.Models;

namespace PeskyBugTracker.Data
{
    public class PeskyBugTrackerContext : DbContext
    {
        public PeskyBugTrackerContext (DbContextOptions<PeskyBugTrackerContext> options)
            : base(options)
        {
        }

        

        public DbSet<PeskyBug> PeskyBugs { get; set; }

        public DbSet<Organisation> Organisations { get; set; }

        public DbSet<Agent> Agents { get; set; }

        public DbSet<NoteEntry> Notes { get; set; }
    }
}
