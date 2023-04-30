using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MyBlog.Services.ViewModels.Attributes
{
    public class NotWhiteSpaceAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value) => 
            value != null && !Regex.IsMatch((string)value, @"\s+");
    }
}
