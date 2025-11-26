using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace notepad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Stack<string> redoStack = new Stack<string>();
        Stack<string> undoStack = new Stack<string>();
        string currentFilePath = null; // مسیر فایل فعلی
        bool isDirty = false;
        private void fontToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            textBox1.Font = fontDialog1.Font;
        }

        private void colorToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            textBox1.ForeColor = colorDialog1.Color;
        }

        private void colorToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            textBox1.BackColor = colorDialog1.Color;
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Paste();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (undoStack.Count > 0)
            {
                string last = undoStack.Pop();
                redoStack.Push(textBox1.Text);
                textBox1.Text = last;
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                string next = redoStack.Pop();
                undoStack.Push(textBox1.Text);
                textBox1.Text = next;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            undoStack.Push(textBox1.Text);
            isDirty = true; // هر بار متن تغییر کرد
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Notepad by Fatemeh\n\n" + "A simple Windows Forms text editor.\n" + "Version 1.0\n" + "© 2025";

            MessageBox.Show(message, "About Notepad", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog fd = new FontDialog())
            {
                fd.Font = textBox1.Font;
                fd.ShowColor = true;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Font = fd.Font;
                    textBox1.ForeColor = fd.Color;
                }
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Enable WordWrap?", "Options",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                textBox1.WordWrap = true;
                textBox1.ScrollBars = ScrollBars.Vertical;
            }
            else
            {
                textBox1.WordWrap = false;
                textBox1.ScrollBars = ScrollBars.Both;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                // اگر فایل جدید باشه، Save As اجرا بشه
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                sfd.Title = "Save As";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = sfd.FileName;
                    System.IO.File.WriteAllText(currentFilePath, textBox1.Text);
                    MessageBox.Show("File saved successfully!", "Save As", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // اگر مسیر فایل قبلاً مشخص شده باشه، مستقیم ذخیره کن
                System.IO.File.WriteAllText(currentFilePath, textBox1.Text);
                MessageBox.Show("File saved successfully!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            sfd.Title = "Save As";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = sfd.FileName; // مسیر ذخیره شد
                System.IO.File.WriteAllText(currentFilePath, textBox1.Text);
                MessageBox.Show("File saved successfully!", "Save As", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // پنجره باز کردن فایل
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            ofd.Title = "Open File";

            // اگر کاربر فایل انتخاب کرد
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // متن فایل رو بخون و داخل TextBox بذار
                textBox1.Text = System.IO.File.ReadAllText(ofd.FileName);

                // مسیر فایل رو ذخیره کن تا بعداً Save مستقیم روی همین فایل کار کنه
                currentFilePath = ofd.FileName;
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog(); // پنجره انتخاب چاپگر
            if (pd.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Printing started...", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // اینجا می‌تونی متن رو برای چاپگر بفرستی
                // برای ساده‌ترین حالت فقط پیام نشون می‌دیم
            }
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = printDocument1; // همون شیء پرینت که قبلاً ساختیم
            ppd.ShowDialog(); // پنجره پیش‌نمایش چاپ رو نشون بده
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isDirty) // فقط وقتی تغییرات ذخیره نشده وجود داره
            {
                if (string.IsNullOrEmpty(currentFilePath))
                {
                    // فایل جدید هست → Save As
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    sfd.Title = "Save As";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        currentFilePath = sfd.FileName;
                        System.IO.File.WriteAllText(currentFilePath, textBox1.Text);
                        isDirty = false;
                    }
                    else
                    {
                        return; // اگر کاربر Cancel زد، خروج لغو بشه
                    }
                }
                else
                {
                    // فایل ذخیره شده ولی تغییرات جدید داره
                    var result = MessageBox.Show("Do you want to save changes?","Exit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        System.IO.File.WriteAllText(currentFilePath, textBox1.Text);
                        isDirty = false;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return; // خروج لغو بشه
                    }
                }
            }

            Application.Exit(); 
        }
    }
}
