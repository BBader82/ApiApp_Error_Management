using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp_Male.helper
{
    public class PagingDetails
    {
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }
        public int CurPage { get; set; }
        public Boolean HasNextPage { get; set; }
        public Boolean HasPrevPage { get; set; }

        public String NextPageURL { get; set; }
        public String PrevPageURL { get; set; }
    }
}
