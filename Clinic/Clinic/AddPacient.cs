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
    public partial class AddPacient : Form
    {
        public AddPacient(string log, string pass)
        {
            InitializeComponent();
            login = log;
            password = pass;
        }
        string login = "";
        string password = "";

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") { MessageBox.Show("Введите Фамилию и Имя пациента!"); return; }
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand();
            string SQL = "INSERT INTO public.pacient(name, birthdate, weight, growth, is_del) VALUES(@name, @birthdate, @weight, @growth, false); ";
            command.CommandText = SQL;
            command.Parameters.AddWithValue("@name", textBox1.Text);
            command.Parameters.AddWithValue("@birthdate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@weight", numericUpDown1.Value);
            command.Parameters.AddWithValue("@growth", numericUpDown2.Value);
            command.Connection = conn;
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                reader.Close();
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
