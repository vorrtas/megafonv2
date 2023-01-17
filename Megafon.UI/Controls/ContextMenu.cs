using MaterialSkin;

namespace Megafon.UI.Controls;

public class ContextMenu : ContextMenuStrip
{
    public ContextMenu() : base()
    {
        ShowImageMargin = false;
        Opening += (s, e) =>
        {
            BackColor = MaterialSkinManager.Instance.BackdropColor;
            Font = MaterialSkinManager.Instance.getFontByType(MaterialSkinManager.fontType.Body1);
            ForeColor = MaterialSkinManager.Instance.Theme == MaterialSkinManager.Themes.LIGHT ? Color.Black : Color.White;
        };
    }
}
