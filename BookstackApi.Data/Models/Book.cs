using BookstackApi.Data.Models;
using System;
using System.Collections.Generic;

namespace BookstackApi.Data.Models
{
    public class Book
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string CoverUrl { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int YearPublished { get; set; }
        public string ISBN { get; set; }
        public string BookReview { get; set; }
        // Book rating
        public int BookRating { get; set; }

        // List of comments
        public virtual IEnumerable<BookComment> ListComments { get; set; }
        // List of Tags
        public virtual IEnumerable<BookTag> BookTags { get; set; }
    }
}
