
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

// Api Localization Makalesi yazalım.

namespace ApiIntro.Dtos
{
    public class LoginRequestDto
    {


        // Resx dosyasında Key Hata mesajı olarak yazdık.
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "EmailFormatError")]
        public string EmailAddress { get; set; }
        
        public string Password { get; set; }

     


    }
}
