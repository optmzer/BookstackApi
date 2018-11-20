using BookstackApi.Data.Models;
using System;
using System.Collections.Generic;

namespace BookstackApi.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string CoverUrl { get; set; } = "defaultCover";
        public string Title { get; set; } = "unknown";
        public string Author { get; set; } = "unknown";
        public int YearPublished { get; set; } = 0;
        public string ISBN { get; set; } = "unknown";
        public string BookReview { get; set; } = "Author of the post did not write any reviews";
        // Book rating
        public int BookRating { get; set; } = 0;

        // List of comments
        public virtual IEnumerable<BookComment> ListComments { get; set; }
        // List of Tags
        public virtual IEnumerable<BookTag> BookTags { get; set; }
    }
}
