using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace ApiApp_Male.Models.entities
{
    public class Users
    {
        [Key]
        public String UserId { get; set; }

        [Required]
        [StringLength(150)]
        public String  UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public String Email { get; set; }

        [Required]
        [StringLength(200)]
        public byte[] Passwordhash { get; set; }

        [Required]
        [StringLength(200)]
        public byte[] Passwordsalt { get; set; }

    }
}
