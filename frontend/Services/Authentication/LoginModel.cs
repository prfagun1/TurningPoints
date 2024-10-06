﻿using System.ComponentModel.DataAnnotations;

namespace frontend.Services.Authentication
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
