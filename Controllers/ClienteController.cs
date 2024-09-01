using APIENTREVISTA2.Models;
using APIENTREVISTA2.Source;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace APIENTREVISTA2.Controllers
{
    [Authorize]
    [ApiController]
    [Route("CLIENT")]
    [EnableCors("_myAllowSpecificOrigins")]
    
    public class ClienteController : ControllerBase
    {
        private readonly ConexionDB _conexionDB;
        public ClienteController(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }
        [HttpGet]
        [Route("ClientAll")]
        public dynamic listarCliente()
        {
            return _conexionDB.Clientes.ToList();
        }
        [HttpDelete]
        [Route("DeleteClientid")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            var clienteExis = await _conexionDB.Clientes.FindAsync(id);
            if (clienteExis == null)
            {
                return NotFound();
            }

            _conexionDB.Clientes.Remove(clienteExis);
            await _conexionDB.SaveChangesAsync();
            return Ok();
        }
        [HttpGet]
        [Route("ClientId")]
        public IActionResult GetOne(int id)
        {
            var prod = _conexionDB.Clientes.Find(id);
            return Ok(prod);
        }
        [HttpPut]
        [Route("UpdateClient")]
        public async Task<IActionResult> ActualizarCliente(int id, ClienteDB cliente)
        {
            var connection = _conexionDB.Database.GetDbConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "ACT_CLIENT";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", cliente.ID_CLIENTE));
                command.Parameters.Add(new SqlParameter("@RUCDNI", cliente.RUCDNI));
                command.Parameters.Add(new SqlParameter("@NOMBRE", cliente.Nombre));
                command.Parameters.Add(new SqlParameter("@DIRECCION", cliente.Direccion));
                command.Parameters.Add(new SqlParameter("@CORREO", cliente.Correo));
                command.Parameters.Add(new SqlParameter("@ACTIVO", cliente.Activo));

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            //return CreatedAtAction(nameof(GetOne), new { id = cliente.ID_CLIENTE }, cliente);
            return Ok();

        }
        [HttpPost]
        [Route("CreateClient")]
        public async Task<IActionResult> CrearCliente(ClienteC cliente)
        {
            cliente.USUARIO = HashString(cliente.USUARIO);
            cliente.CONTRASEÑA = HashString(cliente.CONTRASEÑA);
            var connection = _conexionDB.Database.GetDbConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SP_NEW_CLIENT";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@RUCDNI", cliente.RUCDNI));
                command.Parameters.Add(new SqlParameter("@NOMBRE", cliente.Nombre));
                command.Parameters.Add(new SqlParameter("@DIRECCION", cliente.Direccion));
                command.Parameters.Add(new SqlParameter("@CORREO", cliente.Correo));
                command.Parameters.Add(new SqlParameter("@USUARIO", cliente.USUARIO));
                command.Parameters.Add(new SqlParameter("@PASSWORD", cliente.CONTRASEÑA));



                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();
            var personal = _conexionDB.Clientes.Where(u => u.RUCDNI == cliente.RUCDNI).FirstOrDefault();

            //return CreatedAtAction(nameof(GetOne), new { id = clienteDTO.RUCDNI }, clienteDTO);
            return Ok(personal);

        }
        private string HashString(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }

}
