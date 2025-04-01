using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EdilitSoft.app.Models
{
    public class Inventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdArticulo { get; set; }
        [ForeignKey("Libros")]
        public int IdLibro { get; set; }
        public DateTime Fecha { get; set; }
        [Required]
        public int Existencias { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
        public bool Activo { get; set; }

        public Catalogo? Catalogo { get; set; }
        public Libros? Libros { get; set; }
        public Cotizaciones? Cotizaciones { get; set; }
    }
}
