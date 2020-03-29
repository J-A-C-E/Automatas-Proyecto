using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 


namespace Proyecto_Automatas
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            string servidor = "107.180.51.242";
            string puerto = "3306";
            string usuario = "Javier";
            string contrasena = "17100199";
            string datos = "";

            /*string cadenaConexion = "server=" + servidor + "; port=" + puerto + "; user id=" + usuario + 
                "; password=" + contrasena + "; database=dbJace;";
            */

            string cadenaConexion = "server=" + servidor + "; port=" + puerto + "; user id=" + usuario +
                "; password=" + contrasena;
            MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);

            try
            {
                conexionBD.Open();

                MySqlDataReader reader = null;
                MySqlCommand cmd = new MySqlCommand("SHOW DATABASES", conexionBD);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    datos += reader.GetString(0) + "\n";
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            MessageBox.Show(datos);


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
