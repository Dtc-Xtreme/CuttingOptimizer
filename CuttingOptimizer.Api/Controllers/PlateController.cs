using CuttingOptimizer.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CuttingOptimizer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlateController : Controller
    {
        private IList<Plate> Plates;

        public PlateController()
        {
            Plates = new List<Plate>
            {
                new Plate("PLC2000/1300/5",2000,1300,5),
                new Plate("PLC3000/1900/10",3000,1900,10),
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(Plates == null ? NotFound() : Ok(Plates));
        }

        [HttpGet("name")]
        public async Task<IActionResult> FindByName(string name)
        {
            IList<Plate> resultList = Plates.Where(c => c.ID.ToLower().Contains(name.ToLower())).ToList();
            if (resultList.Count == 0) resultList = null;
            return resultList == null ? NotFound() : Ok(resultList);
        }
    }
}
