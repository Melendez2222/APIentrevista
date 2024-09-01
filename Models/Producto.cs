using System.ComponentModel.DataAnnotations;

namespace APIENTREVISTA2.Models
{
    public class Producto
    {
        [Key]
        public int ID_PRODUCTO { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Categoria_pro_id { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public DateTime Fecha_Creacion { get; set; }
    }
    public class ProductoC
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Categoria_pro_id { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }
}
