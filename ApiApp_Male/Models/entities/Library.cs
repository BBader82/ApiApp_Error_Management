using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp_Male.Models.entities
{
    public class Library
    {
        [Key]
        public int LibraryId { get; set; }
        public String LibraryName { get; set; }
        public String Location { get; set; }
    }
}
