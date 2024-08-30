using DoneTool.Models;
using Microsoft.EntityFrameworkCore;

namespace DoneTool.Data
{
    public class DoneToolContext : DbContext
    {
        public DoneToolContext(DbContextOptions<DoneToolContext> options) : base(options) { }

        public DbSet<Checks> Checks { get; set; }
        public DbSet<TaskChecklist> TaskChecklist { get; set; }
        public DbSet<TaskChecks> TaskChecks { get; set; }

    }
}
