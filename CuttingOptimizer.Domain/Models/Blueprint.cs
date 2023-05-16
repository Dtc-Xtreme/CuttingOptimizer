using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingOptimizer.Domain.Models
{
    public class Blueprint
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string JsonString { get; set; }
    }
}
