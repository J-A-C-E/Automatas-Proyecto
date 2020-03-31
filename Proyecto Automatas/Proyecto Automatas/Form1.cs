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
        #region PARAMETROS CONEXION 
        static string servidor = "107.180.51.242";
        static string puerto = "3306";
        static string usuario = "Javier";
        static string contrasena = "17100199";
        /*static string cadenaConexion = "server=" + servidor + "; port=" + puerto + "; user id=" + usuario +
                 "; password=" + contrasena;*/

        static string cadenaConexion = "SERVER=" + servidor + "; PORT=" + puerto+";Database=bdJace" + ";UID=" + usuario +
                ";PASSWORD=" + contrasena + ";";

        MySqlConnection miconexion = new MySqlConnection(cadenaConexion);
        #endregion

        #region RELLENO QUE NO SIRVE
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


            //string Cadena;

            //txtFrase.Text = txtFrase.Text.Trim();
            //Cadena = txtFrase.Text;
            //MessageBox.Show(Cadena.Length.ToString());


            //try
            //{
            //    miconexion.Open();
            //    string sql = "select * from Matriz";
            //    MySqlDataReader reader = null;
            //    MySqlCommand cmd = new MySqlCommand(sql, miconexion);
            //    reader = cmd.ExecuteReader();

            //    using(MySqlDataReader rdr = cmd.ExecuteReader())
            //    {

            //        while (rdr.Read())
            //        {
            //            /* se supone que leera con estoxd */
            //        }
            //    }

            //    //while (reader.Read())
            //    //{
            //    //    datos += reader.GetString(0) + "\n";
            //    //}
            //}
            //catch (MySqlException ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}


            //MessageBox.Show(datos);




            //esto si sirve pa conectar la bd
            try
            {
                miconexion.Open();
                MessageBox.Show("conexión abierta");
                miconexion.Close();
                MessageBox.Show("conexión cerrada");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //aqui termina la conexion

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Cadena = txtFrase.Text;
            for (int i = 0; i < Cadena.Length; i++)
            {
                MessageBox.Show(Cadena[i].ToString());

            }
        }

        #endregion


        //AQUI SUCEDE LA MAGIA :)
        private void btnIniciar_Click(object sender, EventArgs e)
        {
            //SIRVEN PARA LA CONEXION
            MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);
            MySqlDataAdapter da;
            DataSet ds;
            string query;
            int renglon = 1 ;


            string Cadena = txtFrase.Text;
            string Mensaje;
            int apuntadorID = 0;
            bool Espacio = false;

            for (int i = 0; i < Cadena.Length; i++)
            {
                //Muestra el recorrido del textBox
                MessageBox.Show(Cadena[i].ToString());
                Mensaje = Cadena[i].ToString();
                txtSubcadena.Text = txtSubcadena.Text + Mensaje;
                
                if (Cadena[i] == ' ')
                {
                    try
                    {

                        txtSubcadena.Clear();

                        if (Cadena[i + 2] == '\n')
                        {
                            
                                Espacio = true;
                                Recorrido(ref apuntadorID, ref Mensaje, ref Espacio);
                                MessageBox.Show("Salto de línea");
                                txtArchivo.Text = txtArchivo.Text + "\n" + txtToken.Text + "\n";
                                apuntadorID = 0;
                                i = i + 2;
                                renglon++;
                                txtRenglon.Text = renglon.ToString();
                                txtToken.Clear();
                            
                            
                        }
                        else
                        {
                            //DETECTA SI HAY UN ESPACIO
                            Espacio = true;
                            //apuntadorID++;
                            Recorrido(ref apuntadorID, ref Mensaje, ref Espacio);
                        }
                    }
                    catch (Exception E)
                    {
                        Espacio = true;
                        //apuntadorID++;
                        Recorrido(ref apuntadorID, ref Mensaje, ref Espacio);
                        txtArchivo.Text = txtArchivo.Text + "\n" + txtToken.Text + "\n";
                        txtToken.Clear();
                        MessageBox.Show("Fin de instrucción");
                    }
                   

                }
                else
                {
                    Recorrido(ref apuntadorID, ref Mensaje, ref Espacio);
                    /*query = "SELECT `Z" + Cadena[i] + "` FROM Matriz where ID=" + apuntadorID.ToString() + " ;";
                    da = new MySqlDataAdapter(query, conexionBD);
                    ds = new DataSet();
                    da.Fill(ds);
                    conexionBD.Close();

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        dataGridView1.DataSource = ds.Tables[0];

                        //OBTIENE EL VALOR DEL APUNTADOR
                        apuntadorID = int.Parse(dataGridView1.Rows[0].Cells[0].Value.ToString());
                        MessageBox.Show("EL apuntador es: " + apuntadorID);
                    }*/
                }

            }
        }

        public void Recorrido(ref int apuntadorID, ref string Cadena,ref bool Espacio)
        {
            MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);
            MySqlDataAdapter da;
            DataSet ds;
            string query;
            bool Entra = false;
            string Token = "";

            if(Espacio)
            {

                Entra = true;
                query = "SELECT DEL FROM Matriz where ID=" + apuntadorID.ToString() + " ;";
                da = new MySqlDataAdapter(query, conexionBD);
                ds = new DataSet();
                da.Fill(ds);
                conexionBD.Close();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];

                    //OBTIENE EL VALOR DEL APUNTADOR
                    apuntadorID = int.Parse(dataGridView1.Rows[0].Cells[0].Value.ToString());
                    MessageBox.Show("EL apuntador es: " + apuntadorID);
                }

            }
            else if (Cadena == " ")
            {
               
            }
            else
            {
                query = "SELECT `Z" + Cadena + "` FROM Matriz where ID=" + apuntadorID.ToString() + " ;";
                da = new MySqlDataAdapter(query, conexionBD);
                ds = new DataSet();
                da.Fill(ds);
                conexionBD.Close();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];

                    //OBTIENE EL VALOR DEL APUNTADOR
                    apuntadorID = int.Parse(dataGridView1.Rows[0].Cells[0].Value.ToString());
                    MessageBox.Show("EL apuntador es: " + apuntadorID);
                }
            }

           
            if(Entra)
            {
                query = "SELECT TOKEN" + Cadena + " FROM Matriz where ID=" + apuntadorID.ToString() + " ;";
                da = new MySqlDataAdapter(query, conexionBD);
                ds = new DataSet();
                da.Fill(ds);
                conexionBD.Close();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];

                    //OBTIENE EL VALOR DEL APUNTADOR
                    Token = (dataGridView1.Rows[0].Cells[0].Value.ToString());
                    txtToken.Text = txtToken.Text + Token + " ";
                    apuntadorID = 0;
                    MessageBox.Show("EL apuntador es: " + apuntadorID);
                    
                }
                /*da = new MySqlDataAdapter(query, conexionBD);
                ds = new DataSet();
                da.Fill(ds);
                conexionBD.Close();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];

                    //OBTIENE EL VALOR DEL APUNTADOR
                    Token = dataGridView1.Rows[0].Cells[0].Value.ToString();
                    apuntadorID = 0;
                    MessageBox.Show("EL apuntador es: " + apuntadorID+ "\n Y el token es:"+Token);
                }
                */
                Entra = false;
                Espacio = false;
            }
        }
    }
}
