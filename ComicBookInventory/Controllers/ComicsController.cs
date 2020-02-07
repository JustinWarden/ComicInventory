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
            //this will make sure the user is logged in before they can access any of the inventory content
            ApplicationUser loggedInUser = await GetCurrentUserAsync();

            //once user is logged in create a list of comics and order them publisher
            List<Comic> comics = await _context.Comics.Where(c => c.User == loggedInUser).OrderBy(c => c.Publisher).ToListAsync();
          
            //this is the logic to run the search bar.  the search bar can search by title or publisher in lowercase
            if (searchQuery != null)
            {
                comics = comics.Where(comic => comic.Title.ToLower().Contains(searchQuery) || 
                comic.Publisher.ToLower().Contains(searchQuery)).ToList();
                
            }
         //bring back the view we want to see (comics)
            return View(comics);
        }
     
        // GET: Comics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //check to see if the comic has an id if not return not found
            if (id == null)
            {
                return NotFound();
            }
            //get the current user to populate their inventory
            var user = await GetCurrentUserAsync();

            var comic = await _context.Comics
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id && c.User == user);

            //if no comic if found return not found
            if (comic == null)
            {
                return NotFound();
            }
            //bring back the view we want to see (comics) by id
            return View(comic);
        }

        // GET: Comics/Create

            //user must be authorized (signed in) to create a new comic in the database
        [Authorize]
        public IActionResult Create()
        {
            //this will create the entry fields set up in the comic model
            ComicImageViewModel comicImageViewModel = new ComicImageViewModel();
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");

            //i used a view model to add images to each comic
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
            //the comicimageviewmodel does not contain a userid so we need to remove it to make modelstate valid
            ModelState.Remove("comic.UserId");
            if (ModelState.IsValid)
            {
                //get the current user
                var user = await GetCurrentUserAsync();

                //this will add and image to the comic database
                if (comicImageViewModel.ImageFile != null)
                {
                    using (var memoryStream = new MemoryStream())
                   {
                        await comicImageViewModel.ImageFile.CopyToAsync(memoryStream);
                        comicImageViewModel.comic.ComicImage = memoryStream.ToArray();
                    }
                };

                //add the user id to the comic posted to inventory
                comicImageViewModel.comic.UserId = user.Id;
                _context.Add(comicImageViewModel.comic);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = comicImageViewModel.comic.Id.ToString()});
            }

            //return the view of comicimageviewmodel
            return View(comicImageViewModel);
        }

        // GET: Comics/Edit/5
        //edit comic by comic id
        public async Task<IActionResult> Edit(int? id)
        {
            //if no comic id return notfound
            if (id == null)
            {
                return NotFound();
            }
            //get the comic id
            var comic = await _context.Comics
                .FindAsync(id);

            if (comic == null)
            {
                return NotFound();
            }
            //use the editcomicimageviewmodel to be able to add a new image to comic 
            var viewModel = new EditComicImageViewModel()
            {
                comic = comic
            };
           //return the view of the viewmodel
          return View(viewModel);
        }

        // POST: Comics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditComicImageViewModel viewModel)
        {
            //use the editcomicimageviewmodel to edit a comic with an image get the comic id
            var comicWithImage = await _context.Comics.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            //if comic id cant be found return not found
            if (id != viewModel.comic.Id)
            {
                return NotFound();
            }
            //remove the userid because the editcomicimageviewmodel does not have userid
            ModelState.Remove("comic.UserId");
            if (ModelState.IsValid)
            {
                try
                {
                    //get the current user
                    var currentUser = await GetCurrentUserAsync();

                  //check to see if the comic in inventory has an image attached view the image in the edit page
                    if (viewModel.ImageFile == null && comicWithImage.ComicImage != null)
                    {
                        viewModel.comic.ComicImage = comicWithImage.ComicImage;
                    }

                    //if no image add one to the comic inventory
                    else
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await viewModel.ImageFile.CopyToAsync(memoryStream);
                            viewModel.comic.ComicImage = memoryStream.ToArray();
                        }
                    }
                    ;
                    //add the user id, update the new image and data, and save the changes
                    viewModel.comic.UserId = currentUser.Id;
                    _context.Update(viewModel.comic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if no comic exists with a matching id in the inventory return not found
                    if (!ComicExists(viewModel.comic.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //if comic edit page is updated properly and submited return user back to comic inventory index
                return RedirectToAction(nameof(Index));
            }
            else
            {
            }
            //if comic is not updated properly return view of the edit comic by id page
            return View(viewModel);
        }

        // GET: Comics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
          //make sure comic has an id befor delete if not return not found
            if (id == null)
            {
                return NotFound();
            }
            //make sure the comic and user have an id for their inventory before delete or return not found
            var comic = await _context.Comics
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (comic == null)
            {
                return NotFound();
            }

            //if no comic found return the comic view of index inventory
            return View(comic);
        }

        // POST: Comics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        //confirm with user that they are sure they want to delete comic from inventory
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //remove comic from inventory list save the changes return to index view
            var comic = await _context.Comics.FindAsync(id);
            _context.Comics.Remove(comic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //if the comic exists return the comic with matching id
        private bool ComicExists(int id)
        {
            return _context.Comics.Any(e => e.Id == id);
        }
    }
}
