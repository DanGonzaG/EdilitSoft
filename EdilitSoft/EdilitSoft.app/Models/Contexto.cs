using EdilitSoft.app.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace EdilitSoft.app.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

        //dbset ANDY
        public DbSet<Categorias> Categoria { get; set; }
        public DbSet<Editoriales> Editorial { get; set; }
        public DbSet<Libros> Libro { get; set; }

        //dbset MIJA
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Catalogo> Catalogo { get; set; }

        //dbset DANIEL
        public DbSet<Cotizaciones> Cotizacion { get; set; }

        //dbset JUAN
        public DbSet<Proveedores> Proveedor { get; set; }
        public DbSet<Clientes> Cliente { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Catalogo>(entity =>
            {
                entity.HasKey(x => x.IdCatalogo);
                entity.Property(x => x.IdArticuloFK);
                entity.Property(x => x.Activo).HasDefaultValue(true);
                entity.Property(x => x.RutaImagen).HasMaxLength(500);

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

            //Inventario
            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.HasKey(x => x.IdArticulo);
                entity.Property(x => x.IdLibro).IsRequired();
                entity.Property(x => x.Fecha).IsRequired();
                entity.Property(x => x.Existencias).IsRequired();
                entity.Property(x => x.Precio).HasColumnType("decimal(18,2)");
                entity.Property(x => x.Activo).HasDefaultValue(true);
                entity.Property(x => x.IdArticulo).ValueGeneratedOnAdd().UseIdentityColumn();

            });

            // Proveedores
            modelBuilder.Entity<Proveedores>(entity =>
            {
                entity.HasKey(p => p.IdProveedor);
                entity.Property(p => p.Identificacion).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Telefono).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Correo).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Activo).HasDefaultValue(true);
            });

            // Clientes
            modelBuilder.Entity<Clientes>(entity =>
            {
                entity.HasKey(c => c.IdCliente);
                entity.Property(c => c.Identificacion).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Telefono).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Correo).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Activo).HasDefaultValue(true);
            });

            //Cotizaciones
            modelBuilder.Entity<Cotizaciones>(Cotizacion =>
            {
                Cotizacion.HasKey(c => c.IdCotizacion);
                Cotizacion.Property(o => o.IdArticulo).IsRequired();
                Cotizacion.Property(t => t.IdProovedor).IsRequired();
                Cotizacion.Property(i => i.IdCliente).IsRequired();
                Cotizacion.Property(z => z.IdUsuario).IsRequired();
                Cotizacion.Property(l => l.Ganancia).IsRequired().HasColumnType("decimal(18,2)"); // Especifica el tipo de columna
                Cotizacion.Property(m => m.Total).IsRequired().HasColumnType("decimal(18,2)");
                Cotizacion.Property(x => x.OtrosCostos).HasColumnType("decimal(18,2)"); // Agrega OtrosCostos
                Cotizacion.Property(y => y.Transporte).HasColumnType("decimal(18,2)"); // Agrega transporte
                Cotizacion.Property(w => w.Activo).HasDefaultValue(true);
            });

            //Relaciones de tablas
            //Relacion Libro Categoria
            modelBuilder.Entity<Libros>()
                .HasOne(l => l.Categoria)
                .WithMany(c => c.Libros)
                .HasForeignKey(a => a.CategoriaId);

            //Relacion Libro Editorial
            modelBuilder.Entity<Libros>()
                .HasOne(e => e.Editorial)
                .WithMany(f => f.Libros)
                .HasForeignKey(h => h.EditorialId);

            //Relacion Inventario Libro
            modelBuilder.Entity<Inventario>()
                .HasOne(inv => inv.Libros)
                .WithOne(li => li.Inventario)
                .HasForeignKey<Inventario>(lib => lib.IdLibro);

            //Realacion Catalogo Inventario
            modelBuilder.Entity<Catalogo>()
                .HasOne(c => c.Inventario)
                .WithOne(t => t.Catalogo)
                .HasForeignKey<Catalogo>(i => i.IdArticuloFK);

            //Relacion Cotizacion Inventario
            modelBuilder.Entity<Cotizaciones>()
                .HasMany(m => m.Articulos)
                .WithOne(n => n.Cotizaciones)
                .HasForeignKey(fo => fo.IdArticulo);

            //Realcion Cotizaciones Proveedor
            modelBuilder.Entity<Cotizaciones>()
                .HasOne(c => c.Proveedor)
                .WithMany(app => app.Cotizaciones)
                .HasForeignKey(a => a.IdProovedor);

            //Realcion Cotizaciones Cliente
            modelBuilder.Entity<Cotizaciones>()
               .HasOne(c => c.Cliente)
               .WithMany(app => app.Cotizaciones)
               .HasForeignKey(a => a.IdCliente);

            /*modelBuilder.Entity<Cotizaciones>()
                .HasOne(c => c.Cliente)
                .WithOne(app => app.Cotizaciones)
                .HasForeignKey<Cotizaciones>(a => a.IdCliente);*/

        }
    }
}


