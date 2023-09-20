using LdisDirty.AttributeValidation;
using System.ComponentModel.DataAnnotations;

namespace LdisDirty.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Вы не ввели почту")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Вы не ввели пароль ")]
        [PasswordValidation]
        public string Password { get; set; }
    }
}
