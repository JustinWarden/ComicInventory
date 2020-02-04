using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicBookInventory.Models.ViewModels
{
    public class EditComicImageViewModel
    {
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }

        public Comic comic { get; set; }
    }
}
