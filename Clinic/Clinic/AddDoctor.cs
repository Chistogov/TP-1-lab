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
    public partial class AddDoctor : Form
    {
        public AddDoctor(string log, string pass)
        {
            InitializeComponent();
            login = log;
            password = pass;
        }
        string login = "";
        string password = "";

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string category = comboBox1.Text;
            string spec = comboBox2.Text;
            if (name == "" || category == "" || spec == "") { MessageBox.Show("Заполните все поля!"); return; }
            DialogResult result = new DialogResult();
            result = MessageBox.Show(name + " - " + category + " - " + spec + "\nДобавить запись?", "Проверьте правильность", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
                NpgsqlCommand command = new NpgsqlCommand();
                string SQL = "INSERT INTO public.doctor(name, category, specialization, is_del) VALUES(@name, @category, @specialization, false); ";
                command.CommandText = SQL;
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@category", category);
                command.Parameters.AddWithValue("@specialization", spec);
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
}
