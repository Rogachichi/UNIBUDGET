using ClosedXML.Excel;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace BudgetMenagerDEMO
{
    public partial class NowaWplata : Form
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        GlownaStrona gs;
        public NowaWplata(GlownaStrona gs)
        {
            InitializeComponent();
            this.gs = gs;
        }

        protected override void OnClosed(EventArgs e)
        {
            this.Hide();
            gs.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data = monthCalendar1.SelectionRange.Start.ToString("dd.MM.yyyy");
            string tytul = textBox1.Text;
            string kwota = textBox2.Text;

            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Wypełnij wszystkie pola!");
                return;
            }
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("wpłaty");
            int ostatni = worksheet.LastRowUsed().RowNumber();
            worksheet.Cell(ostatni + 1, 1).Value = gs.logowanie.zalogowany; //Cell to kolejno wiersz, kolumna
            worksheet.Cell(ostatni + 1, 2).Value = DateTime.Parse(data);
            worksheet.Cell(ostatni + 1, 3).Value = tytul;
            worksheet.Cell(ostatni + 1, 4).Value = Convert.ToDouble(kwota);
            excel.Save();
            MessageBox.Show("Dodano nowy przychód");

            this.Hide();
            gs.LadujWplaty();
            gs.ZmienSaldo();
            gs.Show();
        }
    }
}
