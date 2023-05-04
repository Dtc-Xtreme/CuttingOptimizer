﻿using CuttingOptimizer.Domain.Models;
using CuttingOptimizer.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuttingOptimizer.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SawController : Controller
    {
        private readonly ISawRepository repository;

        public SawController(ISawRepository repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IList<Saw> Saws = await repository.Saws.ToListAsync();
            return Ok(Saws == null ? NotFound() : Saws);
        }

        [HttpGet("id")]
        public async Task<IActionResult> FindById(string id)
        {
            Saw? saw = await repository.FindById(id);
            return Ok(saw == null ? NotFound() : saw);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Saw saw)
        {
            return Ok(await repository.Create(saw) == false ? NotFound() : saw);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Remove(string id)
        {
            return Ok(await repository.Remove(id) == false ? NotFound() : "Saw is removed!");
        }
    }
}
