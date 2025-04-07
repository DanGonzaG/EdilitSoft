using System.ComponentModel.DataAnnotations;

namespace EdilitSoft.app.Models
{
    public class Categorias
    {
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), MaxLength(100, ErrorMessage = "No más de 100 carácteres")]
        [Display(Name = "Nombre de la Categoria")]
        public string NombreCategoria { get; set; }
        public bool Activo { get; set; }

        //Relacion con libros
        public IEnumerable<Libros>? Libros { get; set; }
    }
}
