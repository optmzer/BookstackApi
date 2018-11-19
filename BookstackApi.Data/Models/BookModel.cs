using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookstackApi.Models
{
    public class BookModel
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
        public virtual IEnumerable<CommentModel> ListComments { get; set; }
    }
}
