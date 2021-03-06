﻿using System.ComponentModel.DataAnnotations;

namespace ZFood.Web.DTO
{
    public class CreateUserRequestDTO
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public string ProviderId { get; set; }
    }
}