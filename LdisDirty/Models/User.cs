﻿using LdisDirty.AttributeValidation;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace LdisDirty.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Вы не введи ник")]
        [NameValidation]
        public string Name { get; set; }
        [Required(ErrorMessage = "Вы не ввели почту")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Вы не ввели пароль ")]
        [PasswordValidation]
        public string Password { get; set; }
        public string? ImageLink { get; set; } 
        public string? Status { get; set; }
        public ICollection<Chat>? Chats { get; set; }
    }
}
