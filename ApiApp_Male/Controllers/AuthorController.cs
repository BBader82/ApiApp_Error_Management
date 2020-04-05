using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ApiApp_Male.helper;
using ApiApp_Male.Models;
using ApiApp_Male.Models.RequestDTO;
using ApiApp_Male.Models.ResponseDTO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ApiApp_Male.Controllers
{
    [Route("api/Author")] //api/author?fdgdfgdfg
    [ApiController]
    public class AuthorController : ControllerBase
    {
               
        private readonly IMapper mapper;
        private readonly LibraryContext dbContext;
        private readonly ErrorClass error;
        

        public AuthorController(IMapper Mapper,
                                LibraryContext dbContext,
                                ErrorClass error)
        {
            mapper = Mapper;
            this.dbContext = dbContext;
            this.error = error;
            
        }
        [HttpGet(Name = "GetAllAuthors")]
        public IActionResult GetAllAuthors(//[FromQuery]String FilterAuthorName,String FilterLocation
                                            [FromQuery] String SearchingString,
                                            String orderby,
                                            [FromQuery] PagingDTO paging)
        {
           
            var AuthorQuery= dbContext.Author.AsQueryable();
            AuthorQuery = AuthorQuery.Where(x => x.IsDeleted == false);
            
            /* if(!String.IsNullOrWhiteSpace(FilterAuthorName))
             {
                 AuthorQuery = AuthorQuery.Where(x => x.AuthorName.Equals(FilterAuthorName));
             }

             if (!String.IsNullOrWhiteSpace(FilterLocation))
             {
                 AuthorQuery = AuthorQuery.Where(x => x.Location.Contains(FilterLocation,StringComparison.OrdinalIgnoreCase));
             }*/
            if (!String.IsNullOrWhiteSpace(SearchingString))
            {
                AuthorQuery = AuthorQuery.Where(x => x.AuthorName.Equals(SearchingString) ||
                                                   x.Location.Contains(SearchingString, StringComparison.OrdinalIgnoreCase));
            }

            if (!String.IsNullOrWhiteSpace(orderby))
            {
                AuthorQuery = AuthorQuery.OrderBy(orderby);
            }
            //  else
            //    AuthorQuery = AuthorQuery.OrderBy(x=>x.AuthorName);

            //Paging Details
            /* var PDetails = new PagingDetails();

             PDetails.TotalRows = AuthorQuery.Count();
             PDetails.TotalPages = (int) Math.Ceiling((double) PDetails.TotalRows / paging.RowCount);
             PDetails.CurPage = paging.PageNumber;
             PDetails.HasNextPage = PDetails.CurPage < PDetails.TotalPages;
             PDetails.HasPrevPage = PDetails.CurPage>1;*/
            var ResponseDTO=AuthorQuery.Select(p => mapper.Map<AuthorResponseDTO>(p));

            var pagedResponse = new PagedResponse<AuthorResponseDTO>(ResponseDTO, paging);

            if (pagedResponse.Paging.HasNextPage)
                pagedResponse.Paging.NextPageURL= Url.Link("GetAllAuthors", new {
                                          SearchingString,
                                          orderby,
                                          paging.RowCount,
                                          PageNumber=paging.PageNumber+1
                                        });
            if (pagedResponse.Paging.HasPrevPage)
                pagedResponse.Paging.PrevPageURL = Url.Link("GetAllAuthors", new
                {
                    SearchingString,
                    orderby,
                    paging.RowCount,
                    PageNumber = paging.PageNumber - 1
                });

            //Response.Headers.Add("X-Paging",JsonConvert.SerializeObject(PDetails));


            // AuthorQuery =AuthorQuery.Skip((paging.PageNumber - 1) * paging.RowCount).
            //              Take(paging.RowCount);

            // return  Ok(AuthorQuery.Select(p=> mapper.Map<AuthorResponseDTO>(p)));
            return Ok(pagedResponse); 
        }

        [HttpGet("{Id}")] //Api/Author/
        public IActionResult GetAuthorById(int Id)
        {
            Author CurAuthor = dbContext.Author.Where(x => x.AuthorId == Id && x.IsDeleted == false).FirstOrDefault();
            if (CurAuthor == null)
            {
                error.LoadError("Ath001");
                ModelState.AddModelError(error.ErrorProp, error.ErrorMessage);
                return ValidationProblem();
            }
                          

            return Ok(mapper.Map<AuthorResponseDTO>(CurAuthor));
                
        }

        [HttpPost]
        public IActionResult AddAuthor([FromBody]AuthorAddRequestDTO NewAuthor)
        {

            //check Author Name duplication

            if (dbContext.Author.Any(x => x.AuthorName == NewAuthor.AuthorName))
            {
                error.LoadError("Ath002");
                ModelState.AddModelError(error.ErrorProp, error.ErrorMessage);
                return ValidationProblem();

            }
                //return BadRequest("Duplicate Name");

            /*if (!ModelState.IsValid)
                //  return BadRequest(ModelState);
                return ValidationProblem();

          /*  if (String.IsNullOrEmpty(NewAuthor.AuthorName))
                return BadRequest(new
                {
                    ErrorCode = 501,
                    ErrorMessage = "Invalid Empty Name"
                });*/

            /*var curauthor = Authors.Where(x => x.AuthorId == NewAuthor.AuthorId && x.IsDeleted == false).SingleOrDefault();
            if (curauthor != null)
                return Conflict(new
                {
                    ErrorCode = 502,
                    ErrorMessage = "Duplicate Author Id"
                });*/

            /*  Author CurAuthor = new Author() { 
                  AuthorId= Authors.Max(x=>x.AuthorId)+1,
                  AuthorName=NewAuthor.AuthorName,
                  Location=NewAuthor.Location,
                  IsDeleted=false,
                  Books=new List<Book>()
              };*/

            //Mapping

            var CurAuthor=mapper.Map<Author>(NewAuthor);
            //  CurAuthor.AuthorId = dbContext.Author.Max(x => x.AuthorId) + 1;
            CurAuthor=dbContext.Author.Add(CurAuthor).Entity;
            dbContext.SaveChanges();
            //return Ok();
            return CreatedAtAction(nameof(GetAuthorById),
                                    //"GetAuthorById",
                                    new { Id = CurAuthor.AuthorId },
                                    MapAuthorToResponse(CurAuthor));

        }

        [HttpPut("{AuthorId}")]
        public IActionResult UpdateAuthor(int AuthorId,AuthorUpdateRequestDTO newAuthor)
        {
            if (String.IsNullOrWhiteSpace(newAuthor.AuthorName))
            {
                error.LoadError("Ath003");
                ModelState.AddModelError(error.ErrorProp, error.ErrorMessage);
                return ValidationProblem();
            }

            var CurAuthor = dbContext.Author.Where(x => x.AuthorId == AuthorId).SingleOrDefault();
            if (CurAuthor == null)
            {
                error.LoadError("Ath001");
                ModelState.AddModelError(error.ErrorProp, error.ErrorMessage);
                return ValidationProblem();
            }

            // CurAuthor.AuthorName = newAuthor.AuthorName;
            // CurAuthor.Location = newAuthor.Location;
            mapper.Map(newAuthor, CurAuthor);

            return NoContent();//Ok(CurAuthor);

        }

        [HttpPatch("{AuthorId}")]
        public IActionResult UpdateAuthorPartially(int AuthorId,JsonPatchDocument AuthorPatch)
        {
            var CurAuthor = dbContext.Author.Where(x => x.AuthorId == AuthorId).SingleOrDefault();
            if (CurAuthor == null)
            {
                return NotFound(new
                {
                    ErrorCode = 503,
                    ErrorMessage = "Invalid Author Id"
                });
            }

            AuthorPatch.ApplyTo(CurAuthor);

            //check Cur Author Validation
            if (!TryValidateModel(CurAuthor))
                return ValidationProblem();


            return NoContent(); // Ok(CurAuthor)

        }

        [HttpDelete("{AuthorId}")]
        public IActionResult DeleteAuthor(int AuthorId)
        {

            
            var CurAuthor = dbContext.Author.Where(x => x.AuthorId == AuthorId).SingleOrDefault();
            if (CurAuthor == null)
            {
                return NotFound("Author Is not Exists");
            }
            //Authors.Remove(CurAuthor);
            CurAuthor.IsDeleted = true;

            return NoContent();
        }


        private AuthorResponseDTO MapAuthorToResponse(Author x)
        {
            return new AuthorResponseDTO()
            {
                AuthorId = x.AuthorId,
                AuthorName = x.AuthorName,
                Location = x.Location,
                BookCount = (x.Books == null) ? 0 : x.Books.Count()

            };
        }

    }
}