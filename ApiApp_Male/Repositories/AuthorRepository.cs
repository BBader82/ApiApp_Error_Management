using ApiApp_Male.Models;
using ApiApp_Male.Models.RequestDTO;
using ApiApp_Male.Models.ResponseDTO;
using ApiApp_Male.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace ApiApp_Male.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext dbContext;
        private readonly IMapper mapper;

        public AuthorRepository( LibraryContext dbContext,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        public PagedResponse<AuthorResponseDTO> GetAll(IUrlHelper Url, string FilterAuthorName, string FilterLocation, string orderby, PagingDTO paging)
        {
            var AuthorQuery = dbContext.Author.AsQueryable();
            AuthorQuery = AuthorQuery.Where(x => x.IsDeleted == false);

            if (!String.IsNullOrWhiteSpace(FilterAuthorName))
            {
                AuthorQuery = AuthorQuery.Where(x => x.AuthorName.Equals(FilterAuthorName));
            }

            if (!String.IsNullOrWhiteSpace(FilterLocation))
            {
                AuthorQuery = AuthorQuery.Where(x => x.Location.Contains(FilterLocation, StringComparison.OrdinalIgnoreCase));
            }
            

            if (!String.IsNullOrWhiteSpace(orderby))
            {
                AuthorQuery = AuthorQuery.OrderBy(orderby);
            }
            else
                AuthorQuery = AuthorQuery.OrderBy(x => x.AuthorName);

            
            var ResponseDTO = AuthorQuery.Select(p => mapper.Map<AuthorResponseDTO>(p));

            var pagedResponse = new PagedResponse<AuthorResponseDTO>(ResponseDTO, paging);

            if (pagedResponse.Paging.HasNextPage)
                pagedResponse.Paging.NextPageURL = Url.Link("GetAllAuthors", new
                {
                    FilterAuthorName,
                    FilterLocation,
                    orderby,
                    paging.RowCount,
                    PageNumber = paging.PageNumber + 1
                });
            if (pagedResponse.Paging.HasPrevPage)
                pagedResponse.Paging.PrevPageURL = Url.Link("GetAllAuthors", new
                {
                    FilterAuthorName,
                    FilterLocation,
                    orderby,
                    paging.RowCount,
                    PageNumber = paging.PageNumber - 1
                });

            return pagedResponse;
        }

        public AuthorResponseDTO GetById(int Id,out String ErrorCode)
        {
            ErrorCode = "";
            Author CurAuthor = dbContext.Author.Where(x => x.AuthorId == Id && x.IsDeleted == false).FirstOrDefault();
            if (CurAuthor == null)
            {
                ErrorCode="Ath001";
                return null;
            }
            return mapper.Map<AuthorResponseDTO>(CurAuthor);
        }

        public Author AddAuthor(AuthorAddRequestDTO NewAuthor, out String ErrorCode)
        {
            //check Author Name duplication
            ErrorCode = "";
            if (dbContext.Author.Any(x => x.AuthorName == NewAuthor.AuthorName))
            {
                ErrorCode="Ath002";
                return null;

            }

            //Mapping

            var CurAuthor = mapper.Map<Author>(NewAuthor);
            //  CurAuthor.AuthorId = dbContext.Author.Max(x => x.AuthorId) + 1;
            CurAuthor = dbContext.Author.Add(CurAuthor).Entity;
            SaveChanges();
            return CurAuthor;
        }
        public void UpdateAuthor(int AuthorId, AuthorUpdateRequestDTO newAuthor, out String ErrorCode)
        {
            ErrorCode = "";
            if (String.IsNullOrWhiteSpace(newAuthor.AuthorName))
            {
                ErrorCode="Ath003";
                return;
            }

            var CurAuthor = dbContext.Author.Where(x => x.AuthorId == AuthorId).SingleOrDefault();
            if (CurAuthor == null)
            {
                ErrorCode = "Ath001";
                return;
            }

            // CurAuthor.AuthorName = newAuthor.AuthorName;
            // CurAuthor.Location = newAuthor.Location;
            mapper.Map(newAuthor, CurAuthor);
            SaveChanges();
        }
        public Author UpdateAuthorPartially(int AuthorId, JsonPatchDocument AuthorPatch, out String ErrorCode)
        {
            ErrorCode = "";
            var CurAuthor = dbContext.Author.Where(x => x.AuthorId == AuthorId).SingleOrDefault();
            if (CurAuthor == null)
            {
                ErrorCode = "Ath001";
                return null;
            }

            AuthorPatch.ApplyTo(CurAuthor);
            return CurAuthor;

        }
        public void DeleteAuthor(int AuthorId,out string ErrorCode)
        {
            ErrorCode="";
            var CurAuthor = dbContext.Author.Where(x => x.AuthorId == AuthorId).SingleOrDefault();
            if (CurAuthor == null)
            {
                ErrorCode = "Ath001";
                return;
            }
            //Authors.Remove(CurAuthor);
            CurAuthor.IsDeleted = true;
            SaveChanges();
        }
        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }
    }
}
