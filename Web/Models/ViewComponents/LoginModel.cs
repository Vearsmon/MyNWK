﻿using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewComponents;

public class LoginModel
{
    [Required(ErrorMessage = "Не указан Email")]
    public string Email { get; set; }
     
    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}