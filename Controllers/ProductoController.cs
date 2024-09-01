using APIENTREVISTA2.Models;
using APIENTREVISTA2.Source;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace APIENTREVISTA2.Controllers
{

    [ApiController]
    [Route("PRODUCT")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ProductoController : ControllerBase
    {
        private readonly ConexionDB _conexionDB;
        public ProductoController(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        [HttpGet]
        [Route("ListAll")]

        public IEnumerable<Producto> Get()
        {
            return _conexionDB.Producto.ToList();
        }
        [Authorize]
        [HttpDelete]
        [Route("DeleteProductid")]
        public async Task<IActionResult> EliminarProduct(int id)
        {
            var productoexis = await _conexionDB.Producto.FindAsync(id);
            if (productoexis == null)
            {
                return NotFound();
            }

            _conexionDB.Producto.Remove(productoexis);
            await _conexionDB.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        [Route("ListProductOne")]
        public IActionResult GetOne(int id)
        {
            var prod = _conexionDB.Producto.Find(id);
            return Ok(prod);
        }
        [Authorize]
        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<IActionResult> ActualizarProducto(int id, Producto producto) {

            var connection = _conexionDB.Database.GetDbConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "ACT_PRODUCT";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", producto.ID_PRODUCTO));
                command.Parameters.Add(new SqlParameter("@CODIGO", producto.Codigo));
                command.Parameters.Add(new SqlParameter("@NOMBRE", producto.Nombre));
                command.Parameters.Add(new SqlParameter("@CATEGORIA", producto.Categoria_pro_id));
                command.Parameters.Add(new SqlParameter("@PRECIO", producto.Precio));
                command.Parameters.Add(new SqlParameter("@STOCK", producto.Stock));
                command.Parameters.Add(new SqlParameter("@ACTIVO", producto.Activo));

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            //return CreatedAtAction(nameof(GetOne), new { id = producto.ID_PRODUCTO }, producto);
            return Ok();

        }
        [Authorize]
        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> Crearproducto(ProductoC productoC)
        {

            var connection = _conexionDB.Database.GetDbConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SP_NEW_PRODUCTO";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@CODIGO", productoC.Codigo));
                command.Parameters.Add(new SqlParameter("@NOMBRE", productoC.Nombre));
                command.Parameters.Add(new SqlParameter("@CATEGORIA", productoC.Categoria_pro_id));
                command.Parameters.Add(new SqlParameter("@PRECIO", productoC.Precio));
                command.Parameters.Add(new SqlParameter("@STOCK", productoC.Stock));

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return Ok();

        }

    }
}
