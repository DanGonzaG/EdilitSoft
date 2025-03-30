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
        public decimal Transporte { get; set; }

        public decimal? OtrosCostos { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal Ganancia { get; set; }//volor que debe de ser porcentual   

        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal Total { get; set; }

        public bool Activo { get; set; }

        //Relacion con las otras tablas
        public IEnumerable<Inventario>? Articulos { get; set; }

        public Proveedores? Proveedor { get; set; }

        public Clientes? Cliente { get; set; }
    }
}
