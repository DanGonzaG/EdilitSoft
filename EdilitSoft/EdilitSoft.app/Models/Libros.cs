using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EdilitSoft.app.Models
{
    public class Libros
    {
        public int IdLibro { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), MaxLength(100, ErrorMessage = "No más de 100 carácteres")]
        public string ISBN { get; set; }
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }
        [Display(Name = "Editorial")]
        public int EditorialId { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), MaxLength(1000, ErrorMessage = "No más de 1000 carácteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), MaxLength(100, ErrorMessage = "No más de 100 carácteres")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public string Sinopsis { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Año")]
        public int Anyo { get; set; }
        public bool Activo { get; set; }

        //Relacion con otras tablas
        public Categorias? Categoria { get; set; }
        public Editoriales? Editorial { get; set; }
        public Inventario? Inventario { get; set; }
    }
}
