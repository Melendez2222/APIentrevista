
using Microsoft.EntityFrameworkCore;

using APIENTREVISTA2.Models;
using System.Data;

namespace APIENTREVISTA2.Source
{
    public class ConexionDB : DbContext
    {
        internal CommandType CommandType;

        public ConexionDB(DbContextOptions<ConexionDB> options) : base(options)
        { 
            //
        }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<DetalleFactura> Detalle_Factura { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<Personal> Personal { get; set; }
    }
}
