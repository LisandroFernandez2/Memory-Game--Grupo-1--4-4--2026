using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace juegojuego.BasedeDatos
{
    internal class AppDbContext : DbContext
    {
        //DbSets de cada tabla 

        public DbSet<JugadorDB> Jugador => Set<JugadorDB>();
        public DbSet<CartaDB> Carta => Set<CartaDB>();
        public DbSet<Modo_JuegoDB> Modo_Juego => Set<Modo_JuegoDB>();
        public DbSet<MovimientoDB> Movimiento => Set<MovimientoDB>();
        public DbSet<PartidaDB> Partida => Set<PartidaDB>();
        public DbSet<Par_descubiertoDB> Par_descubierto => Set<Par_descubiertoDB>();
        public DbSet<RecordDB> Record => Set<RecordDB>();
        public DbSet<TematicaDB> Tematica => Set<TematicaDB>();
        public DbSet<NivelDB> Nivel => Set<NivelDB>();
        //Conexión a la base de datos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=localhost;Port=3306;Database=memory_game;Uid=root;pwd=;";
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            optionsBuilder.UseMySql(connectionString, serverVersion);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

    }
}
