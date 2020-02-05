using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ComicBookInventory.Data;
using ComicBookInventory.Models;
using Microsoft.AspNetCore.Identity;
using ComicBookInventory.Models.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace ComicBookInventory.Controllers
{
    public class ComicsController : Controller
    {
 
        private readonly ApplicationDbContext _context;
        // Private field to store user manager
        private readonly UserManager<ApplicationUser> _userManager;
        public ComicsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // Private method to get current user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        // GET: Comics
        [Authorize]
        public async Task<IActionResult> Index(string searchQuery)
               
        {

            ApplicationUser loggedInUser = await GetCurrentUserAsync();

            List<Comic> comics = await _context.Comics.Where(c => c.User == loggedInUser).OrderBy(c => c.Publisher).ToListAsync();
          

            if (searchQuery != null)
            {
                comics = comics.Where(comic => comic.Title.ToLower().Contains(searchQuery) || 
                comic.Publisher.ToLower().Contains(searchQuery)).ToList();
                    
                    
            }


            return View(comics);
        }
     
        // GET: Comics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await GetCurrentUserAsync();

            var comic = await _context.Comics
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id && c.User == user);

            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        // GET: Comics/Create
        [Authorize]
        public IActionResult Create()
        {
            ComicImageViewModel comicImageViewModel = new ComicImageViewModel();
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View(comicImageViewModel);
        }

        // POST: Comics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        //public async Task<IActionResult> Create(ComicImageViewModel comicImageViewModel);
        public async Task<IActionResult> Create( ComicImageViewModel comicImageViewModel)
        {
            ModelState.Remove("comic.UserId");
            if (ModelState.IsValid)
            {

                var user = await GetCurrentUserAsync();

                if (comicImageViewModel.ImageFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                   {
                        await comicImageViewModel.ImageFile.CopyToAsync(memoryStream);
                        comicImageViewModel.comic.ComicImage = memoryStream.ToArray();
                    }
                };

                comicImageViewModel.comic.UserId = user.Id;
                _context.Add(comicImageViewModel.comic);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = comicImageViewModel.comic.Id.ToString()});
            }

            return View(comicImageViewModel);
        }

        // GET: Comics/Edit/5   ORIGINAL!!!!

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comic = await _context.Comics
                .FindAsync(id);

            if (comic == null)
            {
                return NotFound();
            }
            var viewModel = new EditComicImageViewModel()
            {
                comic = comic
            };
           
          return View(viewModel);
        }
        // POST: Comics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditComicImageViewModel viewModel)
        {
            var comicWithImage = await _context.Comics.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (id != viewModel.comic.Id)
            {
                return NotFound();
            }
            ModelState.Remove("comic.UserId");
            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await GetCurrentUserAsync();
                    if (viewModel.ImageFile == null && comicWithImage.ComicImage != null)
                    {
                        viewModel.comic.ComicImage = comicWithImage.ComicImage;
                    }
                    else
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await viewModel.ImageFile.CopyToAsync(memoryStream);
                            viewModel.comic.ComicImage = memoryStream.ToArray();
                        }
                    }
                    ;
                    viewModel.comic.UserId = currentUser.Id;
                    _context.Update(viewModel.comic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComicExists(viewModel.comic.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
            }
            return View(viewModel);
        }

        // GET: Comics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
          
            if (id == null)
            {
                return NotFound();
            }

            var comic = await _context.Comics
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        // POST: Comics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comic = await _context.Comics.FindAsync(id);
            _context.Comics.Remove(comic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComicExists(int id)
        {
            return _context.Comics.Any(e => e.Id == id);
        }
    }
}
