using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EdilitSoft.app.Models
{
    public class Contexto : IdentityDbContext<IdentityUser>
    //public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) :base(options){ }

        //dbset ANDY
        //1 categorias
        //2 editoriales
        //3 libros

        //dbset MIJA
        //1 inventario
        DbSet<Inventario> Inventario { get; set; }
        //2 catalgo
        DbSet<Catalogo> Catalogo { get; set; }

        //dbset DANIEL
        //1 cotizaciones

        //dbset JUAN
        //1 proveedores
        //2 clientes


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventario>(entity => 
            {
                entity.HasKey(x => x.IdArticulo);
                entity.Property(x => x.Activo).HasDefaultValue(true);
                entity.Property(x => x.Precio).HasColumnType("decimal(18,2)");
                entity.HasOne(i=> i.Catalogo)
                      .WithOne()
                      .HasForeignKey<Catalogo>(c => c.IdArticulo)
                      .OnDelete(DeleteBehavior.Restrict);

            });
            modelBuilder.Entity<Catalogo>(entity =>
            {
                entity.HasKey(x => x.IdCatalogo);
                entity.Property(x => x.Activo).HasDefaultValue(true);
                entity.Property(x => x.RutaImagen).HasMaxLength(500);
                entity.HasMany(c => c.Articulos)
                      .WithOne()
                      .HasForeignKey(i => i.IdArticulo)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
        }






    }
}
