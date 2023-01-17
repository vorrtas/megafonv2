using System.ComponentModel;

namespace Megafon.UI.Views;

public class BaseView : UserControl
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
    public BaseView()
    {
        SuspendLayout();
        components = new Container();
        AutoScaleMode = AutoScaleMode.None;
        DoubleBuffered = true;
        Name = "BaseControl";
        Size = new Size(950, 530);
        ResumeLayout(false);
    }
}
