using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieReviews.Models
{
    public class Review
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string ApplicationUserId { get; set; }
        [Required]
        [MinLength(4, ErrorMessage = "Must have a valid title")]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [MinLength(60, ErrorMessage = "Reviews must be at least 60 characters")]
        public string Body { get; set; }
        public Movie Movie { get; set; }
        public int MovieId { get; set; }
    }
}