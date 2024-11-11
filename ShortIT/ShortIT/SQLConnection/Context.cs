using ShortIT.Entity;
using System.Data.Entity;


namespace ShortIT.SQLConnection
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ShortenedURL> ShortenedURLs { get; set; }

        public Context() : base("Server = localhost\\sqlexpress; Database=ShortIT;Trusted_Connection=True;")

        {
            Users = this.Set<User>();
            ShortenedURLs = this.Set<ShortenedURL>();
            
        }
    }
}
