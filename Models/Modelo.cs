using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pr_Concesionaria.Models
{
    public class Modelo
    {
        public int IdModelo { get; set; }
        public string NombreModelo { get; set; }
        public int Anio { get; set; }
        public int IdMarca { get; set; }
    }

}
