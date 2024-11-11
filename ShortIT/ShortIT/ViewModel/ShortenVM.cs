using System.ComponentModel.DataAnnotations;

namespace ShortIT.ViewModel
{
    public class ShortenVM
    {
        [Required(ErrorMessage = "URL is required")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        [StringLength(2048, ErrorMessage = "URL is too long. Maximum 2048 characters allowed.")]
        public string Url { get; set; }

        [Range(5, 10, ErrorMessage = "Symbols count must be between 5 and 10")]
        public int? SymbolsCount { get; set; }

        [StringLength(20, ErrorMessage = "Custom URL is too long. Maximum 20 characters allowed.")]
        public string CustomUrl { get; set; }

        public int MaxUses { get; set; }
    }
}
