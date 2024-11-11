using ShortIT.SQLConnection;

namespace ShortIT.Repository
{
    public class IDGenerator
    {
        private Random _random = new Random();
        private const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
        private Context context = new Context();
        public string GenerateId(int length)
        {
            string Id = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
            if (!context.ShortenedURLs.Any(x => x.ID == Id))
            {
                return Id;
            }
            return GenerateId(length);
        }
    }
}
