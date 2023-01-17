using Megafon.Contracts.Interfaces;
using Megafon.Domain;
using Megafon.UI;
using Megafon.UI.Forms;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Megafon;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        var builder = Host.CreateDefaultBuilder();
        
        builder.ConfigureServices((context, services) =>
        {
            services.AddServices();
            services.AddViews();
        });

        var app = builder.Build();

        ICultureService culture = app.Services.GetRequiredService<ICultureService>();
        culture.SetDefaultCulture();

        IThemeService theme = app.Services.GetRequiredService<IThemeService>();
        theme.LoadThemeOnStart();
        theme.SaveThemeOnClose();

//#if DEBUG
//        LoginForm.LoginDebug();
//#else
//        LoginForm.ShowLoginDialog();
//#endif

//        IAuthService auth = app.Services.GetRequiredService<IAuthService>();
//        if (!auth.IsLoggedIn())
//            Environment.Exit(0);

        MainForm form = app.Services.GetRequiredService<MainForm>();

        Application.Run(form);
    }
}
