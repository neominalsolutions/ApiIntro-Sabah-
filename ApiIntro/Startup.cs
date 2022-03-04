
using ApiIntro.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ApiIntro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddXmlSerializerFormatters()
             .AddDataAnnotationsLocalization(o =>
              {
                  // Dil bazl� hata mesajlar� vermek i�in kulland�k.
                  // AddDataAnnotationsLocalization uygulamadaki DataAnnotationslar� Resource dosyas�ndan oku 
                  o.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(DataAnnotations));
              });



            // Data Annotations Errorlar� resource dosyas�nda okuyaca��z.

            // AddXmlSerializerFormatters bunun ile xml deste�ide olur.
            // api oldu�u i�in sadece controller var
            // Open API ile gelir.
            // Swashbuckle.AspNetCore
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiIntro", Version = "v1" });
            });

            services.AddLocalization(); // Uygulamaya �oklu dil deste�i ekle

            services.AddRequestLocalization(options =>
            {
                // Her bir request de uygulamadaki tan�ml� olan dilleri.
                // default tan�ml� dili
                // Accept-Language ile header �zerinden dil de�i�ikli�i gelidi�inde current Language de�i�tirir.

                options.DefaultRequestCulture = new("tr-TR");

                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("tr-TR")
                };

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;



                // yukar�daki culture de�erleri destek verdiklerimiz

                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    var language = context.Request.Headers["Accept-Language"].ToString();

                    // e�er istekde headerda bir dil yoksa direkt olarak tr-TR olarak ayarlan�r.
                    var defaultLanguage = string.IsNullOrEmpty(language) ? "tr-TR" : language;

                    // ProviderCultureResult ile //Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
                    //Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                    // kodu �al��t�rarak dil de�i�tirdik.
                    return Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
                }));
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiIntro v1"));
            }


            //var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();

            //app.UseRequestLocalization(locOptions.Value);

            app.UseRequestLocalization(); // her bir istekte context.request.header i�erisindeki accept-language okuyan bir ara yaz�l�m var. bunu aktif ediyoruz.

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
