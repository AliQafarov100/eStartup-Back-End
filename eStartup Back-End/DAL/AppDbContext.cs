using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eStartup_Back_End.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eStartup_Back_End.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Setting> Settings { get; set; }
    }
}
