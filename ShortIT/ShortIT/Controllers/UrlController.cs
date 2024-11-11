using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ShortIT.SQLConnection;
using ShortIT.Entity;
using ShortIT.Repository;
using ShortIT.ViewModel;

namespace ShortIT.Controllers
{
    public class UrlController : Controller
    {
        private readonly Context _context;

        public UrlController(Context context)
        {
            _context = context;
        }

        private Guid GetCurrentUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.Sid));
        }

        private async Task<ShortenedURL> GetOwnedUrlByIdAsync(string id)
        {
            var shortenedURL = await _context.ShortenedURLs.FindAsync(id);
            if (shortenedURL == null || shortenedURL.OwnerId != GetCurrentUserId())
            {
                return null; // Return null if not found or user does not own it
            }
            return shortenedURL;
        }

        // GET: Url/Dashboard
        public async Task<IActionResult> DashBoard()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var currentUserId = GetCurrentUserId(); 

            var userUrls = await _context.ShortenedURLs
                                          .Where(x => x.OwnerId == currentUserId)
                                          .ToListAsync();

            return View(userUrls);
        }


        // GET: Url/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if (id == null)
                return NotFound();

            var shortenedURL = await GetOwnedUrlByIdAsync(id);
            if (shortenedURL == null)
                return RedirectToAction("Index", "Home");

            return View(shortenedURL);
        }

        
       

        // POST: Url/ShortenUrl
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShortenUrl([FromBody] ShortenVM item)
        {
            try
            {
                var iDGenerator = new IDGenerator();
                string shortId;

                // Determine short ID based on CustomUrl or SymbolsCount
                if (!string.IsNullOrEmpty(item.CustomUrl))
                {
                    shortId = item.CustomUrl;
                }
                else if (item.SymbolsCount.HasValue && item.SymbolsCount >= 5 && item.SymbolsCount <= 10)
                {
                    shortId = iDGenerator.GenerateId(item.SymbolsCount.Value);
                }
                else
                {
                    return BadRequest(new { message = "Please provide either a valid Custom URL or a Symbols Count between 5 and 10." });
                }

                // Check for URL conflict
                if (_context.ShortenedURLs.Any(x => x.ID == shortId))
                {
                    return Conflict(new { message = "The specified shortened URL already exists. Please choose another one." });
                }

                // Create the shortened URL entry, setting MaxUses if specified
                var shortenedURL = new ShortenedURL(shortId, item.Url)
                {
                    OwnerId = User.Identity.IsAuthenticated ? GetCurrentUserId() : Guid.Empty,
                    MaxUses = item.MaxUses 
                };

                _context.ShortenedURLs.Add(shortenedURL);
                await _context.SaveChangesAsync();

                var shortenedUrl = $"{Request.Scheme}://{Request.Host}/{shortId}";
                return Ok(new { shortenedUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred on the server. Please try again.", details = ex.Message });
            }
        }

        // GET: Url/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if (id == null)
                return NotFound();

            var shortenedURL = await GetOwnedUrlByIdAsync(id);
            if (shortenedURL == null)
                return RedirectToAction("Index", "Home");

            return View(shortenedURL);
        }

        // POST: Url/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,OwnerId,Url")] ShortenedURL shortenedURL)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if (id != shortenedURL.ID)
                return NotFound();

            // Validate ownership
            var existingUrl = await GetOwnedUrlByIdAsync(id);
            if (existingUrl == null)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                try
                {
                    existingUrl.Url = shortenedURL.Url; // Only update the URL, not the ID or OwnerId
                    await _context.SaveChangesAsync();
                    return RedirectToAction("DashBoard");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShortenedURLExists(shortenedURL.ID))
                        return NotFound();

                    throw;
                }
            }
            return View(shortenedURL);
        }

        // GET: Url/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if (id == null)
                return NotFound();

            var shortenedURL = await GetOwnedUrlByIdAsync(id);
            if (shortenedURL == null)
                return RedirectToAction("Index", "Home");

            return View(shortenedURL);
        }

        // POST: Url/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var shortenedURL = await GetOwnedUrlByIdAsync(id);
            if (shortenedURL == null)
                return RedirectToAction("Index", "Home");

            _context.ShortenedURLs.Remove(shortenedURL);
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        private bool ShortenedURLExists(string id)
        {
            return _context.ShortenedURLs.Any(e => e.ID == id);
        }

        // Route: /{id} - Redirect to the original URL
        [Route("{id}")]
        public async Task<IActionResult> RedirectToUrl(string id)
        {
            var shortenedURL = await _context.ShortenedURLs.FindAsync(id);
            if (shortenedURL == null)
                return NotFound();

            var url = shortenedURL.Url.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? shortenedURL.Url
                : "https://" + shortenedURL.Url;

            shortenedURL.OpenedTimes++;
            if (shortenedURL.OpenedTimes == shortenedURL.MaxUses)
            {
                DeleteAfterUses(shortenedURL);
            }
            _context.SaveChanges();
            return Redirect(url);
        }
        private async void DeleteAfterUses(ShortenedURL item)
        {
            _context.ShortenedURLs.Remove(item);
        }
    }
}
