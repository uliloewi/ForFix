using Microsoft.Extensions.Logging;

namespace Expo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

            builder.Services.AddBootstrapBlazor(opt => {
                opt.DefaultCultureInfo="de";
                opt.FallbackCulture = "de";
                opt.SupportedCultures = new List<string> { "de-DE", "en-US" };
            }, 
            options =>
            {   // Ignore cultural info loss logs
                options.IgnoreLocalizerMissing = true;

                // Set up RESX format multilingual resource files such as Program.{CultureName}.resx
                options.ResourceManagerStringLocalizerType = typeof(MauiProgram);

                // Set Json format embedded resource file
                options.AdditionalJsonAssemblies = new[] {typeof(BootstrapBlazor.Shared.App).Assembly};

                // Set Json physical path file
                options.AdditionalJsonFiles = new string[]
                {
                    @"Resources\languages\de.json"
                };
            });

            return builder.Build();
        }
    }
}