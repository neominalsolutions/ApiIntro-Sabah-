using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiIntro.Dtos
{
   
    // postmanden gönderirken 
    public class LanguageHeaderDto
    {
        // Header bilgisi boş gelmemesi için headerdan gelen bilgiyi [FromHeader] attribute ile işaretledik.
        [FromHeader(Name ="language")]
        public string Language { get; set; }

    }
}
