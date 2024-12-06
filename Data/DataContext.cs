using Baithuchanh2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Baithuchanh2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
