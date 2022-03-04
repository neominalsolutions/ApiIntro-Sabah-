
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
                  // Dil bazlý hata mesajlarý vermek için kullandýk.
                  // AddDataAnnotationsLocalization uygulamadaki DataAnnotationslarý Resource dosyasýndan oku 
                  o.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(DataAnnotations));
              });



            // Data Annotations Errorlarý resource dosyasýnda okuyacaðýz.

            // AddXmlSerializerFormatters bunun ile xml desteðide olur.
            // api olduðu için sadece controller var
            // Open API ile gelir.
            // Swashbuckle.AspNetCore
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiIntro", Version = "v1" });
            });

            services.AddLocalization(); // Uygulamaya çoklu dil desteði ekle

            services.AddRequestLocalization(options =>
            {
                // Her bir request de uygulamadaki tanýmlý olan dilleri.
                // default tanýmlý dili
                // Accept-Language ile header üzerinden dil deðiþikliði gelidiðinde current Language deðiþtirir.

                options.DefaultRequestCulture = new("tr-TR");

                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("tr-TR")
                };

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;



                // yukarýdaki culture deðerleri destek verdiklerimiz

                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    var language = context.Request.Headers["Accept-Language"].ToString();

                    // eðer istekde headerda bir dil yoksa direkt olarak tr-TR olarak ayarlanýr.
                    var defaultLanguage = string.IsNullOrEmpty(language) ? "tr-TR" : language;

                    // ProviderCultureResult ile //Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
                    //Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                    // kodu çalýþtýrarak dil deðiþtirdik.
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

            app.UseRequestLocalization(); // her bir istekte context.request.header içerisindeki accept-language okuyan bir ara yazýlým var. bunu aktif ediyoruz.

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
