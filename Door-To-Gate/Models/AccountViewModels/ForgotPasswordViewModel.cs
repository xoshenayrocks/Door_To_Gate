using System;
using System.ComponentModel.DataAnnotations;

namespace Door_To_Gate.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
