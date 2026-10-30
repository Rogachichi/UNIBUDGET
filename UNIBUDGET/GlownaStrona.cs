using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using UNIBUDGET;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BudgetMenagerDEMO
{
    public partial class GlownaStrona : Form
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        
        public PanelLogowania logowanie;

        //Zamyka aplikację łącznie z ukrytym formsem od logowania
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            MessageBox.Show("Wylogowano pomyślnie");
            logowanie.Show();
            this.Hide();
        }

        public GlownaStrona(PanelLogowania logowanie)
        {
            InitializeComponent();
            this.logowanie = logowanie;

            Glowna();
            Wplaty();
            Wydatki();
            Raporty();
            Oszczednosci();
            Subkonta();
        }

        private void Glowna()
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            label5.Text = "Witaj, " + logowanie.zalogowany + "! Miło Cię widzieć :)";
            ZmienSaldo();
        }

        public void ZmienSaldo()
        {
            double wplaty = 0.0, wydatki = 0.0;
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("wpłaty");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                {
                    wplaty += (double)row.Cell(4).Value;
                }
            }

            worksheet = excel.Worksheet("wydatki");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                {
                    wydatki += (double)row.Cell(5).Value;
                }
            }
            label7.Text = PoliczSaldo();
            label10.Text = wplaty.ToString();
            label13.Text = wydatki.ToString();
            textBox1.Text = PoliczSaldo();
            if ((wplaty - wydatki) > 0)
            {
                label7.ForeColor = Color.Green;
            }
            else if ((wplaty - wydatki) < 0)
            {
                label7.ForeColor = Color.Red;
            }
        }

        public string PoliczSaldo()
        {
            double wplaty = 0.0, wydatki = 0.0;
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("wpłaty");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                {
                    wplaty += (double)row.Cell(4).Value;
                }
            }

            worksheet = excel.Worksheet("wydatki");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                {
                    wydatki += (double)row.Cell(5).Value;
                }
            }

            return (wplaty - wydatki).ToString();
        }

        private void Wplaty()
        {
            button2.BackColor = Color.FromArgb(0, 162, 232);
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderColor = Color.Black;
            button2.FlatAppearance.BorderSize = 1;
            LadujWplaty();
        }

        private void Wydatki()
        {
            button3.BackColor = Color.FromArgb(0, 162, 232);
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderColor = Color.Black;
            button3.FlatAppearance.BorderSize = 1;
            LadujWydatki();
        }

        private void Raporty()
        {
            kategoriewybieranie.Items.Add("Przychody");
            kategoriewybieranie.Items.Add("Wydatki");
            kategoriewybieranie.Items.Add("Wszystko");
            kategoriewybieranie.SelectedIndex = 0;
        }

        private void WykresDochody()
        {
            chart2.Legends.Clear();

            Series przychodySeria = new Series("Przychody")
            {
                ChartType = SeriesChartType.Column,
                BorderWidth = 2
            };

            double suma = 0;
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("wpłaty");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                {
                    suma += Convert.ToDouble(row.Cell(4).Value.ToString());
                }
            }


            przychodySeria.Points.AddXY("Przychód",suma);

            // Dodajemy serię danych do wykresu
            chart2.Series.Clear();  // Najpierw usuwamy stare serie, aby uniknąć konfliktów
            chart2.Series.Add(przychodySeria);

            // Ustawienia wykresu
            chart2.ChartAreas[0].AxisY.Title = "Kwota";
            chart2.ChartAreas[0].AxisY.Minimum = double.NaN; // Ustawienie automatycznego minimum
            chart2.ChartAreas[0].AxisY.Maximum = double.NaN; // Ustawienie automatycznego maksimum
            chart2.ChartAreas[0].RecalculateAxesScale(); // Wymuszenie ponownego przeliczenia skali
        }

        private void WykresWydatki()
        {
            chart2.Legends.Clear();
            var wydatkiKategorie = new Dictionary<string, double>();
            Series wydatkiSeria = new Series("Przychody")
            {
                ChartType = SeriesChartType.Column,
                BorderWidth = 2
            };

            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("wydatki");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                {
                    string kategoria = row.Cell(3).Value.ToString();
                    if (!wydatkiKategorie.ContainsKey(kategoria))
                    {
                        wydatkiKategorie.Add(kategoria, Convert.ToDouble(row.Cell(5).Value.ToString()));
                    }
                    else
                    {
                        wydatkiKategorie[kategoria] += Convert.ToDouble(row.Cell(5).Value.ToString());
                    }
                    //suma += Convert.ToDouble(row.Cell(5).Value.ToString());
                }
            }

            foreach (var kategoria in wydatkiKategorie.Keys)
            {
                wydatkiSeria.Points.AddXY(kategoria, wydatkiKategorie[kategoria]);
            }

            chart2.Series.Clear();
            chart2.Series.Add(wydatkiSeria);

            chart2.ChartAreas[0].AxisX.Title = "Kategorie";
            chart2.ChartAreas[0].AxisY.Title = "Kwota";

            chart2.ChartAreas[0].AxisY.Minimum = double.NaN; // Ustawienie automatycznego minimum
            chart2.ChartAreas[0].AxisY.Maximum = double.NaN; // Ustawienie automatycznego maksimum
            chart2.ChartAreas[0].RecalculateAxesScale(); // Wymuszenie ponownego przeliczenia skali
        }

        private void WykresWszystko()
        {
            chart2.Legends.Clear();
            var wydatkiKategorie = new Dictionary<string, double>();
            Series wydatkiSeria = new Series("Przychody")
            {
                ChartType = SeriesChartType.Column,
                BorderWidth = 2
            };

            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("wydatki");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                {
                    string kategoria = row.Cell(3).Value.ToString();
                    if (!wydatkiKategorie.ContainsKey(kategoria))
                    {
                        wydatkiKategorie.Add(kategoria, Convert.ToDouble(row.Cell(5).Value.ToString()));
                    }
                    else
                    {
                        wydatkiKategorie[kategoria] += Convert.ToDouble(row.Cell(5).Value.ToString());
                    }
                    //suma += Convert.ToDouble(row.Cell(5).Value.ToString());
                }
            }


            double suma = 0;
            worksheet = excel.Worksheet("wpłaty");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                {
                    suma += Convert.ToDouble(row.Cell(4).Value.ToString());
                }
            }

            foreach (var kategoria in wydatkiKategorie.Keys)
            {
                wydatkiSeria.Points.AddXY(kategoria, wydatkiKategorie[kategoria]);
            }

            for (int i = 0; i < wydatkiSeria.Points.Count; i++)
                wydatkiSeria.Points[i].Color = Color.Red;

            wydatkiSeria.Points.AddXY("Przychód", suma);
            int lastPointIndex = wydatkiSeria.Points.Count - 1;
            wydatkiSeria.Points[lastPointIndex].Color = Color.Green;

            chart2.Series.Clear();
            chart2.Series.Add(wydatkiSeria);

            chart2.ChartAreas[0].AxisX.Title = "Kategorie";
            chart2.ChartAreas[0].AxisY.Title = "Kwota";

            chart2.ChartAreas[0].AxisY.Minimum = double.NaN; // Ustawienie automatycznego minimum
            chart2.ChartAreas[0].AxisY.Maximum = double.NaN; // Ustawienie automatycznego maksimum
            chart2.ChartAreas[0].RecalculateAxesScale(); // Wymuszenie ponownego przeliczenia skali
        }

        private void Oszczednosci()
        {
            LadujOszczednosci();
        }

        public void LadujOszczednosci()
        {
            comboBox1.Items.Clear();
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("oszczędności");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany && row.Cell(5).Value.GetText() == "Otwarty")
                {
                    comboBox1.Items.Add(row.Cell(2).Value.ToString());
                }
            }
        }
        private void Subkonta()
        {
            button4.BackColor = Color.FromArgb(0, 162, 232);
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderColor = Color.Black;
            button4.FlatAppearance.BorderSize = 1;

            button5.BackColor = Color.FromArgb(0, 162, 232);
            button5.FlatStyle = FlatStyle.Flat;
            button5.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderColor = Color.Black;
            button5.FlatAppearance.BorderSize = 1;

            textBox1.Text = PoliczSaldo();
            LadujSubkonta();
        }

        public void LadujSubkonta()
        {
            dataGridView3.Rows.Clear();
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("subkonta");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                {
                    var nr_konta = row.Cell(2).Value.ToString();
                    var saldo = Convert.ToDouble(row.Cell(3).Value.ToString());
                    dataGridView3.Rows.Add(nr_konta, saldo);
                }
            }
        }

        private void dodajSubkonto(object sender, EventArgs e)
        {
            //Generator numeru konta subkonta
            Random rnd = new Random();
            string nr_konta = "";
            for(int i = 0; i < 26; i++)
            {
                nr_konta += rnd.Next(0, 9).ToString();
            }

            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("subkonta");


            int account = 0;
            //Jeżeli jest więcej niż 3 konta, to odmów utworzenia kolejnego
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                    account++;
            }

            if (account >= 3)
            {
                MessageBox.Show("Przekroczono limit dostępnych subkont!", "przekroczono limit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
                

            int ostatni = worksheet.LastRowUsed().RowNumber();
            worksheet.Cell(ostatni + 1, 1).Value = logowanie.zalogowany; //Cell to kolejno wiersz, kolumna
            worksheet.Cell(ostatni + 1, 2).Value = nr_konta;
            worksheet.Cell(ostatni + 1, 3).Value = 0.0;
            excel.Save();
            LadujSubkonta();
        }


        public void DodajWplate(object sender, EventArgs e)
        {
            NowaWplata nowaWplata = new NowaWplata(this);
            nowaWplata.Show();
            this.Hide();
        }

        internal void LadujWplaty()
        {
            dataGridView1.Rows.Clear();
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("wpłaty");
            int i = 1;
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                {
                    var data = ((DateTime)row.Cell(2).Value).ToString("dd.MM.yyyy");
                    var tytul = row.Cell(3).Value.GetText();
                    var wartosc = row.Cell(4).Value;
                    dataGridView1.Rows.Add(i, data, tytul, wartosc);
                    i++;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NowyWydatek nowyWydatek = new NowyWydatek(this);
            nowyWydatek.Show();
            this.Hide();
        }

        internal void LadujWydatki()
        {
            dataGridView2.Rows.Clear();
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("wydatki");
            int i = 1;
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany)
                {
                    var data = ((DateTime)row.Cell(2).Value).ToString("dd.mm.yyyy");
                    var kategoria = row.Cell(3).Value.GetText();
                    var tytul = row.Cell(4).Value.GetText();
                    var wartosc = row.Cell(5).Value;
                    dataGridView2.Rows.Add(i, data, tytul, wartosc, kategoria);
                    i++;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart2.Visible = true;
            string selectedReportType = kategoriewybieranie.SelectedItem.ToString();

            switch (selectedReportType)
            {
                case "Przychody":
                    WykresDochody();
                    break;
                case "Wydatki":
                    WykresWydatki();
                    break;
                case "Wszystko":
                    WykresWszystko();
                    break;
            }
        }

        private void wykresCelu()
        {
            chart1.Visible = true;
            string cel = comboBox1.SelectedItem.ToString();

            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("oszczędności");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany && row.Cell(2).Value.GetText() == cel)
                {
                    chart1.Series.Clear();
                    Series series = new Series("Oszczędności")
                    {
                        ChartType = SeriesChartType.Pie,
                        IsValueShownAsLabel = false,

                    };
                    series.Points.AddXY("Do celu", Convert.ToDouble(row.Cell(3).Value.ToString()) - Convert.ToDouble(row.Cell(4).Value.ToString()));
                    series.Points.AddXY("Wpłacone", Convert.ToDouble(row.Cell(4).Value.ToString()));
                    label18.Text = row.Cell(4).Value.ToString();
                    label16.Text = row.Cell(3).Value.ToString();
                    chart1.Series.Add(series);

                    foreach (var point in series.Points)
                    {
                        point.LegendText = point.AxisLabel; // Ustawienie LegendText na nazwę kategorii
                        point.Label = "";
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label15.Visible = true;
            label16.Visible = true;
            label17.Visible = true;
            label18.Visible = true;
            label19.Visible = true;
            label20.Visible = true;
            button7.Visible = true;
            textBox2.Visible = true;
            wykresCelu();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("oszczędności");


            int goals = 0;
            //Jeżeli jest więcej niż 3 konta, to odmów utworzenia kolejnego
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany && row.Cell(5).Value.GetText() == "Otwarty")
                    goals++;
            }

            if (goals >= 3)
            {
                MessageBox.Show("Przekroczono limit dostępnych celów!", "przekroczono limit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            NowyCel nowycel = new NowyCel(this);
            nowycel.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Double wplata = Convert.ToDouble(textBox2.Text);

            if(wplata> Convert.ToDouble(PoliczSaldo()))
            {
                MessageBox.Show("Nie masz tylu pieniędzy na koncie", "przekroczono limit!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //Odejmuje z głównego konta
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("wydatki");
            int ostatni = worksheet.LastRowUsed().RowNumber();
            worksheet.Cell(ostatni + 1, 1).Value = logowanie.zalogowany; //Cell to kolejno wiersz, kolumna
            worksheet.Cell(ostatni + 1, 2).Value = DateTime.Now;
            worksheet.Cell(ostatni + 1, 3).Value = "Oszczędności";
            worksheet.Cell(ostatni + 1, 4).Value = "Oszczędności";
            worksheet.Cell(ostatni + 1, 5).Value = wplata;
            excel.Save();

            //Wpłaca na cel
            string cel = comboBox1.SelectedItem.ToString();
            excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            worksheet = excel.Worksheet("oszczędności");
            foreach (var row in worksheet.RowsUsed())
            {
                if (row.Cell(1).Value.GetText() == logowanie.zalogowany && row.Cell(2).Value.GetText() == cel)
                {
                    double wplacone = Convert.ToDouble(row.Cell(4).Value.ToString());
                    row.Cell(4).Value = wplacone + wplata;
                }
            }
            excel.Save();
            ZmienSaldo();
            wykresCelu();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int selectedrowindex = dataGridView3.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView3.Rows[selectedrowindex];
            string nr_konta = Convert.ToString(selectedRow.Cells["NrKonta"].Value);
            PrzelejNaOszczednosciowe oszczednosciowe = new PrzelejNaOszczednosciowe(this, nr_konta);
            oszczednosciowe.Show();
        }
    }
}
