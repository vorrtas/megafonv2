namespace Megafon.UI.Forms;

public partial class LoginForm : Form
{
    public static void ShowLoginDialog()
    {
        LoginForm form = new LoginForm();
        form.ShowDialog();
    }

    public static void LoginDebug()
    {
        //using (LoginForm form = new LoginForm())
        //{
        //    form.cbUser.Text = "Łukasz Hoffmann";
        //    form.tbPassword.Text = "8264";
        //    form.Authorize();
        //}
    }



    public LoginForm()
    {
        InitializeComponent();
    }
}
