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
        public TransactionGetModel()
        {
            LinkedTransactions = new List<TransactionGetModel>();
        }
        public string UserCode { get; set; }
        public string Sandwich { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }

        public IEnumerable<TransactionGetModel> LinkedTransactions { get; set; }
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
            return _db.Transactions.Where(x => x.ParentId == null).ToList().Select(x => FromModel(x));
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

            var children = _db.Transactions.Where(x => x.ParentId == transaction.Id).ToList().Select(x => FromModel(x));

            var rtn = new TransactionGetModel();
            rtn.Id = transaction.Id;
            rtn.Amount = transaction.Amount;
            rtn.Comment = transaction.Comment;
            rtn.Date = transaction.Date;
            rtn.LinkedTransactions = children;
            if (transaction.SandwichId != null)
                rtn.Sandwich = _sandwichLogic.GetById(transaction.SandwichId.Value).Name;

            rtn.Type = transaction.Type;
            rtn.UserCode = _userLogic.GetById(transaction.UserId).Code;
            return rtn;
        }
    }
}