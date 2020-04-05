using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp_Male.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public String BookName { get; set; }
        public int PublishYear { get; set; }
        public int PaperCount { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
