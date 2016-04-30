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
    public partial class AddDrug : Form
    {
        public AddDrug(string log, string pass)
        {
            InitializeComponent();
            login = log;
            password = pass;
        }
        string login = "";
        string password = "";

        private void button1_Click(object sender, EventArgs e)
        {
            string title = textBox1.Text;
            string manufacturer = textBox2.Text;
            if (title == "" || manufacturer == "") { MessageBox.Show("Заполните все поля!"); return; }
            DialogResult result = new DialogResult();
            result = MessageBox.Show(title + " - " + manufacturer + "\nДобавить запись?", "Проверьте правильность", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
                NpgsqlCommand command = new NpgsqlCommand();
                string SQL = "INSERT INTO public.drugs(title, manufacturer, is_del) VALUES(@title, @manufacturer, false);";
                command.CommandText = SQL;
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@manufacturer", manufacturer);
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
