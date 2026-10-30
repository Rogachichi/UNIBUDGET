using ClosedXML.Excel;
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace BudgetMenagerDEMO
{
    public partial class Rejestracja : Form
    {
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        private PanelLogowania logowanie;
        public Rejestracja(PanelLogowania pl)
        {
            InitializeComponent();
            logowanie = pl;

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
            haslo1.UseSystemPasswordChar = true;
            haslo2.UseSystemPasswordChar = true;

        }

        protected override void OnClosed(EventArgs e)
        {
            powrot(this, e);
        }

        private void powrot(object sender, EventArgs e)
        {
            logowanie.Show();
            this.Hide();
        }

        private void rejestracja(object sender, EventArgs e)
        {
            if (isUserAlreadyExist())
            {
                MessageBox.Show("Konto o takim loginie lub emailu już istnieje!", "Konto już istnieje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if(!haslo1.Text.Equals(haslo2.Text))
            {
                MessageBox.Show("Podane hasła są różne","Hasła się różnią", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }else if (login.Text=="" || email.Text=="" || haslo1.Text == "" || haslo2.Text == "")
            {
                MessageBox.Show("Conajmniej jedno pole jest puste", "Pominięto pole", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("użytkownicy");
            int ostatni = worksheet.LastRowUsed().RowNumber();
            worksheet.Cell(ostatni + 1, 1).Value = login.Text; //Cell to kolejno wiersz, kolumna
            worksheet.Cell(ostatni + 1, 2).Value = haslo1.Text;
            worksheet.Cell(ostatni + 1, 3).Value = email.Text;
            excel.Save();
            MessageBox.Show("Pomyślnie utworzono nowe konto!");
            logowanie.Show();
            this.Hide();
        }

        private bool isUserAlreadyExist()
        {
            XLWorkbook excel = new XLWorkbook(projectDirectory + "/UNIBUDGET.xlsx");
            var worksheet = excel.Worksheet("użytkownicy");

            foreach (var row in worksheet.RowsUsed())
            {
                if (login.Text == row.Cell(1).GetValue<string>() || haslo1.Text == row.Cell(3).GetValue<string>())
                    return true;
            }
            return false;
        }
        
}
}
