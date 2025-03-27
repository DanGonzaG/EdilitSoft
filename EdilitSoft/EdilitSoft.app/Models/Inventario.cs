namespace EdilitSoft.app.Models
{
    public class Inventario
    {
        public int IdArticulo { get; set; }
        public int IdLibro { get; set; }
        public DateTime Fecha { get; set; }
        public int Existencias { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; }

        public Catalogo? Catalogo { get; set; }
    }
}
