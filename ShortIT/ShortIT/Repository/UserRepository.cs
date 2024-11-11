using Scrypt;
using System;
using ShortIT.SQLConnection;
using ShortIT.Entity;

namespace ShortIT.Repository
{
    public class UserRepository
    {
        public User FindByUsernameAndPassword(string username, string password)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            using (var context = new Context())
            {
                foreach (var user in context.Users)
                {
                    if (user.Username.Equals(username) && encoder.Compare(password, user.Password))
                    {
                        return user;
                    }
                }
            }
            return null;
        }
        public bool findByUsername(string username)
        {
            using (var context = new Context())
            {
                return context.Users.Any(x => x.Username == username);
            }
        }
        public void Add(User item)
        {
            using (var context = new Context())
            {
                context.Users.Add(item);
                context.SaveChanges();
            }
        }
    }
}
