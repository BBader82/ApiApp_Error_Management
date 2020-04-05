using ApiApp_Male.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp_Male.Models.RequestDTO
{
    public class AuthorAddRequestDTO
    {
        [Required]
        [StringLength(10)]
       
        public String AuthorName { get; set; }

        [Required]
        [StringArray(AllowValues =new String[] {"Gaza","Rafah","NorthGaza" })]
        public String Location { get; set; }

       
    }
}
