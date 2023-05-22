using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuttingOptimizer.Api.Models
{
    public class PlateDTO
    {
        [Required(ErrorMessage = "Materiaal is een verplicht veld.")]
        public string ID { get; set; }

        [Required(ErrorMessage = "Materiaal lengte is een verplicht veld.")]
        [Range(1, int.MaxValue, ErrorMessage = "Materiaal lengte moet groter zijn dan 0.")]
        public int Length { get; set; }

        [Required(ErrorMessage = "Materiaal breedte is een verplicht veld.")]
        [Range(1, int.MaxValue, ErrorMessage = "Materiaal breedte moet groter zijn dan 0.")]
        public int Width { get; set; }

        [Required(ErrorMessage = "Materiaal dikte is een verplicht veld.")]
        [Range(1, int.MaxValue, ErrorMessage = "Materiaal dikte moet groter zijn dan 0.")]
        public int Height { get; set; }
    }
}
