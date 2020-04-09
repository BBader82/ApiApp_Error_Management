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
using ApiApp_Male.Repositories;
using ApiApp_Male.Repositories.Interfaces;
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
       
        private readonly IErrorClass error;
        private readonly IAuthorRepository authorRep;

        public AuthorController(IMapper Mapper,
                               
                                IErrorClass error,
                                IAuthorRepository AuthorRep)
        {
            mapper = Mapper;
         
            this.error = error;
            authorRep = AuthorRep;
        }

        [HttpGet(Name = "GetAllAuthors")]
        public IActionResult GetAllAuthors([FromQuery]String FilterAuthorName,
                                            String FilterLocation,
                                           // [FromQuery] String SearchingString,
                                            String orderby,
                                            [FromQuery] PagingDTO paging)
        {

            var Result=authorRep.GetAll(Url, FilterAuthorName, FilterLocation, orderby, paging);

            return Ok(Result); 
        }

        [HttpGet("{Id}")] //Api/Author/
        public IActionResult GetAuthorById(int Id)
        {
            String ErrorCode = "";
            var result= authorRep.GetById(Id,out ErrorCode);
            if (!String.IsNullOrWhiteSpace( ErrorCode))
            {
                error.LoadError(ErrorCode);
                ModelState.AddModelError(error.ErrorProp, error.ErrorMessage);
                return ValidationProblem();
            }
                        
            return Ok(result);
                
        }

        [HttpPost]
        public IActionResult AddAuthor([FromBody]AuthorAddRequestDTO NewAuthor)
        {
            String ErrorCode = "";
            var result=authorRep.AddAuthor(NewAuthor, out ErrorCode);
            //check Author Name duplication
            if (!String.IsNullOrWhiteSpace(ErrorCode))
            {
                error.LoadError(ErrorCode);
                ModelState.AddModelError(error.ErrorProp, error.ErrorMessage);
                return ValidationProblem();

            }

            return CreatedAtAction(nameof(GetAuthorById),
                                    //"GetAuthorById",
                                    new { Id = result.AuthorId },
                                    mapper.Map<AuthorResponseDTO>(result));

        }

        [HttpPut("{AuthorId}")]
        public IActionResult UpdateAuthor(int AuthorId,AuthorUpdateRequestDTO newAuthor)
        {
            String ErrorCode = "";
            authorRep.UpdateAuthor(AuthorId, newAuthor, out ErrorCode);
            if (!String.IsNullOrWhiteSpace(ErrorCode))
            {
                error.LoadError(ErrorCode);
                ModelState.AddModelError(error.ErrorProp, error.ErrorMessage);
                return ValidationProblem();

            }

            return NoContent();//Ok(CurAuthor);

        }

        [HttpPatch("{AuthorId}")]
        public IActionResult UpdateAuthorPartially(int AuthorId,JsonPatchDocument AuthorPatch)
        {
            String ErrorCode = "";
            var CurAuthor = authorRep.UpdateAuthorPartially(AuthorId, AuthorPatch, out ErrorCode);
            if (!String.IsNullOrWhiteSpace(ErrorCode))
            {
                error.LoadError(ErrorCode);
                ModelState.AddModelError(error.ErrorProp, error.ErrorMessage);
                return ValidationProblem();

            }
            //check Cur Author Validation
            if (!TryValidateModel(CurAuthor))
                return ValidationProblem();

            authorRep.SaveChanges();

            return NoContent(); // Ok(CurAuthor)

        }

        [HttpDelete("{AuthorId}")]
        public IActionResult DeleteAuthor(int AuthorId)
        {

            String ErrorCode = "";
            authorRep.DeleteAuthor(AuthorId, out ErrorCode);
            if (!String.IsNullOrWhiteSpace(ErrorCode))
            {
                error.LoadError(ErrorCode);
                ModelState.AddModelError(error.ErrorProp, error.ErrorMessage);
                return ValidationProblem();

            }

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