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
    public partial class AddNote : Form
    {
        public AddNote(string FI, string value, string log, string pass)
        {
            InitializeComponent();
            textBox1.Text = FI;
            login = log;
            password = pass;
            id_pacient = FI;
            textBox1.Text = value;
        }
        string login = "";
        string password = "";
        string id_pacient = "";

        private void button1_Click(object sender, EventArgs e)
        {
            string keyDoctor = ((KeyValuePair<string, string>)comboBox1.SelectedItem).Key;
            string keyDrugs = ((KeyValuePair<string, string>)comboBox2.SelectedItem).Key;
            if (textBox2.Text == "") { MessageBox.Show("Укажите дозировку"); return; }
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand();
            string SQL = "INSERT INTO public.recipe(id_doctor, id_pacient, id_drugs, dosage, is_del) VALUES(@id_doctor, @id_pacient, @id_drugs, @dosage, false); ";
            command.CommandText = SQL;
            command.Parameters.AddWithValue("@id_doctor", keyDoctor);
            command.Parameters.AddWithValue("@id_pacient", id_pacient);
            command.Parameters.AddWithValue("@id_drugs", keyDrugs);
            command.Parameters.AddWithValue("@dosage", textBox2.Text);
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
            MessageBox.Show("Запись успешно добавлена!");
            this.Close();
        }

        private void refreshCB(ComboBox cmb, string table, string row)
        {
            Dictionary<string, string> comboSource = new Dictionary<string, string>();
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand("SELECT id," + row + " FROM "+ table, conn);
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        comboSource.Add(Convert.ToString(reader.GetValue(0)), Convert.ToString(reader.GetValue(1)));
                    }
                }
                else { MessageBox.Show("Ошибка при загрузке!"); return; }
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
            cmb.DataSource = new BindingSource(comboSource, null);
            cmb.DisplayMember = "Value";
            cmb.ValueMember = "Key";

        }

        private void AddNote_Load(object sender, EventArgs e)
        {
            refreshCB(comboBox1,"doctor","name");
            refreshCB(comboBox2, "drugs", "title");
        }
    }
}
