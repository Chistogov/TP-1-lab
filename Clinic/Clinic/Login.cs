using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clinic
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool islogin = false;
            string role = "";
            try
            {                
                if (comboBox1.Text == "Администратор")
                    role = "Admin";
                else
                    if (comboBox1.Text == "Врач")
                        role = "Doctor";
                    else
                        if (comboBox1.Text == "Гость")
                        role = "Guest";
                NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + role + ";Password=" + textBox1.Text + ";Database=clinic;");
                conn.Open();
                conn.Close(); //Закрываем соединение.
                islogin = true;
            }
            catch (Exception t)
            {
                islogin = false;
                MessageBox.Show("Введены неверные данные!");
            }
            if (islogin == true)
            {
                Form1 MyForm = new Form1(role, textBox1.Text);
                MyForm.Show();
                this.Hide();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Гость")
                textBox1.Text = "Guest";
        }
    }
}
