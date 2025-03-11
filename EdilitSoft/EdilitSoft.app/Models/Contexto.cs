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
        //2 catalgo

        //dbset DANIEL
        //1 cotizaciones

        //dbset JUAN
        //1 proveedores
        //2 clientes


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }






    }
}
