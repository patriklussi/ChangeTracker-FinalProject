using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalProjectBackend.Model;

namespace FinalProjectBackend.Data
{
    public class FinalProjectBackendContext : DbContext
    {
        public FinalProjectBackendContext (DbContextOptions<FinalProjectBackendContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Challenge> Challenge { get; set; }
        public DbSet<WeeklyMsg> WeeklyMsgs { get; set; } = default!;
        public DbSet<DailyMsg> DailyMsgs { get; set; } = default!;
    }
}
