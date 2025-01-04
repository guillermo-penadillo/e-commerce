using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pr_Concesionaria.Models
{
    public class Marca
    {
        public int IdMarca { get; set; }
        public string NombreMarca { get; set; }
        public int IdPais { get; set; }
    }

}
