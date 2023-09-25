using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipServerConsole.Classes
{
    public class MyApplicationContext : DbContext
    {
        public DbSet<Move> Moves { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-D3D9JEM;Database=GameBS_DB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
