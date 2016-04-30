using Npgsql;
using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace Clinic
{
    public partial class Form1 : Form
    {
        public Form1(string log, string pass)
        {
            InitializeComponent();
            login = log;
            password = pass;          
        }
        string login = "";
        string password = "";
        int PagesCount = 0;
        int page = 1;
        string limit = "";
        string lastQuery = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            refreshCB();
            comboBox1.Text = "recipe";
            if (login != "Admin")
            {
                //comboBox1.Enabled = false;
                управлениеToolStripMenuItem.Visible = false;
            }
            if (login == "Guest")
            {
                файлToolStripMenuItem.Visible = false;
            }
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            refreshCBDoctor();
            comboBox2.Items.Add("");
            page = 0;
            page = 1;
            limit = "LIMIT "+ numericUpDown1.Value.ToString() +" OFFSET 0";
            showTable();
        }
        //Количество записей
        private void rowCount()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand(lastQuery, conn);
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                int n = 0;
                while (reader.Read())
                {
                    //MessageBox.Show(reader.GetValue(0).ToString());                    
                    n++;
                }
                double t = n / Convert.ToDouble(numericUpDown1.Value);
                PagesCount = Convert.ToInt32(Math.Ceiling(t));
                labelPages.Text = Convert.ToString(page) + "/" + Convert.ToString(PagesCount);
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
        }
        //Добавление в список таблиц
        private void refreshCB()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id="+login+";Password="+password+";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand("SELECT table_name FROM information_schema.tables WHERE table_schema = 'public'", conn);
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                   comboBox1.Items.Add(reader.GetString(0));
                }
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }
            finally
            {
                reader.Close();
                conn.Close();
            }
        }
        //Добавление в список врачей
        private void refreshCBDoctor()
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand("SELECT name FROM doctor", conn);
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader.GetString(0));
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
        //Показать таблицу
        private void showTable()
        {
            if (comboBox1.Text != "")
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id="+login+";Password="+password+";Database=clinic;");
                string sql = "";
                
                    sql = "SELECT * FROM public.\"" + comboBox1.Text + "\"";
                if (comboBox1.Text == "recipe")
                {
                    sql = "SELECT recipe.id, doctor.name as Доктор, pacient.name as Пациент, drugs.title as Лекарство, recipe.dosage as Дозировка FROM recipe, doctor, drugs, pacient WHERE (doctor.id = recipe.id_doctor) and (drugs.id = recipe.id_drugs) and (pacient.id = recipe.id_pacient) and (recipe.is_del != TRUE)";
                    if (comboBox2.Text != "")
                        sql += " and (doctor.name = \'" + comboBox2.Text + "\')";
                    if (textBox1.Text != "")
                        sql += " and (pacient.name = \'" + textBox1.Text + "\')";
                    if (textBox2.Text != "")
                        sql += " and (drugs.title = \'" + textBox2.Text + "\')";
                    if (textBox3.Text != "")
                        sql += " and (recipe.dosage = \'" + textBox3.Text + "\')";
                }
                if (comboBox1.Text == "doctor")
                {
                    sql = "SELECT id, name as Доктор, category as Категория, specialization as Специализация FROM public.doctor WHERE (doctor.is_del != TRUE) ";
                    if (textBox6.Text != "")
                        sql += " and (doctor.name = \'" + textBox6.Text + "\')";
                    if (textBox5.Text != "")
                        sql += " and (doctor.category = \'" + textBox5.Text + "\')";
                    if (textBox4.Text != "")
                        sql += " and (doctor.specialization = \'" + textBox4.Text + "\')";
                }
                if (comboBox1.Text == "pacient")
                {
                    sql = "SELECT id, name as Имя, birthdate as Дата_Рождения, weight as Вес, growth as Рост FROM public.pacient WHERE (pacient.is_del != TRUE) ";
                    if (textBox9.Text != "")
                        sql += " and (pacient.name = \'" + textBox9.Text + "\')";
                    if (dateTimePicker1.Checked)
                        sql += " and (pacient.birthdate = \'" + dateTimePicker1.Value + "\')";
                    if (textBox7.Text != "")
                        sql += " and (pacient.weight = \'" + textBox7.Text + "\')";
                    if (textBox10.Text != "")
                        sql += " and (pacient.growth = \'" + textBox10.Text + "\')";
                }
                if (comboBox1.Text == "drugs")
                {
                    sql = "SELECT id, title as Наименование, manufacturer as Производитель FROM public.drugs WHERE (drugs.is_del != TRUE) ";
                    if (textBox12.Text != "")
                        sql += " and (drugs.title = \'" + textBox12.Text + "\')";
                    if (textBox11.Text != "")
                        sql += " and (drugs.manufacturer = \'" + textBox11.Text + "\')";
                }
                lastQuery = sql;
                sql += limit;                
                //Конец составления запроса
                NpgsqlCommand command = new NpgsqlCommand(sql,conn);
                NpgsqlDataReader reader = null;
                try
                {
                    conn.Open();
                    reader = command.ExecuteReader();
                    int i = reader.VisibleFieldCount;
                    if (i > 0 && reader.HasRows)
                    {
                        dataGridView1.Rows.Clear();
                        dataGridView1.Columns.Clear();
                        for (int l = 0; l < i; l++)
                            dataGridView1.Columns.Add(reader.GetName(l), reader.GetName(l));

                        int row = 0;
                        while (reader.Read())
                        {
                            dataGridView1.Rows.Add();
                            for (int cell = 0; cell < i; cell++)
                                try
                                {
                                    dataGridView1.Rows[row].Cells[cell].Value = (reader.GetValue(cell));
                                }
                                catch { dataGridView1.Rows[row].Cells[cell].Value = ""; }
                            row++;
                    }
                    }
                    else MessageBox.Show("Таких записей не найдено!");
                    reader.Close();

                }
                catch (Exception m)
                {
                    MessageBox.Show(m.Message);
                }
                finally
                {
                    conn.Close();
                    if (dataGridView1.Columns.Count > 0)
                        dataGridView1.Columns[0].Visible = false;
                }
            }
        }
        //Закрытие формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        //Меню 
        private void добавитьВрачаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDoctor MyForm = new AddDoctor(login, password);
            MyForm.Show();
        }

        private void добавитьЛекарствоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDrug MyForm = new AddDrug(login, password);
            MyForm.Show();
        }

        private void добавитьПациентаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPacient MyForm = new AddPacient(login, password);
            MyForm.Show();
        }

        private void выписатьНазначениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AskPacient MyForm = new AskPacient(textBox1.Text, login, password);
            MyForm.Show();
        }

        private void записейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrushForm MyForm = new CrushForm(login, password);
            MyForm.Show();
        }

        //Выбор таблицы
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            page = 1;
            if (comboBox1.Text == "recipe")
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;                
            }
            if (comboBox1.Text == "doctor")
            {
                panel1.Visible = false;
                panel2.Visible = true;
                panel3.Visible = false;
                panel4.Visible = false;                
            }
            if (comboBox1.Text == "pacient")
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = true;
                panel4.Visible = false;                
            }
            if (comboBox1.Text == "drugs")
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = true;               
            }
            showTable();
            rowCount();
        }

        private void dataGridView1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode != Keys.Delete || login == "Guest" || login == "Doctor" || dataGridView1.Columns[0].Visible == true) return;
            int row  = dataGridView1.CurrentCell.RowIndex;
            string t = dataGridView1[0, row].Value.ToString();
            DialogResult result = new DialogResult();
            result = MessageBox.Show(dataGridView1[1, row].Value.ToString() + " - " + dataGridView1[2, row].Value.ToString() + "\nУдалить запись?", "Подтвердите действие", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                
               NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
               NpgsqlCommand command = new NpgsqlCommand("UPDATE "+comboBox1.Text+" SET is_del = true WHERE id = "+t+";", conn);
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
            showTable();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null || login == "Guest" || dataGridView1.Columns[0].Visible == true) return;
            int row = dataGridView1.CurrentCell.RowIndex;
            string t = dataGridView1[0, row].Value.ToString();
            AltForm MyForm = new AltForm(login, password, comboBox1.Text, t);
            MyForm.Show();
        }
        //Обновить
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")                
            page = 1;
            limit = "LIMIT " + numericUpDown1.Value.ToString() + " OFFSET 0";
            labelPages.Text = Convert.ToString(page) + "/" + Convert.ToString(PagesCount);
            showTable();
            rowCount();
        }

        private void prevStartButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
                rowCount();
            page = 1;
            limit = "LIMIT " + numericUpDown1.Value.ToString() + " OFFSET 0";
            labelPages.Text = Convert.ToString(page) + "/" + Convert.ToString(PagesCount);
            showTable();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
                rowCount();            
            if (page < PagesCount)
            {
                page++;
                labelPages.Text = Convert.ToString(page) + "/" + Convert.ToString(PagesCount);
                int h = Convert.ToInt32(numericUpDown1.Value) * (page-1);
                limit = "LIMIT " + numericUpDown1.Value.ToString() + " OFFSET " + Convert.ToString(h);
                showTable();
            }
        }

        private void nextEndButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
                rowCount();
            page = PagesCount;
            int h = Convert.ToInt32(numericUpDown1.Value) * (page - 1);
            limit = "LIMIT " + numericUpDown1.Value.ToString() + " OFFSET " + Convert.ToString(h);
            labelPages.Text = Convert.ToString(page) + "/" + Convert.ToString(PagesCount);
            showTable();
        }

        private void prevButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
                rowCount();
            if (page > 1)
            {
                page--;
                labelPages.Text = Convert.ToString(page) + "/" + Convert.ToString(PagesCount);
                int h = Convert.ToInt32(numericUpDown1.Value) * (page - 1);
                limit = "LIMIT " + numericUpDown1.Value.ToString() + " OFFSET " + Convert.ToString(h);
                showTable();
            }
        }
    }
}
