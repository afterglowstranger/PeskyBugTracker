using Microsoft.EntityFrameworkCore;
using PeskyBugTracker.Models;
using System.Net;

namespace BugData
{
    public class BugContext:DbContext
    {

        public DbSet<PeskyBug> PeskyBugs { get; set; }

        public DbSet<Organisation> Organisations { get; set; }

        public DbSet<Agent> Agents { get; set; }

        public DbSet<NoteEntry> Notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source =(localdb)\\MSSQLLocalDB; Initial Catalog = PeskyBugTracker"
            );
        }

    }
}