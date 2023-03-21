using CuttingOptimizer.Domain;
using CuttingOptimizer.Domain.Models;

namespace CuttingOptimizer.Web.ViewModels
{
    public class PlateViewModel
    {
        public List<Plate> Plates { get; set; }
        public Saw Saw { get; set; }
    }
}
