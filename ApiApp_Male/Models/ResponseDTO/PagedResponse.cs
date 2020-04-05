using ApiApp_Male.helper;
using ApiApp_Male.Models.RequestDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ApiApp_Male.Models.ResponseDTO
{
    public class PagedResponse<T>
    {
        public PagedResponse(IQueryable<T> Query,PagingDTO ClientPaging)
        {
            Paging = new PagingDetails();

            Paging.TotalRows = Query.Count();

            Paging.TotalPages = (int)Math.Ceiling((double)Paging.TotalRows / ClientPaging.RowCount);
            Paging.CurPage = ClientPaging.PageNumber;
            Paging.HasNextPage = Paging.CurPage < Paging.TotalPages;
            Paging.HasPrevPage = Paging.CurPage > 1;

            Data=Query.Skip((ClientPaging.PageNumber - 1) *
                            ClientPaging.RowCount).Take(ClientPaging.RowCount).ToList();
        }
        public PagingDetails Paging { get; set; }
        public List<T> Data { get; set; }
    }
}
