using System.Diagnostics.CodeAnalysis;

namespace EdilitSoft.app.Models
{
    public class Catalogo
    {
        public int IdCatalogo { get; set; }
        public int IdArticuloFK { get; set; }

        public string? RutaImagen { get; set; }
        public bool Activo { get; set; }

        public Inventario? Inventario { get; set; }
    }
}
