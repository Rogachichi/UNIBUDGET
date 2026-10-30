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
    public partial class NowyCel : Form
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        GlownaStrona gs;

        public NowyCel(GlownaStrona gs)
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
            string tytul = textBox1.Text;
            string kwota = textBox2.Text;


            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("oszczędności");
            int ostatni = worksheet.LastRowUsed().RowNumber();
            worksheet.Cell(ostatni + 1, 1).Value = gs.logowanie.zalogowany; //Cell to kolejno wiersz, kolumna
            worksheet.Cell(ostatni + 1, 2).Value = tytul;
            worksheet.Cell(ostatni + 1, 3).Value = Convert.ToDouble(kwota);
            worksheet.Cell(ostatni + 1, 4).Value = 0.0;
            worksheet.Cell(ostatni + 1, 5).Value = "Otwarty";
            excel.Save();
            MessageBox.Show("Dodano nowy cel");

            this.Hide();
            gs.LadujOszczednosci();
            gs.ZmienSaldo();
            gs.Show();
        }
    }
}
