using System.ComponentModel.DataAnnotations;

namespace EdilitSoft.app.Models
{
    public class Cotizaciones
    {
        public int IdCotizacion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public int IdArticulo { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public int IdProovedor { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public int IdUsuario { get; set; }
        public decimal transporte { get; set; }

        public decimal? OtrosCostos { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal ganancia { get; set; }//volor que debe de ser porcentual   

        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal total { get; set; }

        public bool activo { get; set; }

        //Relacion con las otras tablas
        //public Articulos? Articulo { get; set; }

       // public Usuario? Usuario { get; set; }

        //public Proveedores? Proveedor { get; set; }

        //public Clientes? Cliente { get; set; }
    }
}
