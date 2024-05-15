using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Examen.Entidades;

    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext (DbContextOptions<DbContext> options)
            : base(options)
        {
        }

        public DbSet<Examen.Entidades.Anime> Anime { get; set; } = default!;

public DbSet<Examen.Entidades.Tipo> Tipo { get; set; } = default!;
    }
