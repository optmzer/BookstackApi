using System;

namespace BookstackApi.Models
{
    public class BookComment
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
    }
}
