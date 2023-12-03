using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment_2___Windows_Form_Application
{
    public partial class LogInForm : Form
    {
        public UserList userList = new UserList();

        public LogInForm()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (userList.checkLogin(usernameTextBox.Text, passwordTextBox.Text))
            {
                TextEditorForm textEditorForm = new TextEditorForm(this, userList.getUser(usernameTextBox.Text, passwordTextBox.Text));
                textEditorForm.Show();
                this.Hide();
                usernameTextBox.Text = "";
                passwordTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("Login details incorrect! Please check your login information and try again.", "Login Incorrect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                usernameTextBox.Text = "";
                passwordTextBox.Text = "";
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            userList.saveUsersToFile();
            Environment.Exit(0);
        }

        private void newUserButton_Click(object sender, EventArgs e)
        {
            NewUserForm newUserForm = new NewUserForm(this);
            newUserForm.Show();
            this.Hide();
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            userList.saveUsersToFile();
        }
    }
}
