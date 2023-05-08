using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static
{
    public class MinFiveWords : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string fieldValue = (string)value;

            if(fieldValue != null)
            {
                string[] words = fieldValue.Split(" ");
                if (words.Length < 5)
                {
                    return new ValidationResult("La descrizione deve avere almeno 5 parole");
                }
            }
            else
            {
                return new ValidationResult("La descrizione deve avere almeno 5 parole");
            }

            return ValidationResult.Success;

        }
    }
}
