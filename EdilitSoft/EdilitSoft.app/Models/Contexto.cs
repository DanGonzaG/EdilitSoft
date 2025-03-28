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
        public DbSet<Categorias> Categoria { get; set; }
        public DbSet<Editoriales> Editorial { get; set; }
        public DbSet<Libros> Libro { get; set; }

        //dbset MIJA
        //1 inventario
        DbSet<Inventario> Inventario { get; set; }
        //2 catalgo
        DbSet<Catalogo> Catalogo { get; set; }

        //dbset DANIEL
        public DbSet<Cotizaciones> Cotizacion { get; set; }

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

            //Categoria
            modelBuilder.Entity<Categorias>(Categoria =>
            {
                Categoria.HasKey(c => c.IdCategoria);
                Categoria.Property(t => t.NombreCategoria).IsRequired().HasMaxLength(100);
                Categoria.Property(q => q.Activo).HasDefaultValue(true);
            });

            //Editorial
            modelBuilder.Entity<Editoriales>(Editorial =>
            {
                Editorial.HasKey(e => e.IdEditorial);
                Editorial.Property(d => d.NombreEditorial).IsRequired().HasMaxLength(100);
                Editorial.Property(i => i.SitioWeb).HasMaxLength(50);
                Editorial.Property(t => t.Telefono).HasMaxLength(200);
                Editorial.Property(o => o.Activo).HasDefaultValue(true);
            });

            //Libros
            modelBuilder.Entity<Libros>(Libro =>
            {
                Libro.HasKey(l => l.IdLibro);
                Libro.Property(i => i.ISBN).IsRequired().HasMaxLength(100);
                Libro.Property(b => b.Titulo).IsRequired().HasMaxLength(1000);
                Libro.Property(r => r.Autor).IsRequired().HasMaxLength(100);
                Libro.Property(o => o.Sinopsis).IsRequired();
                Libro.Property(a => a.Anyo).IsRequired();
                Libro.Property(q => q.Activo).HasDefaultValue(true);

            });

            //Cotizaciones
            modelBuilder.Entity<Cotizaciones>(Cotizacion =>
            {
                Cotizacion.HasKey(c => c.IdCotizacion);
                Cotizacion.Property(o => o.IdArticulo).IsRequired();
                Cotizacion.Property(t => t.IdProovedor).IsRequired();
                Cotizacion.Property(i => i.IdCliente).IsRequired();
                Cotizacion.Property(z => z.IdUsuario).IsRequired();
                Cotizacion.Property(l => l.ganancia).IsRequired();
                Cotizacion.Property(m => m.total).IsRequired();
                Cotizacion.Property(w => w.activo).HasDefaultValue(true);
            });


            //Relaciones de tablas
            modelBuilder.Entity<Libros>().HasOne(l => l.Categoria).WithMany(c => c.Libros).HasForeignKey(a => a.CategoriaId);
            modelBuilder.Entity<Libros>().HasOne(e => e.Editorial).WithMany(f => f.Libros).HasForeignKey(h => h.EditorialId);


        }






    }
}
