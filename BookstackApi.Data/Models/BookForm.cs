using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstackApi.Models
{
    public class BookForm
    {
        public string Title { get; set; } = "unknown";
        public string Author { get; set; } = "unknown";
        public int YearPublished { get; set; } = 0;
        public string ISBN { get; set; } = "unknown";
        public string BookReview { get; set; } = "Author of the post did not write any reviews";
        // Book rating
        public int BookRating { get; set; } = 0;
        // Image
        public IFormFile Image { get; set; }
    }
}
