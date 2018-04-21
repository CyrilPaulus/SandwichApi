using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SandwichApi.Logic;
using SandwichApi.Models;

namespace SandwichApi.Controllers
{

    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private readonly TransactionLogic _transactionLogic;

        public TransactionController(TransactionLogic transactionLogic)
        {
            _transactionLogic = transactionLogic;
        }

        [HttpGet]
        public IEnumerable<TransactionGetModel> GetAll()
        {
            return _transactionLogic.GetAll();
        }

        [HttpGet("{id}", Name = "GetTransaction")]
        public IActionResult GetById(int id)
        {
            var transaction = _transactionLogic.FromModel(_transactionLogic.GetById(id));
            if (transaction == null)
                return NotFound();

            return new ObjectResult(transaction);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TransactionPostModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var transaction = _transactionLogic.FromModel(_transactionLogic.Create(model));
            return CreatedAtRoute("GetTransaction", new { id = transaction.Id }, transaction);
        }
    }
}