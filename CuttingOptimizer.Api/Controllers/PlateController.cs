using CuttingOptimizer.Api.Models;
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
            IList<Plate> plates = await repository.Plates.ToListAsync();
            return Ok(plates == null ? NotFound() : plates);
        }

        [HttpGet("id")]
        public async Task<IActionResult> FindById(string id)
        {
            Plate? plate = await repository.FindById(id);
            return Ok(plate == null ? NotFound() : plate);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlateDTO plate)
        {
            Plate newPlate = new Plate
            {
                ID = plate.ID,
                Length = plate.Length,
                Width = plate.Width,
                Height = plate.Height
            };
            return Ok(await repository.Create(newPlate) == false ? NotFound() : newPlate);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Remove(string id)
        {
            return Ok(await repository.Remove(id) == false ? NotFound() : "Plate is removed!");
        }
    }
}
