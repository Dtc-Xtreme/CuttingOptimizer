using CuttingOptimizer.Domain.Models;
using CuttingOptimizer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace CuttingOptimizer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlueprintController : Controller
    {
        private readonly IQuotationRepository repository;

        public BlueprintController(IQuotationRepository repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<Blueprint> blueprints = await repository.Quotes.ToListAsync();
            return Ok(blueprints == null ? NotFound() : blueprints);
        }

        [HttpGet("id")]
        public async Task<IActionResult> FindById(int id)
        {
            Blueprint? blueprint = await repository.FindById(id);
            return Ok(blueprint == null ? NotFound() : blueprint);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blueprint blueprint)
        {
            return Ok(await repository.Create(blueprint) == false ? NotFound() : blueprint);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Blueprint blueprint)
        {
            return Ok(await repository.Update(blueprint) == false ? NotFound() : blueprint);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Remove(int id)
        {
            return Ok(await repository.Remove(id) == false ? NotFound() : "Blueprint " + id + " is removed!");
        }
    }
}
