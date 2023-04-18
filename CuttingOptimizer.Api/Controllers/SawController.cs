using CuttingOptimizer.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CuttingOptimizer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SawController : Controller
    {
        private IList<Saw> Saws;

        public SawController()
        {
            Saws = new List<Saw>
            {
                new Saw("IS5",5),
                new Saw("IS10",10),
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(Saws == null ? NotFound() : Ok(Saws));
        }

        [HttpGet("name")]
        public async Task<IActionResult> FindByName(string name)
        {
            IList<Saw> resultList = Saws.Where(c => c.ID.ToLower().Contains(name.ToLower())).ToList();
            if(resultList.Count == 0) resultList = null;
            return resultList == null ? NotFound() : Ok(resultList);
        }
    }
}
