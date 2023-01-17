using System.ComponentModel;

using MaterialSkin;
using MaterialSkin.Controls;

namespace Megafon.UI.Forms;

public class BaseForm : MaterialForm
{
    public readonly IContainer components;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    public BaseForm()
    {
        SuspendLayout();
        components = new Container();
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(960, 540);
        FormStyle = FormStyles.ActionBar_None;
        Name = "BaseForm";
        Padding = new Padding(3, 24, 3, 3);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "BaseForm";
        ResumeLayout(false);
        MaterialSkinManager.Instance.AddFormToManage(this);
        Icon = Resource.megaphone;
    }
}
