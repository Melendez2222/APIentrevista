using System.ComponentModel.DataAnnotations;

namespace APIENTREVISTA2.Models
{
    public class Factura
    {
        [Key]
        public int ID_Factura { get; set; }
        public int Numero_Factura { get; set; }
        public int Cliente_id { get; set; }
        public int Personal_id { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Porcentaje_IGV { get; set; }
        public decimal IGV { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha_creacion {  get; set; }
    }
    public class FacturaC
    {
        public int Cliente_id { get; set; }
        public int Personal_id { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Porcentaje_IGV { get; set; }
    }
}
