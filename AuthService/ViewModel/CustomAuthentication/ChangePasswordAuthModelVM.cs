using System;
using System.ComponentModel.DataAnnotations;

namespace AuthService.ViewModel.CustomAuthentication
{
    public class ChangePasswordAuthModelVM
    {
        public Guid Id { get; set; }
        [DataType(DataType.Password)]
        [Required]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("CurrentPassword")]
        [Required]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { get; set; }
        public string ReturnUrl { get; set; }
    }
}