using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webapp.Attributes
{
    public class FutureOnlyDates : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value != null && value is DateTime)
            {
                DateTime date = Convert.ToDateTime(value);
                if (date >= DateTime.Now)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Only today or future date is permitted");

            }
            return new ValidationResult("Invalid data. Please try again");
        }
    }
}