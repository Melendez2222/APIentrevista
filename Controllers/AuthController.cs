using APIENTREVISTA2.Models;
using APIENTREVISTA2.Source;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace APIENTREVISTA2.Controllers 
{
    [ApiController]
    [Route("PRODUCT")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class AuthController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        private readonly ConexionDB _conexionDB;

        public AuthController(IConfiguration configuration, ConexionDB conexionDB)
        {
            _configuration = configuration;
            _conexionDB = conexionDB;
        }
        
       
        [HttpPost("login")]
        public IActionResult Login(string usuario, string password)
        {
            
            //return Ok(personal);
            try
            {

                usuario = HashString(usuario);
                password = HashString(password);
                //var personal = _conexionDB.Personal.FirstOrDefault(x => x.Usuario == usuario && x.Contraseña == password);
                var personal = _conexionDB.Personal.Where(u => u.Usuario == usuario && u.Contraseña == password).FirstOrDefault();

                if (personal != null)
                {
                    var claims = new[]
                    {
                 new Claim(JwtRegisteredClaimNames.Sub, personal.Usuario),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                        signingCredentials: creds);

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token),id=personal.Id_personal });
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al obtener el usuario", error = ex.Message });
            }



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
