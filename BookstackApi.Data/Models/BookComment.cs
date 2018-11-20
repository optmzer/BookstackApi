using System;

namespace BookstackApi.Data.Models
{
    public class BookComment
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string Author { get; set; } = "unknown";
        public string Content { get; set; } = "default empty message";
    }
}
