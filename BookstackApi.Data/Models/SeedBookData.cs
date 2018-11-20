using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookstackApi.Data.Models
{
    public class SeedBookData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookstackApiDdContext(
                serviceProvider.GetRequiredService<DbContextOptions<BookstackApiDdContext>>()))
            {
                // Look for any movies.
                if (context.Book.Count() > 0)
                {
                    return;   // DB has been seeded
                }

                // Create a list of comments
                var bookComments = new List<BookComment>() {
                    new BookComment
                    {
                        Author = "John Doe",
                        Content = "This is very good book.",
                        Created = DateTime.Now
                    },
                    new BookComment
                    {
                        Author = "Silvia Doe",
                        Content = "Great beginners book",
                        Created = DateTime.Now
                    }
                };
                    
                // Create a list of tags
                var bookTags = new List<BookTag>() {
                    new BookTag
                    {
                        Description = "#programming"
                    },
                    new BookTag
                    {
                        Description = "#programming"
                    }
                };

                context.Book.AddRange(
                    new Book
                    {
                        Created = DateTime.Now,
                        CoverUrl = "https://books.google.co.nz/books/content?id=dewmNEwiv4sC&printsec=frontcover&img=1&zoom=5&edge=curl&imgtk=AFLRE73hd9bSvdmdldCmKBOcF77G6fV-KKmS-48vsTZiQADaRBDjTWHn2PAjusyizNcwgW3-p4hSt6fyHbPbB_2a9SypGxXE_-rpsOb08XbgCDON0WegohIpVpFVk4DXtjV4yOyQBPGW",
                        Title = "Head First C#",
                        Author = "Andrew Stellman",
                        YearPublished = 2013,
                        ISBN = "9781449358846",
                        BookReview = "C# is a general purpose, object-oriented, component-based programming language. As a general purpose language, there are a number of ways to apply C# to accomplish many different tasks.",
                        BookRating = 5,
                        BookTags = bookTags,
                        ListComments = bookComments
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
