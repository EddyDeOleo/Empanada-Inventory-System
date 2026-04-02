namespace EmpanadaInventory.Models
{
    public class Empanada
    {
        public int Id { get; set; }
        public string Sabor { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int CantidadInventario { get; set; }
    }
}