using Microsoft.EntityFrameworkCore;
using dotnet_rpg.Models;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        //This will create a db table named Characters
        public DbSet<Character> Characters { get; set; }
        
    }
}