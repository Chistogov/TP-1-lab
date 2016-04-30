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
    public partial class AltForm : Form
    {
        public AltForm(string log, string pass, string tbl, string ID)
        {
            InitializeComponent();
            login = log;
            password = pass;
            table = tbl;
            id = ID;
        }
        string login = "";
        string password = "";
        string table = "";
        string id = "";
        string id_pacient = "";
        private void AltForm_Load(object sender, EventArgs e)
        {
            if (table == "doctor")
                createDoctor();
            else
            if (table == "pacient")
                createPacient();
            else
            if (table == "drugs")
                createDrugs();
            else
            if (table == "recipe")
                createRecipe();
            else { MessageBox.Show("Ошибка!"); this.Close(); }
            Size point = new Size(555, 150);
            Size = point;
            Point point2 = new Point(27, 80);
            button1.Location = point2;
        }

        private void refreshCB(ComboBox cmb, string table, string row)
        {
            Dictionary<string, string> comboSource = new Dictionary<string, string>();
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand("SELECT id," + row + " FROM " + table, conn);
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

        private void refreshPacient()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand("SELECT pacient.name FROM public.\"pacient\" WHERE id = " + id_pacient, conn);
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    textBox5.Text = reader.GetString(0);
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
        }

        private void createRecipe()
        {
            refreshCB(comboBox3, "doctor", "name");
            refreshCB(comboBox4, "drugs", "title");
            panelRecipe.Visible = true;
            Point point = new Point(12,12);
            panelRecipe.Location = point;
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");           
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.\"" + table + "\" WHERE id = " + id, conn);
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox3.SelectedValue = Convert.ToString(reader.GetInt32(1));
                    id_pacient = Convert.ToString(reader.GetInt32(2));
                    comboBox4.SelectedValue = Convert.ToString(reader.GetInt32(3));
                    textBox6.Text = reader.GetString(4);
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
                refreshPacient();
            }
        }

        private void createDrugs()
        {
            panelDrugs.Visible = true;
            Point point = new Point(12, 12);
            panelDrugs.Location = point;
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.\"" + table + "\" WHERE id = " + id, conn);
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                int i = reader.VisibleFieldCount;
                if (i > 0 && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        textBox3.Text = reader.GetString(1);
                        textBox4.Text = reader.GetString(2);
                    }
                }
            else { MessageBox.Show("Ошибка!"); Close(); reader.Close(); conn.Close(); }
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

        private void createPacient()
        {
            panelPacient.Visible = true;
            Point point = new Point(12, 12);
            panelPacient.Location = point;
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.\"" + table + "\" WHERE id = " + id, conn);
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                int i = reader.VisibleFieldCount;
                if (i > 0 && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        textBox2.Text = reader.GetString(1);
                        dateTimePicker1.Value = reader.GetDateTime(2);
                        numericUpDown1.Value = reader.GetInt32(3);
                        numericUpDown2.Value = reader.GetInt32(4);
                    }
                }
                else { MessageBox.Show("Ошибка!"); Close(); reader.Close(); conn.Close(); }
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

        private void createDoctor()
        {
            panelDoctor.Visible = true;
            Point point = new Point(12, 12);
            panelDoctor.Location = point;
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.\"" + table + "\" WHERE id = " + id, conn);
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                int i = reader.VisibleFieldCount;
                if (i > 0 && reader.HasRows)
                {
                    while (reader.Read())
                    {
                        textBox1.Text = reader.GetString(1);
                        comboBox1.Items.Add(reader.GetString(2));
                        comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                        comboBox2.Items.Add(reader.GetString(3));
                        comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
                    }
                }
                else { MessageBox.Show("Ошибка!"); Close(); reader.Close(); conn.Close(); }
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (table == "doctor")
            {
                NpgsqlCommand command = new NpgsqlCommand();
                string SQL = "UPDATE public.doctor SET name = @name, category = @category, specialization = @specialization WHERE doctor.id = @id;";
                command.CommandText = SQL;
                command.Parameters.AddWithValue("@name", textBox1.Text);
                command.Parameters.AddWithValue("@category", comboBox1.Text);
                command.Parameters.AddWithValue("@specialization", comboBox2.Text);
                command.Parameters.AddWithValue("@id", id);
                altValue(command);
            }
            else
            if (table == "pacient")
            {
                NpgsqlCommand command = new NpgsqlCommand();
                string SQL = "UPDATE public.pacient SET name = @name, birthdate = @birthdate, weight = @weight, growth = @growth WHERE pacient.id = @id;";
                command.CommandText = SQL;
                command.Parameters.AddWithValue("@name", textBox2.Text);
                command.Parameters.AddWithValue("@birthdate", dateTimePicker1.Text);
                command.Parameters.AddWithValue("@weight", numericUpDown1.Value);
                command.Parameters.AddWithValue("@growth", numericUpDown2.Value);
                command.Parameters.AddWithValue("@id", id);
                altValue(command);
            }
            else
            if (table == "drugs")
            {
                NpgsqlCommand command = new NpgsqlCommand();
                string SQL = "UPDATE public.drugs SET title = @title, manufacturer = @manufacturer WHERE drugs.id = @id;";
                command.CommandText = SQL;
                command.Parameters.AddWithValue("@title", textBox3.Text);
                command.Parameters.AddWithValue("@manufacturer", textBox4.Text);
                command.Parameters.AddWithValue("@id", id);
                altValue(command);
            }
            else
            if (table == "recipe")
            {
                NpgsqlCommand command = new NpgsqlCommand();
                string SQL = "UPDATE public.recipe SET id_doctor = @id_doctor, id_drugs = @id_drugs, dosage = @dosage WHERE recipe.id = @id;";
                command.CommandText = SQL;
                command.Parameters.AddWithValue("@id_doctor", ((KeyValuePair<string, string>)comboBox3.SelectedItem).Key);
                command.Parameters.AddWithValue("@id_drugs", ((KeyValuePair<string, string>)comboBox4.SelectedItem).Key);
                command.Parameters.AddWithValue("@dosage", textBox6.Text);
                command.Parameters.AddWithValue("@id", id);
                altValue(command);
            }
        }

        private void altValue(NpgsqlCommand command)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
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
                Close();
            }
        }

    }
}
