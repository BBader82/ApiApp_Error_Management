using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp_Male.Models.ResponseDTO
{
    public class AuthorResponseDTO
    {
        public int AuthorId { get; set; }
        public String AuthorName { get; set; }
        public String Location { get; set; }

        public int BookCount { get; set; }
        
    }
}
