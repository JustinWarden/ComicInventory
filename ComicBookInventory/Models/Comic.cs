using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ComicBookInventory.Models
{

    public class Comic
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string Title { get; set; }

        public int? IssueNumber { get; set; }

        public string Publisher { get; set; }

        public int Year { get; set; }

        public int VolumeNumber { get; set; }

        public double Price { get; set; }


        [StringLength(55, ErrorMessage = "Please shorten the Notes to 55 characters")]
        public string Notes { get; set; }


        [Display(Name = "Comic Image")]
        public byte[] ComicImage { get; set; }

    }

}
