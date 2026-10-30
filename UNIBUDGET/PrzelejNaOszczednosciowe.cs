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
    public partial class PrzelejNaOszczednosciowe : Form
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        string nr_konta;
        GlownaStrona gs;
        public PrzelejNaOszczednosciowe(GlownaStrona gs, string nr_konta)
        {
            InitializeComponent();
            textBox1.Text = gs.PoliczSaldo();
            textBox2.Text = "0,00";
            this.nr_konta = nr_konta;
            this.gs = gs;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(Convert.ToDouble(textBox2.Text) <= Convert.ToDouble(gs.PoliczSaldo()))
            {
                //Odejmuje z głównego konta
                XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
                var worksheet = excel.Worksheet("wydatki");
                int ostatni = worksheet.LastRowUsed().RowNumber();
                worksheet.Cell(ostatni + 1, 1).Value = gs.logowanie.zalogowany; //Cell to kolejno wiersz, kolumna
                worksheet.Cell(ostatni + 1, 2).Value = DateTime.Now;
                worksheet.Cell(ostatni + 1, 3).Value = "Oszczędności";
                worksheet.Cell(ostatni + 1, 4).Value = "Oszczędności";
                worksheet.Cell(ostatni + 1, 5).Value = Convert.ToDouble(textBox2.Text);
                excel.Save();

                //Dodaje na subkonto
                excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
                worksheet = excel.Worksheet("subkonta");
                foreach (var row in worksheet.RowsUsed())
                {
                    if (row.Cell(2).Value.GetText() == nr_konta)
                    {
                        double wplacone = Convert.ToDouble(row.Cell(3).Value.ToString());
                        row.Cell(3).Value = wplacone + Convert.ToDouble(textBox2.Text);
                    }
                }
                excel.Save();
            }
            else
            {
                MessageBox.Show("Brak wystarczających środków na koncie głównym", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            gs.ZmienSaldo();
            gs.LadujSubkonta();
            this.Hide();
        }
    }
}
