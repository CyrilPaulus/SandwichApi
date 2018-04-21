using System;

namespace SandwichApi.Models
{

    public enum TransactionType {
        // This user bought some sandwiches
        Order,
        // This user ordered a sandwich
        Sandwich,
        // This user added some money in the balance
        Credit,
        // This user took some money out of the balande
        Debit
    }
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int? SandwichId { get; set; }
        public int? ParentId { get; set; }
        public TransactionType Type {get; set;}

        public User User { get; set; }
        public Sandwich Sandwich { get; set; }
        public Transaction Parent {get; set;}

    }
}