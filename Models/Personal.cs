using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace APIENTREVISTA2.Models
{
    public class Personal
    {
        [Key]
        public int Id_personal { get; set; }
        public string Codigo_personal { get; set; }
        public int DNI { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public int Rol {  get; set; }
        public bool Activo { get; set; }
        public DateTime Fecha_creacion { get; set; }

    }
}
