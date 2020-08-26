using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TURISMUL_DURABIL
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(Form1.constr);
        private void Form3_Load(object sender, EventArgs e)
        {
            LoadPlanificari();
        }
        private void filtrare()
        {
            BindingSource bs = new BindingSource();

            bs.DataSource = dataGridView2.DataSource;
            bs.Filter = "DataStart >= '" + dateTimePicker1.Value.AddDays(-1) + "' And " + "DataStop <= '" + dateTimePicker2.Value.AddDays(1) + "'";
            dataGridView2.DataSource = bs;
        }
        private void filtrare2()
        {
            BindingSource bs = new BindingSource();

            bs.DataSource = dataGridView2.DataSource;
            bs.Filter = "Nume like 'B%'";
            dataGridView2.DataSource = bs;
        }
        private void LoadPlanificari()
        {
            con.Open();

            DataTable table = new DataTable();

            SqlCommand cmd = new SqlCommand("select l.Nume,p.DataStart,p.DataStop,p.Frecventa,p.Ziua FROM Localitati l,Planificari p WHERE p.IDlocalitate = l.IDLocalitate", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(table);

            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].Name = "Nume";
            dataGridView1.Columns[1].Name = "DataStart";
            dataGridView1.Columns[2].Name = "DataStop";
            dataGridView1.Columns[3].Name = "Frecventa";
            dataGridView1.Columns[4].Name = "Ziua";
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            adaugarevalori();
          

            datagrid3();
        }
        private void datagrid3()
        {
            int ok = 0;
            DataTable table3 = new DataTable();
            table3.Columns.Add("Nume", typeof(string));
            table3.Columns.Add("Data", typeof(DateTime));
            String searchValue2 = "lunar";
            String searchValue3 = "anual";
            String searchValue1 = "ocazional";
            int rowIndex = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {

                if (dataGridView2.Rows[rowIndex].Cells[3].Value.ToString().Equals(searchValue1))
                {

                    string nume = dataGridView2.Rows[rowIndex].Cells[0].Value.ToString();

                    DateTime datastart = Convert.ToDateTime(dataGridView2.Rows[rowIndex].Cells[1].Value.ToString());
                    DateTime datastop = Convert.ToDateTime(dataGridView2.Rows[rowIndex].Cells[2].Value.ToString());
                    int dsdays = datastart.Day;
                    int dtdays = datastop.Day;
                    int dif = dtdays - dsdays;
                    if (ok == 1)
                    {
                        for (int i = 0; i <= dif; i++)
                         {
                         table3.Rows.Add(nume, datastart.AddDays(i));
                         }
                    }
                    else ok++;

                }
                if (dataGridView2.Rows[rowIndex].Cells[3].Value.ToString().Equals(searchValue2))
                {
                    string nume = dataGridView2.Rows[rowIndex].Cells[0].Value.ToString();
                    DateTime data = Convert.ToDateTime(dataGridView2.Rows[rowIndex].Cells[1].Value.ToString());
                    table3.Rows.Add(nume, data);
                }
                if (dataGridView2.Rows[rowIndex].Cells[3].Value.ToString().Equals(searchValue3))
                {
                    string nume = dataGridView2.Rows[rowIndex].Cells[0].Value.ToString();
                    DateTime data = Convert.ToDateTime(dataGridView2.Rows[rowIndex].Cells[1].Value.ToString());
                    table3.Rows.Add(nume, data);
                }



                rowIndex = row.Index;

            }
            table3.DefaultView.Sort = "Data asc";
            table3 = table3.DefaultView.ToTable();
            dataGridView3.DataSource = table3;
        }
        private void adaugarevalori()
        {
            DataTable table2 = new DataTable();

            table2.Columns.Add("Nume", typeof(string));
            table2.Columns.Add("DataStart", typeof(DateTime));
            table2.Columns.Add("DataStop", typeof(DateTime));
            table2.Columns.Add("Frecventa", typeof(string));
            String searchValue = "ocazional";
            String searchValue1 = "anual";
            String searchValue2 = "lunar";
            int rowIndex = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                if (dataGridView1.Rows[rowIndex].Cells[3].Value.ToString().Equals(searchValue))
                {
                    string nume = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();

                    DateTime datastart = Convert.ToDateTime(dataGridView1.Rows[rowIndex].Cells[1].Value.ToString());
                    DateTime datastop = Convert.ToDateTime(dataGridView1.Rows[rowIndex].Cells[2].Value.ToString());


                    //cazul datei intre cele doua picktime
                    if (datastart >= dateTimePicker1.Value.Date && datastop <= dateTimePicker2.Value.Date) {
                        table2.Rows.Add(nume, datastart, datastop, searchValue);
                       
                    }
                   
                    // cazul in care datastart este mai mare si data stop mai mare
                    if (datastart >= dateTimePicker1.Value.Date && datastop >= dateTimePicker2.Value.Date && datastart <= dateTimePicker2.Value.Date)
                    {
                        datastop = dateTimePicker2.Value.Date;
                        if(datastop > datastart)
                        table2.Rows.Add(nume, datastart, datastop, searchValue);
                    }
                    //cazul in care datastart este mai mic si datastop este mai mic
                    if (datastart <= dateTimePicker1.Value.Date && datastop <= dateTimePicker2.Value.Date && datastart <= dateTimePicker2.Value.Date)
                    {
                        datastart = dateTimePicker1.Value.Date;
                        if (datastop > datastart)
                            table2.Rows.Add(nume, datastart, datastop, searchValue);
                    }



                }
                if (dataGridView1.Rows[rowIndex].Cells[3].Value.ToString().Equals(searchValue1))
                {

                    int an_d1 = dateTimePicker1.Value.Year;
                    int an_d2 = dateTimePicker2.Value.Year;
                    int dif = an_d2 - an_d1;
                    string nume = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    int doy = Int32.Parse(dataGridView1.Rows[rowIndex].Cells[4].Value.ToString()) - 1;

                    DateTime value = new DateTime(an_d1, 1, 1);
                    
                    for (int i = 0; i <= dif; i++)
                    {
                        DateTime convertedDate = value.AddYears(i).AddDays(doy);
                        
                        if(convertedDate >= dateTimePicker1.Value.Date && convertedDate <= dateTimePicker2.Value.Date)
                        table2.Rows.Add(nume, convertedDate, convertedDate, searchValue1);

                    }
                }
                if (dataGridView1.Rows[rowIndex].Cells[3].Value.ToString().Equals(searchValue2))
                {

                    int an_d1 = dateTimePicker1.Value.Year;
                    int an_d2 = dateTimePicker2.Value.Year;
                    int dif = an_d2 - an_d1;
                    string nume = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                    int luna = Int32.Parse(dataGridView1.Rows[rowIndex].Cells[4].Value.ToString()) - 1;

                    DateTime value = new DateTime(an_d1, 1, 1);
                    

                    for (int i = 0; i <= dif; i++)
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            DateTime xxx = value.AddYears(i).AddMonths(j);
                            int days = DateTime.DaysInMonth(xxx.Year, xxx.Month);

                            if (days >= luna) {
                                DateTime convertedDate = value.AddYears(i).AddMonths(j).AddDays(luna);
                                if (convertedDate >= dateTimePicker1.Value.Date && convertedDate <= dateTimePicker2.Value.Date)
                                    table2.Rows.Add(nume, convertedDate, convertedDate, searchValue2);
                            }
                        }
                    }

                }
                rowIndex = row.Index;
            }
            dataGridView2.DataSource = table2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count > 0)
            { timer1.Start(); }
            else
            {
                MessageBox.Show("Genereaza o excursie!");
            }
        }
        int x = 0;
        int rowIndex = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (rowIndex >= dataGridView3.Rows.Count - 1)
            {
                rowIndex = 0;
            }

            string nume = dataGridView3.Rows[rowIndex].Cells[0].Value.ToString();

            string[] files = Directory.GetFiles(Application.StartupPath + @"\..\..\Imagini\", "*" + nume + "*.*");

            if (x >= files.Length)
            {
                x = 0;
            }

            pictureBox1.Image = Image.FromFile(files[x]);

            DateTime datapickstart = Convert.ToDateTime(dateTimePicker1.Value.ToString());
            int n = Convert.ToInt32(datapickstart.ToString("yyyyMMdd"));
            DateTime datapickstop = Convert.ToDateTime(dateTimePicker2.Value.ToString());
            int m = Convert.ToInt32(datapickstop.ToString("yyyyMMdd"));

            progressBar1.Minimum = n;
            progressBar1.Maximum = m;



            DateTime data = Convert.ToDateTime(dataGridView3.Rows[rowIndex].Cells[1].Value.ToString());
            string startdate = data.ToString("dd/MM/yyyy");
            int y = Convert.ToInt32(data.ToString("yyyyMMdd"));



            label3.Text = nume.ToString();
            label4.Text = startdate.ToString();
            progressBar1.Value = y;



            rowIndex++; x++;
        }
    }
}
