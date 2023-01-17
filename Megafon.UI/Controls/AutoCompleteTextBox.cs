using MaterialSkin;
using MaterialSkin.Controls;

namespace Megafon.UI.Controls;

public class AutoComleteBox : MaterialTextBox2
{
    private readonly ListBox _listBox = new()
    {
        BackColor = MaterialSkinManager.Instance.BackdropColor,
        ForeColor = MaterialSkinManager.Instance.Theme == MaterialSkinManager.Themes.LIGHT ? Color.Black : Color.White,
        Font = MaterialSkinManager.Instance.getFontByType(MaterialSkinManager.fontType.Body1),
    };

    private bool _isAdded;
    private string[] _values = Array.Empty<string>();
    private string _formerValue = string.Empty;
    public event EventHandler? OnTextModified;

    public AutoComleteBox()
    {
        baseTextBox.TextChanged += (s, e) =>
        {
            if (OnTextModified is not null)
            {
                OnTextModified.Invoke(this, e);
            }
        };

        InitializeComponent();
        ResetListBox();
    }

    private void InitializeComponent()
    {
        baseTextBox.KeyDown += this_KeyDown;
        baseTextBox.KeyUp += this_KeyUp;
        _listBox.Click += _listBox_Click;
        LostFocus += (s, e) => ResetListBox();
    }

    private void _listBox_Click(object? sender, EventArgs e)
    {
        if (_listBox.SelectedItems.Count > 0)
        {
            this.Text = _listBox.SelectedItems[0]!.ToString();
        }
        ResetListBox();
    }

    private void ShowListBox()
    {
        if (!_isAdded)
        {
            ArgumentNullException.ThrowIfNull(Parent);
            Parent.Controls.Add(_listBox);
            _listBox.Left = Left;
            _listBox.Top = Top + Height;
            _isAdded = true;
        }
        _listBox.BackColor = MaterialSkinManager.Instance.BackdropColor;
        _listBox.ForeColor = MaterialSkinManager.Instance.Theme == MaterialSkinManager.Themes.LIGHT ? Color.Black : Color.White;
        _listBox.Font = MaterialSkinManager.Instance.getFontByType(MaterialSkinManager.fontType.Body1);
        _listBox.Visible = true;
        _listBox.BringToFront();
    }

    private void ResetListBox()
    {
        _listBox.Visible = false;
    }

    private void this_KeyUp(object? sender, KeyEventArgs e)
    {
        UpdateListBox();
    }

    private void this_KeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Enter:
            case Keys.Tab:
                {
                    if (_listBox.Visible)
                    {
                        baseTextBox.Text = _listBox.SelectedItem.ToString();
                        ResetListBox();
                        _formerValue = Text;
                        baseTextBox.Select(this.Text.Length, 0);
                        e.Handled = true;
                    }
                    break;
                }
            case Keys.Down:
                {
                    if ((_listBox.Visible) && (_listBox.SelectedIndex < _listBox.Items.Count - 1))
                        _listBox.SelectedIndex++;
                    e.Handled = true;
                    break;
                }
            case Keys.Up:
                {
                    if ((_listBox.Visible) && (_listBox.SelectedIndex > 0))
                        _listBox.SelectedIndex--;
                    e.Handled = true;
                    break;
                }
        }
    }

    protected override bool IsInputKey(Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Tab:
                if (_listBox.Visible)
                    return true;
                else
                    return false;
            default:
                return base.IsInputKey(keyData);
        }
    }

    private void UpdateListBox()
    {
        if (baseTextBox.Text == _formerValue)
            return;

        _formerValue = baseTextBox.Text;
        string word = baseTextBox.Text;

        if (_values != null && word.Length > 0)
        {
            string[] matches = Array.FindAll(_values,
                x => (x.ToLower().Contains(word.ToLower())));
            if (matches.Length > 0)
            {
                ShowListBox();
                _listBox.BeginUpdate();
                _listBox.Items.Clear();
                Array.ForEach(matches, x => _listBox.Items.Add(x));
                _listBox.SelectedIndex = 0;
                _listBox.Height = 0;
                _listBox.Width = 0;
                baseTextBox.Focus();
                using (Graphics graphics = _listBox.CreateGraphics())
                {
                    for (int i = 0; i < _listBox.Items.Count; i++)
                    {
                        if (i < 20)
                            _listBox.Height += _listBox.GetItemHeight(i);
                        int itemWidth = (int)graphics.MeasureString(((string)_listBox.Items[i]) + "_", _listBox.Font).Width;
                        _listBox.Width = (_listBox.Width < itemWidth) ? itemWidth : this.Width; ;
                    }
                }
                _listBox.EndUpdate();
            }
            else
            {
                ResetListBox();
            }
        }
        else
        {
            ResetListBox();
        }
    }

    public string[] Values
    {
        get => _values;
        set => _values = value;
    }
}