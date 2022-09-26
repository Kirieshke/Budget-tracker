using Budget_tracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Budget_tracker.Data
{
    public class BudgetDbContext :DbContext
    {
        public BudgetDbContext(DbContextOptions options) :base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
