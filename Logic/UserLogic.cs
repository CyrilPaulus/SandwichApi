using System;
using System.Linq;
using SandwichApi.Models;

namespace SandwichApi.Logic
{
    public class UserLogic
    {
        private readonly SandwichContext _db;

        public UserLogic(SandwichContext db)
        {
            _db = db;
        }

        public IQueryable<User> GetUsers()
        {
            return _db.Users;
        }


        public User GetById(int id)
        {
            return _db.Users.Find(id);
        }

        public User AddUser(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }

        public User GetCreate(string code, bool commit = true)
        {
            var user = _db.Users.FirstOrDefault(x => x.Code == code);
            if (user == null)
            {
                user = new User();
                user.Code = code;
                _db.Users.Add(user);

                if (commit)
                    _db.SaveChanges();
            }

            return user;
        }

        internal void Update(User oldUser, User newUser)
        {
            oldUser.Code = newUser.Code;
            oldUser.FirstName = newUser.FirstName;
            oldUser.LastName = newUser.LastName;
            oldUser.Type = newUser.Type;
            _db.SaveChanges();

        }

        internal void Delete(User user)
        {
            _db.Users.Remove(user);
            _db.SaveChanges();
        }
    }
}