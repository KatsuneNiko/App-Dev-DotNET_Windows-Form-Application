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
    public partial class NewUserForm : Form
    {
        private LogInForm logInForm;
        private bool cancel;

        public NewUserForm(LogInForm logInForm)
        {
            this.logInForm = logInForm;
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            cancel = true;
            this.Close();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (!logInForm.userList.checkUsername(usernameTextBox.Text) && passwordTextBox.Text == reEnterPasswordTextBox.Text && !(usernameTextBox.Text.Contains(',') || passwordTextBox.Text.Contains(',') || firstNameTextBox.Text.Contains(',') || lastNameTextBox.Text.Contains(',')))
            {
                //Adds user to the userList stored in logInForm if no discrepancies are detected.
                logInForm.userList.addUser(usernameTextBox.Text, passwordTextBox.Text, userTypeComboBox.Text, firstNameTextBox.Text, lastNameTextBox.Text, dobDateTimePicker.Value);
                MessageBox.Show("User has been successfully added. Log in to user account using your username and password.", "User successfully added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cancel = true;
                this.Close();
            }
            else if (usernameTextBox.Text.Contains(',') || passwordTextBox.Text.Contains(',') || firstNameTextBox.Text.Contains(',') || lastNameTextBox.Text.Contains(','))
            {
                MessageBox.Show("Fields cannot contain commas. Please remove commas and try again.", "Fields cannot contain commas", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (logInForm.userList.checkUsername(usernameTextBox.Text))
            {
                MessageBox.Show("This username is already in use! Please pick a different username.", "Username already exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (passwordTextBox.Text != reEnterPasswordTextBox.Text)
            {
                MessageBox.Show("The entered passwords do not match! Please check your passwords and try again.", "Passwords do not match", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            if (cancel)
            {
                logInForm.Show();
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
