using MaterialSkin;
using System.Text.Json;

using Megafon.Contracts.Interfaces;
using Megafon.Contracts.Models;
using Megafon.Domain.Helpers;

namespace Megafon.Domain.Services;

public class ThemeService : IThemeService
{
       private readonly MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;

    public void LoadThemeOnStart()
    {
        string serializedBundle = string.Empty;
        try
        {
            serializedBundle = File.ReadAllText(Path.Join(Directory.GetCurrentDirectory(), "settings.json"));
        }
        catch { }

        if (serializedBundle == string.Empty)
        {
            SetDefault();
            return;
        }

        ThemeBundle? bundle = JsonSerializer.Deserialize<ThemeBundle>(serializedBundle);

        if (bundle is null)
        {
            SetDefault();
            return;
        }

        materialSkinManager.Theme = bundle.Theme;
        materialSkinManager.ColorScheme = new ColorScheme(bundle.Main, bundle.DarkMain, bundle.LightMain, bundle.Accent, TextShade.WHITE);
        materialSkinManager.EnforceBackcolorOnAllComponents = true;
    }



    public void SaveThemeOnClose()
    {
        Application.ApplicationExit += (s, e) =>
        {
            var bundle = new ThemeBundle()
            {
                Theme = materialSkinManager.Theme,
                Main = (Primary)materialSkinManager.ColorScheme.PrimaryColor.ToInt(),
                LightMain = (Primary)materialSkinManager.ColorScheme.LightPrimaryColor.ToInt(),
                DarkMain = (Primary)materialSkinManager.ColorScheme.DarkPrimaryColor.ToInt(),
                Accent = (Accent)materialSkinManager.ColorScheme.AccentColor.ToInt(),
            };
            var serializedBundle = JsonSerializer.Serialize(bundle);
            File.WriteAllText(Path.Join(Directory.GetCurrentDirectory(), "settings.json"), serializedBundle);
        };
    }

    public void SetDefault()
    {
        materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
        materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue400, Primary.Blue700, Primary.Blue300, Accent.Indigo400, TextShade.WHITE);
    }
}
