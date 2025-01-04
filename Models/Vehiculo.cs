using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pr_Concesionaria.Models
{
    public class Vehiculo
    {
        public int IdVehiculo { get; set; }

        [Display(Name = "Modelo")]
        public int IdModelo { get; set; } 
        public string? Modelo { get; set; }
        public string? Marca { get; set; }

        [Display(Name = "Combustible")]
        public string? TipoCombustible { get; set; }

        [Display(Name = "Transmision")]
        public int IdTransmision { get; set; }
        public string? Transmision { get; set; }
        public string? Categoria { get; set; }

        [Display(Name = "Color")]
        public int IdColor { get; set; } 
        public string? Color { get; set; } 
        public decimal Precio { get; set; }
        public int Stock { get; set; }
    }

}
