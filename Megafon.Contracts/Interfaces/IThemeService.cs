namespace Megafon.Contracts.Interfaces;

public interface IThemeService
{
    public void LoadThemeOnStart();
    public void SetDefault();
    public void SaveThemeOnClose();
}
