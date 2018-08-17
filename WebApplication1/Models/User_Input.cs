using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApplication1.Models
{
    public class User_Input
    {
        [Required(ErrorMessage = "UserName不能为空")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email不能为空")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Address不能为空")]
        public string Address { get; set; }
        [Range(1, 30, ErrorMessage = "你好我好大家好")]
        public int Age { get; set; }
    }
}
