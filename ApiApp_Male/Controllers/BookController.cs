using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiApp_Male.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiApp_Male.Controllers
{
    [Route("api/Author/{AuthorId}/Book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public static List<Book> BookList = new List<Book>() {
        new Book(){ BookId=1,BookName="Computer Information",PaperCount=100,PublishYear=2000,AuthorId=10},
        new Book(){ BookId=2,BookName="Database Introduction",PaperCount=150,PublishYear=2020,AuthorId=10},
        new Book(){ BookId=3,BookName="Data Structured",PaperCount=120,PublishYear=2005,AuthorId=20},
        new Book(){ BookId=4,BookName="Operating Systems",PaperCount=50,PublishYear=2008,AuthorId=30},
        new Book(){ BookId=5,BookName="Asp Net Core 3 API",PaperCount=70,PublishYear=2010,AuthorId=30},
        };

        [HttpGet]
        public IActionResult AllAuthorBooks(int AuthorId)
        {
            return Ok(BookList.Where(x=>x.AuthorId==AuthorId).ToList());
        }

        [HttpGet("{BookId}",Name ="SingleBookRoute")]
        public IActionResult GetBookById(int AuthorId,int BookId)
        {

            var CurBook = BookList.Where(x => x.AuthorId == AuthorId && x.BookId == BookId).SingleOrDefault();
            if (CurBook == null)
                return NotFound("Book Not Found");

            return Ok(CurBook);

        }

        [HttpPost]
        public IActionResult AddBook(int AuthorId,Book NewBook)
        {
            //validation
           /*if(! AuthorController.Authors.Any(x => x.AuthorId == AuthorId))
            {
                return NotFound("Author is not exists");
            }*/
            if (AuthorId != NewBook.AuthorId)
                return BadRequest("invalid Author Id");

            if (BookList.Any(x => x.AuthorId == AuthorId && x.BookId== NewBook.BookId))
            {
                return Conflict("Book Is Already Exists");
            }
            BookList.Add(NewBook);

            return CreatedAtRoute("SingleBookRoute", new { AuthorId = AuthorId, BookId = NewBook.BookId },
                   NewBook);
                /*CreatedAtAction(nameof(GetBookById), new { AuthorId = AuthorId, BookId = NewBook.BookId },
                   NewBook);*/
        }

        [HttpDelete("{BookId}")]
        public IActionResult DeleteBook(int AuthorId,int BookId)
        {
         //   MajorController.Majors.find(x=>x.MajorId=1).majorName
          /*  if (!AuthorController.Authors.Any(x => x.AuthorId == AuthorId))
            {
                return NotFound("Author is not exists");
            }*/

            var CurBook = BookList.Where(x => x.AuthorId == AuthorId && x.BookId == BookId).SingleOrDefault();
            if(CurBook==null)
            {
                return NotFound("Book Is not Exists");
            }
            BookList.Remove(CurBook);

            return NoContent();
        }
    }
}