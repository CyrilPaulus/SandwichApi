using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SandwichApi.Logic;
using SandwichApi.Models;

namespace SandwichApi.Controllers
{

    [Route("api/[controller]")]
    public class SandwichController : Controller
    {        
        private readonly SandwichLogic _sandwichLogic;

        public SandwichController(SandwichLogic sandwichLogic) {
            _sandwichLogic = sandwichLogic;
        }

        [HttpGet]
        public IEnumerable<Sandwich> GetAll() {
            return _sandwichLogic.GetAll();
        }

        [HttpGet("{id}", Name="GetSandwich")]
        public IActionResult GetById(int id) {
            var sandwich = _sandwichLogic.GetById(id);
            if(sandwich == null)
                return NotFound();

            return new ObjectResult(sandwich);
        }
    }
}