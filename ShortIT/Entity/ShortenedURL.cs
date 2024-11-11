using System.ComponentModel.DataAnnotations;

namespace ShortIT.Entity
{
    public class ShortenedURL
    {
        [Key]
        public string ID { get; set; }
        public Guid OwnerId { get; set; }
        public string Url { get; set; }
        public int MaxUses { get; set; }
        public int OpenedTimes { get; set; }
        public ShortenedURL()
        {

        }
        public ShortenedURL(string iD, string url)
        {
            ID = iD;
            Url = url;
            OpenedTimes = 0;
        }
    }
}
