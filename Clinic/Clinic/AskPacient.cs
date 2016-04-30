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
    public partial class AskPacient : Form
    {
        public AskPacient(string FI, string log, string pass)
        {
            InitializeComponent();
            textBox1.Text = FI;
            login = log;
            password = pass;
        }
        string login = "";
        string password = "";
        string name = "";

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> comboSource = new Dictionary<string, string>();
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            name = textBox1.Text;
            NpgsqlCommand command = new NpgsqlCommand();
            string SQL = "SELECT * FROM pacient WHERE pacient.name = @name";
            command.CommandText = SQL;
            command.Parameters.AddWithValue("@name", name);
            command.Connection = conn;
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    MessageBox.Show("Пациента с таким именем нет в базе!");
                    reader.Close();
                    conn.Close();
                    return;
                }
                while (reader.Read())
                {
                    comboSource.Add(Convert.ToString(reader.GetValue(0)), Convert.ToString(reader.GetValue(2)));
                }
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

            comboBox1.DataSource = new BindingSource(comboSource, null);
            comboBox1.DisplayMember = "Value";
            comboBox1.ValueMember = "Key";

            if (comboSource.Count > 1)
            {
                panel1.Visible = true;
            }
            else
            {
                string key = comboSource.Keys.First();
                string value = comboSource.Values.First();
                AddNote MyForm = new AddNote(key, name, login, password);
                MyForm.Show();
                this.Close();
            }
        }    


        private void button2_Click(object sender, EventArgs e)
        {
            string key = ((KeyValuePair<string, string>)comboBox1.SelectedItem).Key;
            //string value = ((KeyValuePair<string, string>)comboBox1.SelectedItem).Value;
            AddNote MyForm = new AddNote(key, name, login, password);
            MyForm.Show();
            this.Close();
        }
    }
}
