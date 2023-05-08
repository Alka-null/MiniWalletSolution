using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
            //    values: new object[] { "1", "SuperAdmin", "SUPERADMIN", Guid.NewGuid().ToString() });

        }
        private DbSet<User> Users { get; set; }
    }
}
