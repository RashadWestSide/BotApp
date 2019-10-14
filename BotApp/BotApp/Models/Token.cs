using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BotApp.Models
{
    public class Token
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Token")]
        public string mtoken { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}