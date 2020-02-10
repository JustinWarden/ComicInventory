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
using Microsoft.AspNetCore.Authorization;

namespace ComicBookInventory.Controllers
{
    public class WishlistsController : Controller
    {
         
        private readonly ApplicationDbContext _context;
        // Private field to store user manager
        private readonly UserManager<ApplicationUser> _userManager;
        public WishlistsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // Private method to get current user
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Wishlists
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //make sure the user is logged in and get current user
            ApplicationUser loggedInUser = await GetCurrentUserAsync();

            //when the user is logged in populate the wishlist index with comics order by publisher
            List<Wishlist> wishlists = await _context.Wishlist.Where(w => w.User == loggedInUser).OrderBy(w => w.Publisher).ToListAsync();
          
            //return the index view of the wishlist inventory
            return View(wishlists);
        }

        // GET: Wishlists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //if the user has no id return not found
            if (id == null)
            {
                return NotFound();
            }
            //get the current user 
            var user = await GetCurrentUserAsync();

            //popluate the wishlist inventory by user
            var wishlist = await _context.Wishlist
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            //if user has no wishlist return not found
            if (wishlist == null)
            {
                return NotFound();
            }

            //return the view of the wishlist index
            return View(wishlist);
        }

        // GET: Wishlists/Create
        [Authorize]
        //authorize that the user is logged in. creates the form for wishlist comics 
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Wishlists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Title,IssueNumber,Publisher,Year,VolumeNumber,Price,Notes,ComicImage")] Wishlist wishlist)
        {
            //wishlist does not have user id so remove userid from modelstate
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                //get current user by user id.  add comic to their wishlist and save the changes
                var currentUser = await GetCurrentUserAsync();
                wishlist.UserId = currentUser.Id;
                _context.Add(wishlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wishlist.UserId);

          //return to index view of user wishlist

            return View(wishlist);
        }

        // GET: Wishlists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //if their is no user id return not found
            if (id == null)
            {
                return NotFound();
            }

        //populate the edit wishlist by id
            var wishlist = await _context.Wishlist.FindAsync(id);

            //if their is no wishlist id return not found
            if (wishlist == null)
            {
                return NotFound();
            }

            //edit the populated comic fields and return view to the index wishlist
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wishlist.UserId);
            return View(wishlist);
        }

        // POST: Wishlists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Title,IssueNumber,Publisher,Year,VolumeNumber,Price,Notes,ComicImage")] Wishlist wishlist)
        {
            //if the id does not equal the wishlist id return not found
            if (id != wishlist.Id)
            {
                return NotFound();
            }
            //wishlist does not have userid so remove userid to validate modelstate
            ModelState.Remove("UserId");
            if (ModelState.IsValid)
            {
                try
                {
                    //get current user the wishlist userid must match the current users id
                    var currentUser = await GetCurrentUserAsync();
                    wishlist.UserId = currentUser.Id;

                    //update and save the changes to wishlist comic
                    _context.Update(wishlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if wishlist id does not exist return not found
                    if (!WishlistExists(wishlist.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            //return to wishlist index view
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", wishlist.UserId);
            return View(wishlist);
        }

        // GET: Wishlists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //if no id return not found
            if (id == null)
            {
                return NotFound();
            }

            //include user into the wishlist by id
            var wishlist = await _context.Wishlist
                .Include(w => w.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            //if there is no wishlist return not found
            if (wishlist == null)
            {
                return NotFound();
            }
            //return view to the wishlist index
            return View(wishlist);
        }

        // POST: Wishlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //get wishlist comic by id. remove comic by id, and save the changes.  return by to wishlist index view
            var wishlist = await _context.Wishlist.FindAsync(id);
            _context.Wishlist.Remove(wishlist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

         //the wishlist exists and has an id 
        private bool WishlistExists(int id)
        {
            return _context.Wishlist.Any(e => e.Id == id);
        }
    }
}
