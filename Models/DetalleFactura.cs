using System.ComponentModel.DataAnnotations;

namespace APIENTREVISTA2.Models
{
    public class DetalleFactura
    {
        [Key]
        public int ID_ITEM { get; set; }
        public int Factura_id { get; set; }
        public int Producto_id { get; set; }
        public string Nombre_Producto { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }
    public class DetalleFacturaC
    {
        public int Factura_id { get; set; }
        public int Producto_id { get; set; }
        public int Cantidad { get; set; }
    }
}
