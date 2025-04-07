using System.ComponentModel.DataAnnotations;

namespace EdilitSoft.app.Models
{
    public class Clientes
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), MaxLength(100, ErrorMessage = "No más de 100 caracteres")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), MaxLength(100, ErrorMessage = "No más de 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), MaxLength(100, ErrorMessage = "No más de 100 caracteres")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Campo obligatorio"), MaxLength(100, ErrorMessage = "No más de 100 caracteres")]
        public string Correo { get; set; }

        public bool Activo { get; set; }

        public List<Cotizaciones>? Cotizaciones { get; set; }
        //public Cotizaciones? Cotizaciones { get; set; }
    }
}
