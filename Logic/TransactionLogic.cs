using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SandwichApi.Models;

namespace SandwichApi.Logic
{

    public class TransactionPostModel
    {
        public TransactionPostModel()
        {
            LinkedTransactions = new List<TransactionPostModel>();
        }

        [Required]
        public string UserCode { get; set; }
        public string Sandwich { get; set; }
        [Required]
        public TransactionType Type { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
        public IEnumerable<TransactionPostModel> LinkedTransactions { get; set; }
        public string Comment { get; set; }
    }

    public class TransactionGetModel
    {        
        public string UserCode { get; set; }
        public string Sandwich { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public int? ParentId {get; set;}
        public int Id { get; internal set; }
    }

    public class TransactionLogic
    {
        private readonly SandwichContext _db;

        public readonly UserLogic _userLogic;

        private readonly SandwichLogic _sandwichLogic;

        public TransactionLogic(
            SandwichContext db,
            UserLogic userLogic,
            SandwichLogic sandwichLogic
            )
        {
            _db = db;
            _userLogic = userLogic;
            _sandwichLogic = sandwichLogic;
        }

        public IQueryable<Transaction> GetTransactions() {
            return _db.Transactions.AsQueryable();
        }
        public IEnumerable<TransactionGetModel> GetAll()
        {

            return from transaction in _db.Transactions
                    join user in _db.Users on transaction.UserId equals user.Id
                    select new TransactionGetModel() {
                        Id = transaction.Id,
                        ParentId = transaction.ParentId,
                        Type = transaction.Type,
                        Amount = transaction.Amount,
                        Comment = transaction.Comment,
                        Date = transaction.Date,
                        Sandwich = transaction.Sandwich.Name,
                        UserCode = transaction.User.Code
                    };
            
        }

        public Transaction GetById(int id)
        {
            return _db.Transactions.Find(id);
        }

        public Transaction Create(TransactionPostModel model, Transaction parent = null, bool commit = true)
        {
            var transaction = new Transaction();
            transaction.Amount = model.Amount;
            transaction.Comment = model.Comment;
            transaction.Date = model.Date ?? DateTime.UtcNow;
            transaction.Type = model.Type;
            transaction.User = _userLogic.GetCreate(model.UserCode, true);

            if (!string.IsNullOrEmpty(model.Sandwich))
                transaction.Sandwich = _sandwichLogic.GetCreate(model.Sandwich, true);

            if (parent != null)
                transaction.Parent = parent;

            _db.Transactions.Add(transaction);

            foreach (var linkedModel in model.LinkedTransactions)
                Create(linkedModel, transaction, false);

            if (commit)
                _db.SaveChanges();

            return transaction;
        }

        public TransactionGetModel FromModel(Transaction transaction)
        {
            if (transaction == null)
                return null;

          

            var rtn = new TransactionGetModel();
            rtn.Id = transaction.Id;
            rtn.Amount = transaction.Amount;
            rtn.Comment = transaction.Comment;
            rtn.Date = transaction.Date;
            rtn.ParentId = transaction.ParentId;
            rtn.Type = transaction.Type;
            rtn.UserCode = _userLogic.GetById(transaction.UserId).Code;
            return rtn;
        }
    }
}