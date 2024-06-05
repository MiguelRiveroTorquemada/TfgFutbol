using Microsoft.EntityFrameworkCore;
using Classes;

namespace Data
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Jugador>? Jugadores { get; set; }
        public DbSet<Partido>? Partidos { get; set; }
        public DbSet<Producto>? Productos { get; set; }
        public DbSet<Cliente>? Clientes { get; set; }
        public DbSet<Carrito>? Carritos { get; set; }
        public DbSet<CarnetSocio>? CarnetSocios { get; set; }
        public DbSet<Evento>? Eventos { get; set; }
        public DbSet<Usuario>? Usuarios { get; set; }










        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Jugador>()
                .Ignore(j => j.golesPorPartido);
                
                
        }


    }
}

