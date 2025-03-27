using System.Diagnostics.CodeAnalysis;

namespace EdilitSoft.app.Models
{
    public class Catalogo
    {
        public int IdCatalogo { get; set; }
        public int IdArticulo { get; set; }
        [AllowNull]
        public string RutaImagen { get; set; }
        public bool Activo { get; set; }

        public IEnumerable<Inventario>? Articulos { get; set; }
    }
}
