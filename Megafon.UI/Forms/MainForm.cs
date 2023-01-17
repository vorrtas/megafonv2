using MaterialSkin.Controls;

using Megafon.Contracts.Interfaces;

namespace Megafon.UI.Forms;

public class MainForm : BaseForm
{
    private readonly MaterialTabControl materialTabControl;
    private readonly List<TabPage> _pages;

    private void AddPage(string pageName, Control? view = null, string? requiredClaim = null)
    {
        TabPage page = new TabPage();
        page.BackColor = Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
        page.Location = new Point(4, 24);
        page.Name = pageName + "Page";
        page.Padding = new Padding(3);
        page.Size = new Size(946, 445);
        page.TabIndex = _pages.Count;
        page.Text = pageName;
        _pages.Add(page);
    }
    private void SetTab(MaterialTabControl materialTabControl)
    {
        materialTabControl.Depth = 0;
        materialTabControl.Dock = DockStyle.Fill;
        materialTabControl.ForeColor = Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        materialTabControl.Location = new Point(3, 64);
        materialTabControl.MouseState = MaterialSkin.MouseState.HOVER;
        materialTabControl.Multiline = true;
        materialTabControl.Name = "materialTabControl";
        materialTabControl.SelectedIndex = 0;
        materialTabControl.Size = new Size(954, 473);
        materialTabControl.TabIndex = 0;
    }
    private void SetForm()
    {
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(960, 540);
        Controls.Add(materialTabControl);
        DrawerAutoShow = true;
        DrawerTabControl = materialTabControl;
        FormStyle = FormStyles.ActionBar_40;
        MinimumSize = new Size(960, 540);
        Name = "MainForm";
        Padding = new Padding(3, 64, 3, 3);
        Text = "Megafon";
        WindowState = FormWindowState.Maximized;
    }

    private readonly IAuthService _authService;
    
    public MainForm(IAuthService authService) : base()
    {
        materialTabControl = new MaterialTabControl();
        _authService = authService;

        _pages = new();
        AddPage("Menu");
        AddPage("Zamówienie");
        AddPage("Archiwum");
        AddPage("Produkcja");
        AddPage("Statystyki");
        AddPage("Premie");
        AddPage("Ustawienia");

        foreach (var page in _pages)
            page.SuspendLayout();

        SuspendLayout();

        foreach (var page in _pages)
            materialTabControl.Controls.Add(page);

        SetTab(materialTabControl);
        SetForm();

        foreach (var page in _pages)
            page.ResumeLayout(false);

        ResumeLayout(false);
    }
}
