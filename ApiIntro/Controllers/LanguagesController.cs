using ApiIntro.Dtos;
using ApiIntro.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



namespace ApiIntro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {

        // bu resource dil dosyasına erişmemizi sağlayan servis.
        // IHtmlLocalizer diye bir servis var onunla resx dosyasında html okuyabiliriz.
        private readonly IStringLocalizer<Resource> _stringLocalizer;


        public LanguagesController(IStringLocalizer<Resource> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet("lang")]
        public IActionResult GetCurrentLanguage()
        {
            // hangi dildeysek bu dil ile alakalı key üzerinden value okuruz.
            string title =  _stringLocalizer.GetString("UserNotFound");

            //System.Globalization.CultureInfo.CurrentCulture.
            // GreenWich saati için UtcNow 
            string date = DateTime.UtcNow.ToString();

            return Ok(System.Globalization.CultureInfo.CurrentCulture.Name);
        }


        [HttpPost("setLanguage")]
        public IActionResult ChangeLanguage([FromHeader(Name = "language")] string language, [FromHeader(Name ="currency")] string currency)
        {

            //Thread.CurrentThread.CurrentCulture = new CultureInfo(request.Language);
            //Thread.CurrentThread.CurrentCulture.


            //Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);

            //System.Globalization.CultureInfo.CurrentCulture = new CultureInfo(language);




            return Ok(Thread.CurrentThread.CurrentCulture.Name);
        }


        [HttpPost("setLanguage-v2")]
        public IActionResult ChangeLanguage2([FromHeader] LanguageHeaderDto request)
        {

            //Thread.CurrentThread.CurrentCulture = new CultureInfo(request.Language);
            //Thread.CurrentThread.CurrentCulture.
            // CurrentThread.CurrentCulture
            Thread.CurrentThread.CurrentCulture = new CultureInfo(request.Language);

            return Ok(Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
        }

        [HttpPost("set-location/{latitude}/{longitude}")]
        public IActionResult SetLocation([FromRoute] string latitude, [FromRoute] string longitude)
        {
            return Ok();
        }
    }
}
