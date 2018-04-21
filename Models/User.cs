using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SandwichApi.Models
{

    public enum UserType
    {
        Internal,
        External,
        Student,
        Other
    }
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }        
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        [Required]
        public UserType Type { get; set; }

        public List<Transaction> Transactions {get; set;}
    }
}