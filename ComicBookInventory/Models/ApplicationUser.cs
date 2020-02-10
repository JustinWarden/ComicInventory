using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComicBookInventory.Models
{
    public class ApplicationUser : IdentityUser
    {
       public virtual ICollection<Comic> Comics { get; set; }

        public virtual ICollection<Wishlist> Wishlist { get; set; }

    }
}
