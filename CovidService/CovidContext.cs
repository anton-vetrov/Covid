using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using CovidService.Models;

namespace CovidService
{

    public class CovidContext : DbContext
    {
        protected override void OnConfiguring
       (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "CovidCases");
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<County> Counties { get; set; }
        public DbSet<Case> Cases { get; set; }
    }
}
