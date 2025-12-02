using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace notepad
{
    public partial class FindReplaceform : Form
    {
        private TextBox mainTextBox; // رفرنس به textBox1 از فرم اصلی

        public FindReplaceform(TextBox tb)
        {
            InitializeComponent();
            mainTextBox = tb;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string searchText = txtFind.Text;
            int index = mainTextBox.Text.IndexOf(searchText);

            if (index != -1)
            {
                mainTextBox.Select(index, searchText.Length);
                mainTextBox.Focus();
            }
            else
            {
                MessageBox.Show("متن پیدا نشد!");
            }
        }
        private void btnReplace_Click_1(object sender, EventArgs e)
        {
            string searchText = txtFind.Text;
            string replaceText = txtReplace.Text;

            if (mainTextBox.Text.Contains(searchText))
            {
                mainTextBox.Text = mainTextBox.Text.Replace(searchText, replaceText);
                MessageBox.Show("جایگزینی انجام شد!");
            }
            else
            {
                MessageBox.Show("متن پیدا نشد!");
            }
        }
    }
}
