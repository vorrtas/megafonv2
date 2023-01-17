using MaterialSkin;

namespace Megafon.UI.Controls;

public class CalendarPicker : DateTimePicker
{
    private bool droppedDown = false;
    private readonly Image calendarIcon = Resource.calendarwhite;
    private RectangleF iconButtonArea;
    private const int calendarIconWidth = 34;
    private const int arrowIconWidth = 17;

    public Color SkinColor
    {
        get => MaterialSkinManager.Instance.ColorScheme.AccentColor;
        set => Invalidate();
    }
    public Color TextColor
    {
        get => MaterialSkinManager.Instance.ColorScheme.TextColor;
        set => Invalidate();
    }

    private DateTime? _datasource { get; set; } = null;
    public DateTime? DataSource
    {
        get => _datasource;
        set
        {
            _datasource = value;

            if (_datasource is null)
            {
                Format = DateTimePickerFormat.Custom;
                CustomFormat = " ";
            }
            else
            {
                Format = DateTimePickerFormat.Custom;
                CustomFormat = "yyyy-MM-dd";
                Value = (DateTime)_datasource;
            }
        }
    }


    public CalendarPicker()
    {
        SetStyle(ControlStyles.UserPaint, true);
        Format = DateTimePickerFormat.Short;
        MinimumSize = new Size(0, 35);
        var manager = MaterialSkinManager.Instance;
        Font = manager.getFontByType(MaterialSkinManager.fontType.Body1);
        manager.ColorSchemeChanged += (s) => ApplyMaterialStyle();
        manager.ThemeChanged += (s) => ApplyMaterialStyle();
        MouseDown += (s, e) => Clicked(e);
        DataSource = null;
    }

    private void Clicked(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            DataSource = null;
        }
    }

    private void ApplyMaterialStyle()
    {
        var manager = MaterialSkinManager.Instance;
        SkinColor = manager.ColorScheme.PrimaryColor;
        TextColor = manager.ColorScheme.TextColor;
    }

    protected override void OnValueChanged(EventArgs eventargs)
    {
        base.OnValueChanged(eventargs);
        DataSource = Value;
    }

    protected override void OnDropDown(EventArgs eventargs)
    {
        base.OnDropDown(eventargs);
        droppedDown = true;
    }

    protected override void OnCloseUp(EventArgs eventargs)
    {
        base.OnCloseUp(eventargs);
        droppedDown = false;
    }

    protected override void OnKeyPress(KeyPressEventArgs e)
    {
        base.OnKeyPress(e);
        e.Handled = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        using (Graphics graphics = CreateGraphics())
        using (SolidBrush skinBrush = new SolidBrush(SkinColor))
        using (SolidBrush openIconBrush = new SolidBrush(Color.FromArgb(50, 64, 64, 64)))
        using (SolidBrush textBrush = new SolidBrush(TextColor))
        using (StringFormat textFormat = new StringFormat())
        {
            RectangleF clientArea = new RectangleF(0, 0, Width - 0.5F, Height - 0.5F);
            RectangleF iconArea = new RectangleF(clientArea.Width - calendarIconWidth, 0, calendarIconWidth, clientArea.Height);
            textFormat.LineAlignment = StringAlignment.Center;

            //Draw surface
            graphics.FillRectangle(skinBrush, clientArea);
            //Draw text
            graphics.DrawString("   " + Text, Font, textBrush, clientArea, textFormat);
            //Draw open calendar icon highlight
            if (droppedDown == true) graphics.FillRectangle(openIconBrush, iconArea);
            //Draw icon
            graphics.DrawImage(calendarIcon, Width - calendarIcon.Width - 9, (Height - calendarIcon.Height) / 2);

        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        int iconWidth = GetIconButtonWidth();
        iconButtonArea = new RectangleF(Width - iconWidth, 0, iconWidth, Height);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (iconButtonArea.Contains(e.Location))
            Cursor = Cursors.Hand;
        else Cursor = Cursors.Default;
    }

    private int GetIconButtonWidth()
    {
        int textWidh = TextRenderer.MeasureText(Text, Font).Width;
        if (textWidh <= Width - (calendarIconWidth + 20))
            return calendarIconWidth;
        else return arrowIconWidth;
    }
}