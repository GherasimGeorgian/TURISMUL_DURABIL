using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
namespace TURISMUL_DURABIL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string pathImagini = "";
        public static string constr = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Turism.mdf;Integrated Security = True; Connect Timeout = 30";
       
        private void button1_Click(object sender, EventArgs e)
        {
            alegefolder();
            sterge();
            Initializare();
            MessageBox.Show("Initializarea a fost realizata cu succes!");
        }
        public void alegefolder()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            { pathImagini = fbd.SelectedPath; }
        }
        private void sterge()
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from Imagini;DBCC CHECKIDENT ('Imagini',RESEED,0);", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("delete from Localitati;DBCC CHECKIDENT ('Localitati',RESEED,0);", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cmd = new SqlCommand("delete from Planificari;DBCC CHECKIDENT ('Planificari',RESEED,0);", con);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
        private static void Initializare()
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd;
            StreamReader sr = new StreamReader(Application.StartupPath + @"\..\..\planificari.txt");
            string sir;
            char[] split = { '*' };
            con.Open();
            DateTime dt1, dt2;
            while ((sir = sr.ReadLine()) != null)
            {
                string[] siruri = sir.Split(split);
                cmd = new SqlCommand("insert into localitati(nume) values(@localitate)", con);
                cmd.Parameters.AddWithValue("localitate", siruri[0].Trim());
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("select idlocalitate from localitati where nume=@nume", con);
                cmd.Parameters.AddWithValue("nume", siruri[0].Trim());
                int idlocalitate = Convert.ToInt32(cmd.ExecuteScalar());
                int nrzile;
                switch (siruri[1].Trim())
                {
                    case "ocazional":
                        string d1 = siruri[2], d2 = siruri[3];
                        dt1 = Convert.ToDateTime(d1.Trim(), System.Globalization.CultureInfo.GetCultureInfo("fr-FR"));
                        dt2 = Convert.ToDateTime(d2.Trim(), System.Globalization.CultureInfo.GetCultureInfo("fr-FR"));
                        int i = 4;
                        while (i < siruri.Length)
                        {
                            cmd = new SqlCommand(@"insert into imagini(idlocalitate,calefisier) values (@idlocalitate, @calefisier)", con);
                            cmd.Parameters.AddWithValue("idlocalitate", idlocalitate);
                            cmd.Parameters.AddWithValue("calefisier", siruri[i].Trim());
                            cmd.ExecuteNonQuery();
                            i++;
                        }
                        cmd = new SqlCommand(@"insert into planificari(idlocalitate,frecventa,datastart,datastop) values (@idlocalitate,@frecventa,@datastart,@datastop)", con);
                        cmd.Parameters.AddWithValue("idlocalitate", idlocalitate);
                        cmd.Parameters.AddWithValue("frecventa", "ocazional");
                        cmd.Parameters.AddWithValue("datastart", dt1);
                        cmd.Parameters.AddWithValue("datastop", dt2);
                        cmd.ExecuteNonQuery();

                        break;

                    case "anual":
                        nrzile = int.Parse(siruri[2]);
                        i = 3;
                        while (i < siruri.Length)
                        {
                            cmd = new SqlCommand(@"insert into imagini(idlocalitate,calefisier) values (@idlocalitate, @calefisier)", con);
                            cmd.Parameters.AddWithValue("idlocalitate", idlocalitate);
                            cmd.Parameters.AddWithValue("calefisier", siruri[i].Trim());
                            cmd.ExecuteNonQuery();
                            i++;
                        }
                        cmd = new SqlCommand(@"insert into planificari(idlocalitate,frecventa,ziua) values (@idlocalitate,@frecventa,@ziua)", con);
                        cmd.Parameters.AddWithValue("idlocalitate", idlocalitate);
                        cmd.Parameters.AddWithValue("frecventa", "anual");
                        cmd.Parameters.AddWithValue("ziua", nrzile);
                        cmd.ExecuteNonQuery();
                        break;
                    case "lunar":
                        nrzile = int.Parse(siruri[2]);
                        i = 3;
                        while (i < siruri.Length)
                        {
                            cmd = new SqlCommand(@"insert into imagini(idlocalitate,calefisier) values (@idlocalitate, @calefisier)", con);
                            cmd.Parameters.AddWithValue("idlocalitate", idlocalitate);
                            cmd.Parameters.AddWithValue("calefisier", siruri[i].Trim());
                            cmd.ExecuteNonQuery();
                            i++;
                        }
                        cmd = new SqlCommand(@"insert into planificari(idlocalitate,frecventa,ziua) values (@idlocalitate,@frecventa,@ziua)", con);
                        cmd.Parameters.AddWithValue("idlocalitate", idlocalitate);
                        cmd.Parameters.AddWithValue("frecventa", "lunar");
                        cmd.Parameters.AddWithValue("ziua", nrzile);
                        cmd.ExecuteNonQuery();
                        break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 w = new Form2();
            w.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 w = new Form3();
            w.Show();
        }
    }
}
