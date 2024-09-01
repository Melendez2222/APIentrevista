using APIENTREVISTA2.Models;
using APIENTREVISTA2.Source;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace APIENTREVISTA2.Controllers
{
    [Authorize]
    [ApiController]
    [Route("INVOICE_DETAIL")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class DetalleFactController :ControllerBase
    {
        private readonly ConexionDB conexionDB;
        public DetalleFactController(ConexionDB conexionDB)
        {
            this.conexionDB = conexionDB;
        }
        [HttpGet]
        [Route("ListAll")]
        public dynamic DetalleFactura()
        {
            return conexionDB.Detalle_Factura.ToList();
        }
        [HttpPost]
        [Route("CreateDetalleFactura")]
        public async Task<IActionResult> CrearFactura(DetalleFacturaC DFactura)
        {
            var connection = conexionDB.Database.GetDbConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SP_NEW_DETALLE_FACT";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@IDFACTURA", DFactura.Factura_id));
                command.Parameters.Add(new SqlParameter("@IDPRODUCTO", DFactura.Producto_id));
                command.Parameters.Add(new SqlParameter("@CANTIDAD", DFactura.Cantidad));



                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();
            return Ok();

        }
    }
}
