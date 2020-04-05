using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp_Male.Validation
{
    public class StringArray :ValidationAttribute
    {
        public String[] AllowValues { get; set; }
      
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            if (AllowValues.Contains(value))
                return ValidationResult.Success;

            return new ValidationResult("the value is out of option list");
        }
    }
}
