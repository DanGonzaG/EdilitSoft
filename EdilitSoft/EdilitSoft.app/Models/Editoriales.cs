using System.ComponentModel.DataAnnotations;

namespace EdilitSoft.app.Models
{
    public class Editoriales
    {
        public int IdEditorial { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), MaxLength(100, ErrorMessage = "No más de 100 carácteres")]
        public string NombreEditorial { get; set; }
        public string? Telefono { get; set; }
        public string? SitioWeb { get; set; }
        public bool Activo { get; set; }

        //Relacion con libro
        public IEnumerable<Libros>? Libros { get; set; }
    }
}
