using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutomationManager.Models;
using Microsoft.EntityFrameworkCore;

namespace AutomationManager.Data
{
    public class DatabaseContext : DbContext
    {
  
            public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
            {
            }

            public DbSet<WAMCustomJob> CustomJobs { get; set; }
            public DbSet<WindowsTasks> WindowsTasks { get; set; }
            public DbSet<SQLJob> SQLJobs { get; set; }
            public DbSet<AutomationManager.Models.WAMCustomJob> WAMCustomJob { get; set; }
        
    }
}
