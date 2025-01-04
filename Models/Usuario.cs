using System.ComponentModel.DataAnnotations;

namespace Pr_Concesionaria.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Contraseña")]
        public string Password { get; set; }
    }

}
