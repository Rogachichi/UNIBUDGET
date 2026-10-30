using System;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using System.Windows.Forms;
using ClosedXML.Excel;

namespace BudgetMenagerDEMO
{
    public partial class PanelLogowania : Form
    {
        public string zalogowany;

        public PanelLogowania()
        {
            InitializeComponent();
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            button1.BackColor = Color.FromArgb(0, 162, 232);
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderColor = Color.Black;
            button1.FlatAppearance.BorderSize = 1;

            button2.BackColor = Color.FromArgb(0, 162, 232);
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderColor = Color.Black;
            button2.FlatAppearance.BorderSize = 1;

            tbPassword.UseSystemPasswordChar = true;


            //Œcie¿ka w projekcie
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;


            XLWorkbook excel = new XLWorkbook(projectDirectory+"/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("u¿ytkownicy");

            foreach (var row in worksheet.RowsUsed())
            {
                //login = row.Cell(1).GetValue<string>();
                //has³o = row.Cell(2).GetValue<string>();
            }

        }
        private bool findUser(string login, string haslo)
        {

            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.FullName;

            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("u¿ytkownicy");

            foreach (var row in worksheet.RowsUsed())
            {
                if (login == row.Cell(1).GetValue<string>() && haslo == row.Cell(2).GetValue<string>())
                    return true;
            }

            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (findUser(tbLogin.Text, tbPassword.Text))
            {
                MessageBox.Show("Zalogowano pomyœlnie");
                this.zalogowany = tbLogin.Text;
                GlownaStrona glowna = new GlownaStrona(this);
                glowna.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wprowadzono z³y login lub has³o","B³êdny login lub has³o", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Rejestracja rej = new Rejestracja(this);
            rej.Show();
            this.Hide();
        }

    }
}
