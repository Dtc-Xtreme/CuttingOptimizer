using CuttingOptimizer.Domain.Models;
using CuttingOptimizer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuttingOptimizer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuotationController : Controller
    {
        private readonly IQuotationRepository repository;

        public QuotationController(IQuotationRepository repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<Quotation> quotes = await repository.Quotes.ToListAsync();
            return Ok(quotes == null ? NotFound() : quotes);
        }

        [HttpGet("id")]
        public async Task<IActionResult> FindById(int id)
        {
            Quotation? quotation = await repository.FindById(id);
            return Ok(quotation == null ? NotFound() : quotation);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Quotation quotation)
        {
            return Ok(await repository.Create(quotation) == false ? NotFound() : quotation);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Remove(int id)
        {
            return Ok(await repository.Remove(id) == false ? NotFound() : "Quotation is removed!");
        }
    }
}
