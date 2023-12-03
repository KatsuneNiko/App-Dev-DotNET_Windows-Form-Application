using System;
using System.IO;
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
    public partial class TextEditorForm : Form
    {
        private LogInForm logInForm;
        private bool logOut;
        private User currentUser;
        private String currentFile = "";
        private String currentFileType = ".rtf";

        public TextEditorForm(LogInForm logInForm, User currentUser)
        {
            this.logInForm = logInForm;
            InitializeComponent();

            //Track the currently logged in user and updates the username Label accordingly.
            this.currentUser = currentUser;
            usernameLabel.Text = $"User Name: {currentUser.Username}";

            if (currentUser.UserType == typeEnum.Edit) //Enables the stuff that a user with UserType Edit has access to
            {
                editingRichTextBox.ReadOnly = false;
                newToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                toolStripNewButton.Enabled = true;
                toolStripSaveButton.Enabled = true;
                toolStripSaveAsButton.Enabled = true;
                toolStripBoldButton.Enabled = true;
                toolStripItalicsButton.Enabled = true;
                toolStripUnderlineButton.Enabled = true;
                fontSizeComboBox.Enabled = true;
                cutToolStripMenuItem.Enabled = true;
                toolStripCutButton.Enabled = true;
                copyToolStripMenuItem.Enabled = true;
                toolStripCopyButton.Enabled = true;
                pasteToolStripMenuItem.Enabled = true;
                toolStripPasteButton.Enabled = true;
            }
        }

        private void TextEditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            //Shortcut keys for most (all?) of the Menu elements and actions. Not part of specifications, I just wanted to add it.
            if (e.Control && e.KeyCode == Keys.N)
            {
                if(newToolStripMenuItem.Enabled) newToolStripMenuItem_Click(sender, e);
            }
            else if (e.Control && e.KeyCode == Keys.O)
            {
                openToolStripMenuItem_Click(sender, e);
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                if (saveToolStripMenuItem.Enabled) saveToolStripMenuItem_Click(sender, e);
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.S)
            {
                if (saveAsToolStripMenuItem.Enabled) saveAsToolStripMenuItem_Click(sender, e);
            }
            else if (e.Control && e.KeyCode == Keys.X)
            {
                if (cutToolStripMenuItem.Enabled) editingRichTextBox.Cut();
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                if (copyToolStripMenuItem.Enabled) editingRichTextBox.Copy();
            }
            else if (e.Control && e.KeyCode == Keys.V)
            {
                if (pasteToolStripMenuItem.Enabled) editingRichTextBox.Paste();
            }
            else if (e.Control && e.KeyCode == Keys.B)
            {
                if (toolStripBoldButton.Enabled) applyFontStyle(FontStyle.Bold);
            }
            else if (e.Control && e.KeyCode == Keys.I)
            {
                if (toolStripItalicsButton.Enabled) applyFontStyle(FontStyle.Italic);
            }
            else if (e.Control && e.KeyCode == Keys.U)
            {
                if (toolStripUnderlineButton.Enabled) applyFontStyle(FontStyle.Underline);
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Boolean logOut is used to determine whether to return to log in menu or not when closing Form
            //If logOut isn't enabled (when user presses X) the program exits without returning to logInForm
            logOut = true;
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openTextFileDialog = new OpenFileDialog() 
            {
                Title = "Open File",
                Filter = "All Files (*.*)|*.*|Rich Text Files (*.rtf)|*.rtf|Text Files (*.txt)|*.txt"
            };
            if(openTextFileDialog.ShowDialog() == DialogResult.OK) {
                //If extension is rtf or txt, update the RichTextBox accordingly. Else, display an error message.
                if (Path.GetExtension(openTextFileDialog.FileName) == ".rtf" || Path.GetExtension(openTextFileDialog.FileName) == ".RTF")
                {
                    //try-catch block to catch broken .rtf files when loading file into editingRichTextBox
                    try
                    {
                        editingRichTextBox.LoadFile(currentFile);
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("File format is not valid. The .rtf file may be corrupted.", "File is not valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    currentFile = openTextFileDialog.FileName;
                    currentFileType = Path.GetExtension(currentFile);
                    this.Text = Path.GetFileName(currentFile) + " | Text Editor";
                }
                else if (Path.GetExtension(openTextFileDialog.FileName) == ".txt" || Path.GetExtension(openTextFileDialog.FileName) == ".TXT")
                {
                    currentFile = openTextFileDialog.FileName;
                    currentFileType = Path.GetExtension(currentFile);
                    editingRichTextBox.Text = File.ReadAllText(currentFile);
                    this.Text = Path.GetFileName(currentFile) + " | Text Editor";
                }
                else
                {
                    MessageBox.Show("File type is not supported by this Text Editor. Please only open .txt or .rtf files.", "File type not supported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //If the user has already opened or saved the file previously, save again according to file type.
            if (currentFile != "")
            {
                if (currentFileType == ".txt" || currentFileType == ".TXT")
                {
                    editingRichTextBox.SaveFile(currentFile, RichTextBoxStreamType.PlainText);
                }
                else if (currentFileType == ".rtf" || currentFileType == ".RTF")
                {
                    editingRichTextBox.SaveFile(currentFile, RichTextBoxStreamType.RichText);
                }
            }
            //If the user hasn't saved the file before, then go through Save As menu
            else
            {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveTextFileDialog = new SaveFileDialog() 
            {
                Title = "Save As",
                Filter = "Rich Text Files (*.rtf)|*.rtf|Text Files (*.txt)|*.txt"
            };
            if (saveTextFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFile = saveTextFileDialog.FileName;
                currentFileType = Path.GetExtension(currentFile);
                //Save file contents to richTextBox depending on extension type.
                if (currentFileType == ".txt")
                {
                    editingRichTextBox.SaveFile(currentFile, RichTextBoxStreamType.PlainText);
                }
                else if (currentFileType == ".rtf")
                {
                    editingRichTextBox.SaveFile(currentFile, RichTextBoxStreamType.RichText);
                }
                this.Text = Path.GetFileName(currentFile) + " | Text Editor";
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Clears relevant fields and resets the Windows Form title.
            currentFile = "";
            currentFileType = "";
            editingRichTextBox.Text = "";
            this.Text = "Untitled | Text Editor";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //Always save users to file regardless of how the form is being closed.
            base.OnFormClosing(e);
            logInForm.userList.saveUsersToFile();
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            if (logOut)
            {
                logInForm.Show();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void toolStripNewButton_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
        }

        private void toolStripOpenButton_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
        }

        private void toolStripSaveButton_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        private void toolStripSaveAsButton_Click(object sender, EventArgs e)
        {
            saveAsToolStripMenuItem_Click(sender, e);
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editingRichTextBox.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editingRichTextBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editingRichTextBox.Paste();
        }

        private void toolStripCutButton_Click(object sender, EventArgs e)
        {
            cutToolStripMenuItem_Click(sender, e);
        }

        private void toolStripCopyButton_Click(object sender, EventArgs e)
        {
            copyToolStripMenuItem_Click(sender, e);
        }

        private void toolStripPasteButton_Click(object sender, EventArgs e)
        {
            pasteToolStripMenuItem_Click(sender, e);
        }

        private void toolStripAboutButton_Click(object sender, EventArgs e)
        {
            aboutToolStripMenuItem_Click(sender, e);
        }

        private void applyFontStyle(FontStyle newStyle)
        {
            int selectionStart = editingRichTextBox.SelectionStart;
            int selectionLength = editingRichTextBox.SelectionLength;
            Font currentFont;

            //Applies the styles to each character one by one so that other styles are preserved.
            for (int i = 0; i < selectionLength; i++)
            {
                editingRichTextBox.Select(selectionStart + i, 1);
                currentFont = editingRichTextBox.SelectionFont;
                //XOR operand adds or removes the new Style, depending on if that style already exists or not.
                editingRichTextBox.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, currentFont.Style ^ newStyle);
            }

            //Resets the selection to what the user had before the button was pressed.
            editingRichTextBox.Select(selectionStart, selectionLength);
        }

        private void toolStripBoldButton_Click(object sender, EventArgs e)
        {
            applyFontStyle(FontStyle.Bold);
        }


        private void toolStripItalicsButton_Click(object sender, EventArgs e)
        {
            applyFontStyle(FontStyle.Italic);
        }

        private void toolStripUnderlineButton_Click(object sender, EventArgs e)
        {
            applyFontStyle(FontStyle.Underline);
        }

        private void fontSizeComboBox_TextChanged(object sender, EventArgs e)
        {
            //If the text of the combo box is changed (whether it be through a menu selection or manual input, update the font of the selected area accordingly
            if (float.TryParse(fontSizeComboBox.Text, out float result))
            {
                //Only update if the new font size is between 8 and 20 according to specifications
                if (result <= 20 && result >= 8)
                {
                    Font currentFont = editingRichTextBox.SelectionFont;
                    editingRichTextBox.SelectionFont = new Font(currentFont.FontFamily, result, currentFont.Style);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }
    }
}
