using System.ComponentModel.DataAnnotations;

namespace MyBlog.Services.ViewModels.Attributes
{
    public class NotWhiteSpaceAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            return value != null && !((string)value).Contains(' ');
        }
    }
}
