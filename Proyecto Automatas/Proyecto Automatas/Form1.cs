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

        public void Lenght()
        {
            string Cadena = txtFrase.Text;
            MessageBox.Show(Cadena.Length.ToString());
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            string Cadena;

            txtFrase.Text = txtFrase.Text.Trim();
            Cadena = txtFrase.Text;
            //MessageBox.Show(Cadena.Length.ToString());

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

        private void button1_Click(object sender, EventArgs e)
        {
            //NO SIRVE EL TRIM :(
            string Cadena = txtFrase.Text.Trim();

            for(int i=0; i<Cadena.Length; i++)
            {
                //Muestra el recorrido del textBox
                MessageBox.Show(Cadena[i].ToString());
                if (Cadena[i] == ' ')
                {
                    MessageBox.Show("Aqui hay un espacio");
                }
            }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            string Cadena = txtFrase.Text.Trim();

            //CICLO DE RECORRIDO
            for (int i = 0; i < Cadena.Length; i++)
            {
                if (Cadena[i] == ' ')
                {
                    MessageBox.Show("ESPACIO");
                    txtArchivo.Text = txtArchivo.Text + Cadena[i].ToString();

                    txtSubcadena.Text = "";
                }
                else
                {
                    MessageBox.Show(Cadena[i].ToString());

                    //MUESTRA el el recorrido en textBox
                    txtSubcadena.Text = txtSubcadena.Text + Cadena[i].ToString();
                }

                /*
                //Muestra el recorrido del textBox
                MessageBox.Show(Cadena[i].ToString());

                txtSubcadena.Text = txtSubcadena.Text + Cadena[i].ToString();
                if (Cadena[i] == ' ')
                {
                    MessageBox.Show("Aqui hay un espacio");
                }
                */
            }
        }

       
    }
}
