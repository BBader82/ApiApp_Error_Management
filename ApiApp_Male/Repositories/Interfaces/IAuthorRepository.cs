using ApiApp_Male.Models;
using ApiApp_Male.Models.RequestDTO;
using ApiApp_Male.Models.ResponseDTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp_Male.Repositories.Interfaces
{
    public interface IAuthorRepository
    {
       PagedResponse<AuthorResponseDTO> GetAll(IUrlHelper Url, String FilterAuthorName, String FilterLocation, String orderby, PagingDTO paging);
        AuthorResponseDTO GetById(int Id, out String ErrorCode);
        Author AddAuthor(AuthorAddRequestDTO NewAuthor, out String ErrorCode);
        void UpdateAuthor(int AuthorId, AuthorUpdateRequestDTO newAuthor, out String ErrorCode);
        Author UpdateAuthorPartially(int AuthorId, JsonPatchDocument AuthorPatch, out String ErrorCode);
        void DeleteAuthor(int AuthorId, out string ErrorCode);
        int SaveChanges();
    }
}
