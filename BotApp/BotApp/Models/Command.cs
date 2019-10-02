using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BotApp.Models
{
    public class Command
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Запрос боту")]
        public string Request { get; set; }

        [Required]
        [Display(Name = "Ответ бота на запрос")]
        public string Response { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}