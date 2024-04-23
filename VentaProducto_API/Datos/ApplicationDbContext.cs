using Microsoft.EntityFrameworkCore;
using VentaProducto_API.Modelos;

namespace VentaProducto_API.Datos
{
    public class ApplicationDbContext :DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) //base dbContext, va indicar el padre y le enviamos tada la config 
        {
            
        }

        public DbSet<Producto> Productos { get; set; } //este modelo se creara en la bd


        protected override void OnModelCreating(ModelBuilder modelBuilder) //override del methodo
        {
            modelBuilder.Entity<Producto>().HasData(
                new Producto()
                {
                    Id = 1,
                    Nombre="Colibri",
                    Precio=3000,
                    Detalle="C",
                    Cantidad=0,
                    Tamaño="Mediano",
                    ImageUrl="",
                    FechaCreacion= DateTime.Now,
                    FechaAcualizacion= DateTime.Now
                },
                new Producto()
                {
                    Id = 2,
                    Nombre = "Corazon",
                    Precio = 2000,
                    Detalle = "C",
                    Cantidad = 0,
                    Tamaño = "Mediano",
                    ImageUrl = "",
                    FechaCreacion = DateTime.Now,
                    FechaAcualizacion = DateTime.Now
                }
            );
        }
    }
}
