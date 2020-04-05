using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp_Male.Models
{
    public class Author
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AuthorId { get; set; }
        [Required]
        [MaxLength(100)]
        public String AuthorName { get; set; }
        
        [MaxLength(500)]
        public String Location { get; set; }

      
        public Boolean IsDeleted { get; set; }
        public List<Book> Books{ get; set; }
    }
}
