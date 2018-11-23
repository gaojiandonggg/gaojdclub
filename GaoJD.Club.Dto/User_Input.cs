
using GaoJD.Club.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GaoJD.Club.Dto
{
    public class User_Input
    {
        [Required(ErrorMessage = "不能为空")]
        public string UserName { get; set; }
        [ClassicMovie]
        [Required(ErrorMessage = "不能为空")]
        public string Email { get; set; }
        public string Address { get; set; }
        [Range(1, 30, ErrorMessage = "你好我好大家好")]
        public int Age { get; set; }
    }
}
