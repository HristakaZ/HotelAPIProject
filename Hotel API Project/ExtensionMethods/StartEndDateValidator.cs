using Hotel_API_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hotel_API_Project.ExtensionMethods
{
    public class StartEndDateValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime? startDate = Convert.ToDateTime(value);
            DateTime? endDate = default(DateTime);
            Type type = validationContext.ObjectType;
            if (type == typeof(CreateReservationViewModel))
            {
                CreateReservationViewModel createReservationViewModel = (CreateReservationViewModel)validationContext.ObjectInstance;
                endDate = createReservationViewModel.EndDate;
            }
            if (type == typeof(UpdateReservationViewModel))
            {
                UpdateReservationViewModel updateReservationViewModel = (UpdateReservationViewModel)validationContext.ObjectInstance;
                endDate = updateReservationViewModel.EndDate;
            }

            if (startDate > endDate)
            {
                return new ValidationResult("The start date must be anterior to the end date");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
