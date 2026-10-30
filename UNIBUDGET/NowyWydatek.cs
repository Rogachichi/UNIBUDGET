using BudgetMenagerDEMO;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNIBUDGET
{
    public partial class NowyWydatek : Form
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        GlownaStrona gs;

        public NowyWydatek(GlownaStrona gs)
        {
            InitializeComponent();
            comboBox1.Items.Add("Jedzenie");
            comboBox1.Items.Add("Zdrowie");
            comboBox1.Items.Add("Paliwo");
            comboBox1.Items.Add("Podatki");
            comboBox1.Items.Add("Rozrywka");
            comboBox1.Items.Add("Inne");
            this.gs = gs;
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Hide();
            gs.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null || textBox1.Text== "" || textBox2.Text=="") 
            {
                MessageBox.Show("Wypełnij wszystkie pola!");
                return;
            }
            string data = monthCalendar1.SelectionRange.Start.ToString("dd.MM.yyyy");
            string tytul = textBox1.Text;
            string kwota = textBox2.Text;
            string kategoria = comboBox1.SelectedItem.ToString();

            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("wydatki");
            int ostatni = worksheet.LastRowUsed().RowNumber();
            worksheet.Cell(ostatni + 1, 1).Value = gs.logowanie.zalogowany; //Cell to kolejno wiersz, kolumna
            worksheet.Cell(ostatni + 1, 2).Value = DateTime.Parse(data);
            worksheet.Cell(ostatni + 1, 3).Value = kategoria;
            worksheet.Cell(ostatni + 1, 4).Value = tytul;
            worksheet.Cell(ostatni + 1, 5).Value = Convert.ToDouble(kwota);
            excel.Save();
            MessageBox.Show("Dodano nowy wydatek");

            this.Hide();
            gs.LadujWydatki();
            gs.ZmienSaldo();
            gs.Show();
        }
    }
}
