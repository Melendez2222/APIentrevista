using APIENTREVISTA2.Models;
using APIENTREVISTA2.Source;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace APIENTREVISTA2.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Persons")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class PersonalController : ControllerBase
    {
        private readonly ConexionDB _conexionDB;
        public PersonalController(ConexionDB conexionDB)
        {
            _conexionDB = conexionDB;
        }

        [HttpGet]
        [Route("ListAll")]
        public IEnumerable<Personal> Get()
        {
            return _conexionDB.Personal.ToList();
        }
        [HttpGet]
        [Route("PersonId")]
        public IActionResult GetOne(string codigo)
        {
            var prod = _conexionDB.Clientes.Find(codigo);
            return Ok(prod);
        }
        [HttpPut]
        [Route("UpdatePerson")]
        public async Task<IActionResult> ActualizarPerson(int id, Personal personal)
        {
            personal.Usuario = HashString(personal.Usuario);
            personal.Contraseña = HashString(personal.Contraseña);
            var connection = _conexionDB.Database.GetDbConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "ACT_PERSON";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", personal.Id_personal));
                command.Parameters.Add(new SqlParameter("@CODIGO_PERO", personal.Codigo_personal));
                command.Parameters.Add(new SqlParameter("@DNI", personal.DNI));
                command.Parameters.Add(new SqlParameter("@NOMBRE", personal.Nombre));
                command.Parameters.Add(new SqlParameter("@DIRECCION", personal.Direccion));
                command.Parameters.Add(new SqlParameter("@CORREO", personal.Correo));
                command.Parameters.Add(new SqlParameter("@ROL", personal.Rol));
                command.Parameters.Add(new SqlParameter("@ACTIVO", personal.Activo));

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return CreatedAtAction(nameof(GetOne), new { id = personal.Id_personal }, personal);

        }
        [HttpPost]
        [Route("CreatePerson")]
        public async Task<IActionResult> CrearPersonal(Personal personalDTO)
        {
            personalDTO.Usuario = HashString(personalDTO.Usuario);
            personalDTO.Contraseña = HashString(personalDTO.Contraseña);
            var connection = _conexionDB.Database.GetDbConnection();
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SP_NEW_CLIENT";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@CODIGO_PERSONAL", personalDTO.Codigo_personal));
                command.Parameters.Add(new SqlParameter("@DNI", personalDTO.DNI));
                command.Parameters.Add(new SqlParameter("@NOMBRE", personalDTO.Nombre));
                command.Parameters.Add(new SqlParameter("@DIRECCION", personalDTO.Direccion));
                command.Parameters.Add(new SqlParameter("@COREEO", personalDTO.Correo));
                command.Parameters.Add(new SqlParameter("@USUARIO", personalDTO.Usuario));
                command.Parameters.Add(new SqlParameter("@CONTRASEÑA", personalDTO.Contraseña));

                await command.ExecuteNonQueryAsync();
            }

            await connection.CloseAsync();

            return CreatedAtAction(nameof(GetOne), new { id = personalDTO.Codigo_personal }, personalDTO);

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
