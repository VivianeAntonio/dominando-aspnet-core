using Microsoft.AspNetCore.Localization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace AppSemTemplate.Configuration
{
    public static class GlobalizationConfig
    {
       
        public static WebApplication UseGlobalizationConfig(this WebApplication app)
        {
            var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            
            app.UseRequestLocalization(localizationOptions.Value);

            return app;
        }

        public static WebApplicationBuilder AddGlobalizationConfig(this WebApplicationBuilder builder)
        {
            builder.Services.AddLocalization(opt => opt.ResourcesPath = "Resources");

            builder.Services.Configure<RequestLocalizationOptions>(opt =>
            {
                var supportedCultures = new[] { "pt-BR", "en-US" };
                opt.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });

            return builder;
        }
    }
        
}
