using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Normad.ViewModels
{
    public class RegisterVM
    {
        [Required,MaxLength(50)]
        public string Fullname { get; set; }
        [Required,MaxLength(50)]
        public string Username { get; set; }
        [Required,MaxLength(30),DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required,MaxLength(20),DataType(DataType.Password)]
        public string Password { get; set; }
        [Required,MaxLength(20),DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
