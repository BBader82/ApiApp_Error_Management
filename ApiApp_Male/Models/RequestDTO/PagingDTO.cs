using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp_Male.Models.RequestDTO
{
    public class PagingDTO
    {
        private int rowCount = 10;

        public int RowCount { get => rowCount; set => rowCount = Math.Min(15, value); }
        public int PageNumber { get; set; } = 1;
    }
}
