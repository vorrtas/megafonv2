using System.ComponentModel;
using System.Data;
using System.Globalization;

using MaterialSkin;

using Zuby.ADGV;

namespace Megafon.UI.Controls;

public class Grid : AdvancedDataGridView
{
    private int scrollingRowIndex;
    private int scrollingColIndex;
    private readonly BindingSource _src = new BindingSource() { DataMember = "" };


    public Grid()
    {
        SetDoubleBuffered();
        SetStyles();
        ColumnAdded += (s, e) => SetFormatForDecimals(e);
        FilterStringChanged += (s, e) => FilterChanged(e.FilterString);
        var manager = MaterialSkinManager.Instance;
        manager.ColorSchemeChanged += (s) => ApplyMaterialStyle();
        manager.ThemeChanged += (s) => ApplyMaterialStyle();
        ApplyMaterialStyle();
    }

    public void SetData<T>(IEnumerable<T> data)
    {
        (int x, int y)[] buff = new (int, int)[SelectedCells.Count];
        for (int i = 0; i < buff.Length; i++)
        {
            buff[i] = (SelectedCells[i].RowIndex, SelectedCells[i].ColumnIndex);
        }
        SaveScrollPosition();
        DataTable dt = BuildDataTable(data);
        _src.DataSource = dt.DefaultView;
        DataSource = _src;
        RestoreScrollPosition();
        ClearSelection();
        foreach (var cell in buff)
        {
            try
            {
                this[cell.y, cell.x].Selected = true;
            }
            catch { }
        }
    }

    private void SetStyles()
    {
        AutoGenerateColumns = true;
        EnableHeadersVisualStyles = false;
        AllowUserToAddRows = false;
        AllowUserToDeleteRows = false;
        AllowUserToOrderColumns = true;
        AllowUserToResizeRows = false;
        BorderStyle = BorderStyle.None;
        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        RightToLeft = RightToLeft.No;
        RowHeadersVisible = false;
        EditMode = DataGridViewEditMode.EditProgrammatically;
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        DefaultCellStyle.FormatProvider = CultureInfo.CurrentCulture;
    }

    private static void SetFormatForDecimals(DataGridViewColumnEventArgs e)
    {
        if (e.Column.ValueType == typeof(decimal))
        {
            e.Column.DefaultCellStyle.Format = "N";
        }
        e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
    }

    private void ApplyMaterialStyle()
    {
        var manager = MaterialSkinManager.Instance;

        var alt = manager.Theme == MaterialSkinManager.Themes.DARK ?
            Color.FromArgb(manager.BackgroundColor.R + 20, manager.BackgroundColor.G + 20, manager.BackgroundColor.B + 20) :
            Color.FromArgb(manager.BackgroundColor.R - 20, manager.BackgroundColor.G - 20, manager.BackgroundColor.B - 20);

        BackgroundColor = manager.BackdropColor;
        DefaultCellStyle.BackColor = manager.BackgroundColor;
        AlternatingRowsDefaultCellStyle.BackColor = alt;
        ColumnHeadersDefaultCellStyle.BackColor = manager.BackgroundColor;
        ColumnHeadersDefaultCellStyle.ForeColor = manager.TextHighEmphasisNoAlphaColor;
        DefaultCellStyle.SelectionBackColor = manager.ColorScheme.PrimaryColor;
        AlternatingRowsDefaultCellStyle.SelectionBackColor = manager.ColorScheme.PrimaryColor;
    }

    private static DataTable BuildDataTable<T>(IEnumerable<T> lst)
    {
        DataTable tbl = CreateTable<T>();
        Type entType = typeof(T);
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
        foreach (T item in lst)
        {
            DataRow row = tbl.NewRow();
            foreach (PropertyDescriptor prop in properties)
            {
                var colName = prop.Name;

                var displayname = prop.Attributes.OfType<DisplayNameAttribute>().FirstOrDefault();

                if (displayname is not null)
                {
                    colName = displayname.DisplayName;
                }

                row[colName] = prop.GetValue(item) ?? DBNull.Value;
            }
            tbl.Rows.Add(row);
        }
        return tbl;
    }
    private static DataTable CreateTable<T>()
    {
        Type entType = typeof(T);
        DataTable tbl = new DataTable(entType.Name);
        tbl.Locale = CultureInfo.CurrentCulture;
        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
        foreach (PropertyDescriptor prop in properties)
        {
            Type type = IsNullableType(prop.PropertyType) ? Nullable.GetUnderlyingType(prop.PropertyType)! : prop.PropertyType;
            DataColumn dc = new DataColumn(prop.Name, type);

            var visibleAttribute = prop.Attributes.OfType<BrowsableAttribute>().FirstOrDefault();
            if (visibleAttribute is not null && visibleAttribute.Browsable == false)
            {
                dc.ColumnMapping = MappingType.Hidden;
            }

            var displayname = prop.Attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayname is not null)
            {
                dc.ColumnName = displayname.DisplayName;
            }

            tbl.Columns.Add(dc);
        }
        return tbl;
    }

    private static bool IsNullableType(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
    }

    private void SaveScrollPosition()
    {
        if (CurrentRow is not null)
        {
            scrollingRowIndex = FirstDisplayedCell.RowIndex;
            scrollingColIndex = FirstDisplayedCell.ColumnIndex;
        }
    }

    private void RestoreScrollPosition()
    {
        if (CurrentRow is not null)
        {
            FirstDisplayedScrollingRowIndex = scrollingRowIndex;
            FirstDisplayedScrollingColumnIndex = scrollingColIndex;
        }
    }

    private void FilterChanged(string e)
    {
        var bs = (BindingSource)DataSource;
        bs.Filter = e;
    }
}
