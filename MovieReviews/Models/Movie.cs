using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieReviews.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(40, ErrorMessage="Movie Title can only be 40 characters")]
        [Display(Name = "Movie Title")]
        public string Title { get; set; }
        [Display(Name = "Release Year")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Must be a valid year")]
        public string year { get; set; }
        public string Director { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}