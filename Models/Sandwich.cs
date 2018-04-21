namespace SandwichApi.Models
{
    public class Sandwich
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal LastKnownPrice {get; set;}
    }
}