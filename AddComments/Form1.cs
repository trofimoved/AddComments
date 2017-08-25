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
using AddComments.Code;

namespace AddComments
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private string AddComments(string readPath, string writePath)
        {
            try
            {
                List<string> text = new List<string>();
                using (StreamReader sr = new StreamReader(readPath, Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        text.Add(line);
                    }
                }

                List<string> newText = FileTxt.ConvertTxtToSql(text);

                using (StreamWriter sw = new StreamWriter(writePath, false, Encoding.Default))
                {                    
                    foreach (string line in newText)
                        sw.WriteLine(line);
                }
                return "Готово";
            }
            catch (Exception ex)
            {
                return "Ошибка: " + ex.Message;
            }
        }

        private string RemoveComments(string readPath, string writePath)
        {
            try
            {
                List<string> text = new List<string>();
                using (StreamReader sr = new StreamReader(readPath, Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        text.Add(line);
                    }
                }

                var newSqlText = FileSql.ConvertSqlToTxt(text);

                var newText = FileTxt.ConvertTxtToSql(newSqlText);
                
                using (StreamWriter sw = new StreamWriter(writePath, false, Encoding.Default))
                {
                    foreach (string line in newText)
                        sw.WriteLine(line);
                }
                
                return "Готово";

            }
            catch (Exception ex)
            {
                return "Ошибка: " + ex.Message;
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            tbFileName.Text = files[0];
            tbNewFileName.Text = files[0].Replace(".txt", ".sql");
            //label2.Text = AddComments(tbFileName.Text, tbNewFileName.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lblStatus.Text = AddComments(tbFileName.Text, tbNewFileName.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lblStatus.Text = RemoveComments(tbFileName.Text, tbNewFileName.Text);
        }

        private void lblStatus_TextChanged(object sender, EventArgs e)
        {
            if (lblStatus.Text == AppValue.StatusTextOk)
            {
                timer.Enabled = true;
                timer.Interval = 5000;
                timer.Start();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lblStatus.Text = AppValue.StatusTextDefault;
            timer.Enabled = false;
        }
    }
}
