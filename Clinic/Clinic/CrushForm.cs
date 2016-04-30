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
    public partial class CrushForm : Form
    {
        public CrushForm(string log, string pass)
        {
            InitializeComponent();
            login = log;
            password = pass;
        }
        string login = "";
        string password = "";

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "";
            Random rnd = new Random();
            for (int i = 0; i< numericUpDown1.Value; i++)
            {
                sql = "INSERT INTO public.recipe(id_doctor, id_pacient, id_drugs, dosage, is_del) VALUES(\'" + rnd.Next(1, 1) + "\', \'" + rnd.Next(1, 1) + "\', \'" + rnd.Next(1, 1) + "\', \'" + rnd.Next(1, 100) + " раз в день\', false); ";
                query(sql);
            }
            MessageBox.Show("Готово!");
        }

        private void query(string sql)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;User Id=" + login + ";Password=" + password + ";Database=clinic;");
            NpgsqlCommand command = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = null;
            try
            {
                conn.Open();
                reader = command.ExecuteReader();
                reader.Close();
            }
            catch (Exception m)
            {
                //MessageBox.Show(m.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "DELETE FROM public.recipe WHERE id > " + numericUpDown2.Value + ";";
            query(sql);
            MessageBox.Show("Готово!");
        }

        
    }
}
