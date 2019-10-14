using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BotApp.Models
{
    public class RegistViewModel
    {
        [Required]
        [Display(Name = "Имя")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Token")]
        public string Token { get; set; }

    }
}