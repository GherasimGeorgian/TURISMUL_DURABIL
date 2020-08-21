using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TURISMUL_DURABIL
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string imaginiPath = Form1.pathImagini;
        string constr = Form1.constr;
        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'turismDataSet.Localitati' table. You can move, or remove it, as needed.
            this.localitatiTableAdapter.Fill(this.turismDataSet.Localitati);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            combo2.SelectedItem = null;
            SqlConnection con = new SqlConnection(constr);
            string selected = comboBox1.GetItemText(comboBox1.SelectedItem);
            combo2.Items.Clear();
            con.Open();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select CaleFisier from Imagini where IDLocalitate = (select IDLocalitate from Localitati WHERE Nume='" + selected + "')", con);
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            { combo2.Items.Add(dt.Rows[i][0].ToString()); }

            con.Close();
        }

        private void combo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (combo2.SelectedItem != null)
            {
                string selected = this.combo2.GetItemText(this.combo2.SelectedItem);
                pictureBox1.Image = Image.FromFile("" + imaginiPath + "/" + selected + "  ");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null) {
                string selected = this.combo2.GetItemText(this.combo2.SelectedItem);
                int YourMax = 10;
                if (listBox1.Items.Count < YourMax)
                {
                    listBox1.Items.Add(selected);
                }
            }
        }
        private void CombineImages(FileInfo[] files)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Images|*.png;*.bmp;*.jpg";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
            }

            int nIndex = 0;

            int width = 0;
            int height = 0;
            int k = 0;


            Bitmap imgfin = new Bitmap(840, 630);

            Graphics g = Graphics.FromImage(imgfin);

            g.Clear(Color.Yellow);

            foreach (FileInfo file in files)
            {

                Image img = Image.FromFile(file.FullName);
                Size sz = new Size(200, 200);
                img = (Image)(new Bitmap(img, sz));
                if (nIndex == 0)
                {

                    g.DrawImage(img, new Point(0, 0));

                    nIndex++;

                    width += img.Width + 10;
                    k++;
                }
                else
                {
                    if (k < 3)
                    {
                        g.DrawImage(img, new Point(width, 0));

                        width += img.Width + 10;
                        k++;
                    }
                    else if (k == 3)
                    {
                        g.DrawImage(img, new Point(width, height));
                        height = img.Height + 10;
                        width = 0;
                        k++;
                    }
                    else if ((k > 3) && (k < 7))
                    {
                        g.DrawImage(img, new Point(width, height));
                        height = img.Height + 10;
                        width += img.Width + 10;
                        k++;
                    }
                    else if (k == 7)
                    {
                        g.DrawImage(img, new Point(width, height));
                        height = 2 * img.Height + 20;
                        width = img.Width;
                        k++;
                    }
                    else if (k > 7)
                    {
                        g.DrawImage(img, new Point(width, height));
                        height = 2 * img.Height + 20;
                        width += img.Width + 10;
                        k++;
                    }
                }
                string textbox = textBox1.Text;
                StringFormat format = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                RectangleF rectf = new RectangleF(20, 30, imgfin.Width, imgfin.Height);
                g.DrawString(textbox, new Font("Tahoma", 60), Brushes.Red, rectf, format);


            }

            g.Dispose();
            pictureBox1.Image = imgfin;
            imgfin.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(constr);
            string selected = this.combo2.GetItemText(this.combo2.SelectedItem);

            DirectoryInfo directory = new DirectoryInfo(Application.StartupPath + @"\..\..\Imagini\");
            FileInfo[] files2 = directory.GetFiles("" + selected + "*");
            if (directory != null)
            {
                FileInfo[] files = new FileInfo[listBox1.Items.Count];
                for (int i = 0; i < listBox1.Items.Count; i++)
                {

                    string result = @"" + directory + "" + listBox1.Items[i] + "";
                    files[i] = new FileInfo(result);


                }
                CombineImages(files);
            }
        }
    }
}
