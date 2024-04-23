using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewComponents;

public class LoginModel
{
    [Required(ErrorMessage = "Отсутствует ")]
    public long TelegramId { get; set; }
    
    [Required(ErrorMessage = "Нужен доступ к аккаунту Telegram")]
    public string TelegramUsername { get; set; }
    
    [Display(Name = "Имя пользователя")]
    public string? Name { get; set; }
    
    [Display(Name = "Номер телефона")]
    public string? PhoneNumber { get; set; }
    

    // [Required(ErrorMessage = "Требуется ввести логин")]
    // public string Login { get; set; }
    //
    // [Required(ErrorMessage = "Требуется ввести пароль")]
    // public string Password { get; set; }
}