using SandwichApi.Models;
using System.Linq;

namespace SandwichApi.Logic
{
    public class SandwichLogic
    {
        private readonly SandwichContext _db;

        public SandwichLogic(SandwichContext db)
        {
            _db = db;
        }

        public IQueryable<Sandwich> GetAll()
        {
            return _db.Sandwiches;
        }

        public Sandwich GetById(int id)
        {
            return _db.Sandwiches.Find(id);
        }

        public Sandwich GetCreate(string name, bool commit = true)
        {
            var sandwich = _db.Sandwiches.FirstOrDefault(x => x.Name == name);
            if (sandwich == null)
            {
                sandwich = new Sandwich();
                sandwich.Name = name;
                _db.Sandwiches.Add(sandwich);

                if (commit)
                    _db.SaveChanges();
            }

            return sandwich;

        }
    }
}