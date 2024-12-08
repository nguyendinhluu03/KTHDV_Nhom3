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
        public DbSet<Products> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>()
                .Property(e => e.Created_at)
                .HasColumnName("created_at")   // Ánh xạ thuộc tính 'CreatedAt' tới cột 'created_at' trong cơ sở dữ liệu
                .HasDefaultValueSql("GETDATE()"); // Thiết lập giá trị mặc định là GETDATE() cho 'created_at'

            modelBuilder.Entity<Products>()
                .Property(e => e.Updated_at)
                .HasColumnName("updated_at")   // Ánh xạ thuộc tính 'UpdatedAt' tới cột 'updated_at' trong cơ sở dữ liệu
                .HasDefaultValueSql("GETDATE()"); // Thiết lập giá trị mặc định là GETDATE() cho 'updated_at'
        }
    }
}
