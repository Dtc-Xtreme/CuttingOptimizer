using CuttingOptimizer.Domain.Models;
using CuttingOptimizer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuttingOptimizer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlateController : Controller
    {
        private readonly IPlateRepository repository;
        public PlateController(IPlateRepository repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<Plate> Plates = await repository.Plates.ToListAsync();
            return Ok(Plates == null ? NotFound() : Plates);
        }

        [HttpGet("id")]
        public async Task<IActionResult> FindById(string id)
        {
            Plate? plate = await repository.FindById(id);
            return Ok(plate == null ? NotFound() : plate);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Plate plate)
        {
            return Ok(await repository.Create(plate) == false ? NotFound() : plate);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Remove(string id)
        {
            return Ok(await repository.Remove(id) == false ? NotFound() : "Plate is removed!");
        }
    }
}
