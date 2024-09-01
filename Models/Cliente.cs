using System.ComponentModel.DataAnnotations;

namespace APIENTREVISTA2.Models
{
    public class Cliente
    {
        [Key]
        public int ID_CLIENTE { get; set; }
        public string RUCDNI { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string USUARIO { get; set; }
        public string CONTRASEÑA { get; set; }
        public bool Activo { get; set; }
        public DateTime Fecha_Creacion { get; set; }
    }

    public class ClienteDB
    {
        public int ID_CLIENTE { get; set; }
        public string RUCDNI { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public bool Activo { get; set; }
    }
    public class ClienteC
    {
        public string RUCDNI { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public string USUARIO { get; set; }
        public string CONTRASEÑA { get; set; }
    }

}
