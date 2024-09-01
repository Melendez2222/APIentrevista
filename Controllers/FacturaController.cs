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
    [Route("RECEIPT")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class FacturaController : ControllerBase
    {
        private readonly ConexionDB _conexionDB;
        public FacturaController(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }
        [HttpGet]
        [Route("LastFactura")]
        public dynamic ListarFactura()
        {
            var ultimaFactura = _conexionDB.Factura
                .OrderByDescending(f => f.Fecha_creacion)
                .FirstOrDefault();
            var ultimoCodFact = _conexionDB.Factura
                .OrderByDescending(u=>u.Numero_Factura)
                .FirstOrDefault();

            if (ultimaFactura != null)
            {
                //return ultimaFactura.ID_Factura+1;
                return new
                {
                    UltimaFacturaId = ultimaFactura.ID_Factura + 1,
                    UltimoCodFact = ultimoCodFact.Numero_Factura + 1
                };
            }
            else
            {
                return null;
            }
        }
        [HttpPost]
        [Route("CreateFactura")]
        public async Task<IActionResult> CrearFactura(FacturaC factura) 
        {
            int cliid = Convert.ToInt32(factura.Cliente_id);
            int perid=Convert.ToInt32(factura.Personal_id);
            decimal subt=Convert.ToDecimal(factura.Subtotal);
            decimal igvper= Convert.ToDecimal(factura.Porcentaje_IGV);

            var connection = _conexionDB.Database.GetDbConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SP_NEW_FACTURA";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@CLIENTE_ID", cliid));
                command.Parameters.Add(new SqlParameter("@PERSONAL_ID", perid));
                command.Parameters.Add(new SqlParameter("@SUBTOTAL", subt));
                command.Parameters.Add(new SqlParameter("@PORCENTAJE_IGV", igvper));
                await command.ExecuteNonQueryAsync();
            }
            await connection.CloseAsync();

            //return CreatedAtAction(nameof(GetOne), new { id = clienteDTO.RUCDNI }, clienteDTO);
            return Ok();

        }
    }
}
