using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SandwichApi.Logic;
using SandwichApi.Models;

namespace SandwichApi.Controllers
{

    [Route("api/[controller]")]
    public class UserController : Controller
    {        
        private readonly UserLogic _userLogic;
        private readonly TransactionLogic _transactionLogic;

        public UserController(
            UserLogic userLogic,
            TransactionLogic transactionLogic) {
            _userLogic = userLogic;
            _transactionLogic = transactionLogic;
        }

        [HttpGet]
        public IEnumerable<UserBalanceViewModel> GetAll() {        

            return _userLogic.GetUsers().Select(x => new UserBalanceViewModel() {
                Id = x.Id,
                Code = x.Code,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Type = x.Type,
                Balance = x.Transactions.Sum(y => y.Amount)
            });
            
        }

        [HttpGet("{id}", Name="GetUser")]
        public IActionResult GetById(int id) {
            var user = _userLogic.GetById(id);
            if(user == null)
                return NotFound();

            return new ObjectResult(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody]User user) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = _userLogic.AddUser(user);
            return CreatedAtRoute("GetUser", new {id = newUser.Id}, user);
        }

        [HttpPut("{id}", Name="UpdateUser")]
        public IActionResult Update(int id, [FromBody]User user) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var oldUser = _userLogic.GetById(id);
            _userLogic.Update(oldUser, user);
            return CreatedAtRoute("GetUser", new {id = id}, user);
        }   

        [HttpDelete("{id}", Name="DeleteUser")]
        public IActionResult Delete(int id) {

            var user = _userLogic.GetById(id);
            _userLogic.Delete(user);
            return Ok();
        }
    }


    public class UserBalanceViewModel
    {
        public int Id {get; set;}
        public string Code {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public UserType Type {get; set;}
        public decimal Balance {get; set;}
    }
}

