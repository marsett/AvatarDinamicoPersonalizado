using AvatarDinamicoPersonalizado.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AvatarDinamicoPersonalizado.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Registro> Usuarios { get; set; }
    }
}