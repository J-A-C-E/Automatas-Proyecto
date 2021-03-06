﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;



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

        static string cadenaConexion = "SERVER=" + servidor + "; PORT=" + puerto + ";Database=bdJace" + ";UID=" + usuario +
                ";PASSWORD=" + contrasena + ";";

        MySqlConnection miconexion = new MySqlConnection(cadenaConexion);
        #endregion

        #region RELLENO QUE NO SIRVE

        string ubicacion = @"c:\Archivo\";
        string cadeNum = "";
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
            
            string[] filePaths = Directory.GetFiles(@"c:\Archivo\");

            int i;

                for (i = 0; i < filePaths.Length; i++)
                {
                   cmbArchivo.Items.Add (filePaths[i].ToString());
                }
            actualizar();

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
            int renglon = 1;


            string Cadena = txtFrase.Text;
            string Mensaje="";
            int apuntadorID = 0;
            bool Espacio = false;
            int Numeros = 0;
            


            for (int i = 0; i < Cadena.Length; i++)
            {

                if(Sostenido)
                {
                    Instruccion = Instruccion + Cadena[i].ToString();
                    //MessageBox.Show(Instruccion);
                }

                try
                {
                    Numeros = int.Parse(Cadena[i].ToString());
                    cadeNum = cadeNum + Numeros.ToString();
                    EsNumero = true;
                    Recorrido(ref apuntadorID, ref Mensaje, ref Espacio);

                }
                catch(Exception x)
                {
                    EsNumero = false;
                }
                //Muestra el recorrido del textBox
                MessageBox.Show(Cadena[i].ToString());
                Mensaje = Cadena[i].ToString();
                txtSubcadena.Text = txtSubcadena.Text + Mensaje;

                if (Cadena[i] == '#')
                {
                    Instruccion = Instruccion + Cadena[i].ToString();
                    Recorrido(ref apuntadorID, ref Mensaje, ref Espacio);
                    Sostenido = true;
                }


                else if (Cadena[i] == ' ')
                {
                    try
                    {

                        txtSubcadena.Clear();

                        if (Cadena[i + 2] == '\n')
                        {

                            Espacio = true;
                            Recorrido(ref apuntadorID, ref Mensaje, ref Espacio);
                            MessageBox.Show("Salto de línea");
                            txtArchivo.Text = txtArchivo.Text + txtToken.Text + "\r\n";
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
                        txtArchivo.Text = txtArchivo.Text + txtToken.Text + "\r\n";
                        txtToken.Clear();
                        MessageBox.Show("Fin de instrucción");
                        txtRenglon.Text = "1";
                    }


                }

                else
                {
                    
                    Recorrido(ref apuntadorID, ref Mensaje, ref Espacio);
                    
                }

            }
            actualizar();

        }

        bool Sostenido = false;
        bool EsNumero = false;

        public void Recorrido(ref int apuntadorID, ref string Cadena, ref bool Espacio)
        {
            MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);
            MySqlDataAdapter da;
            DataSet ds;
            string query;
            bool Entra = false;
            string Token = "";

            if (Espacio)
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
                if(Sostenido)
                {
                    Cadena = "LET";
                }

                if (EsNumero)
                {
                    Cadena = "1";
                }

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


            if (Entra)
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

                    if(Token=="IDE")
                    {
                        //METODO
                        BuscarTokenIde();
                    }

                    else if (Token == "CONE")
                    {
                        //METODO
                        BuscarTokenCONE();
                    }
                    else
                    {
                        txtToken.Text = txtToken.Text + Token + " ";
                    }

                    apuntadorID = 0;
                    MessageBox.Show("EL apuntador es: " + apuntadorID);

                }
           
                Entra = false;
                Espacio = false;
                Sostenido = false;
                EsNumero = false;
            }
        }

        string Instruccion = "";
        public void BuscarTokenIde()
        {
            MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);
            MySqlDataAdapter da;
            DataSet ds;
            string query;
            string query2;
            int queryid;
            string Token = "";

                query = "select TOKEN from IDENTIFICADOR where NOMBRE like '%" + Instruccion +"%'";
                da = new MySqlDataAdapter(query, conexionBD);
                ds = new DataSet();
                da.Fill(ds);
                conexionBD.Close();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    Token = dataGridView1.Rows[0].Cells[0].Value.ToString();
                    txtToken.Text = txtToken.Text + Token + " ";

                }

                else
                {
                    //aqui tomamos el ultimo id
                    query2 = "select MAX(ID) from IDENTIFICADOR";
                    da = new MySqlDataAdapter(query2, conexionBD);
                    ds = new DataSet();
                    da.Fill(ds);
                    conexionBD.Close();

                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        dataGridView1.DataSource = ds.Tables[0];
                        query2 = dataGridView1.Rows[0].Cells[0].Value.ToString();
                        queryid = Convert.ToInt32(query2) + 1;
                        query2 = Convert.ToString(queryid);
                        

                    }
                        query = "insert into IDENTIFICADOR (ID, NOMBRE, TOKEN) values (" + query2 +", '" + Instruccion + "', 'IDE" + query2 + "')";
                        da = new MySqlDataAdapter(query, conexionBD);
                        ds = new DataSet();
                        da.Fill(ds);
                        conexionBD.Close();

                        query = "select TOKEN from IDENTIFICADOR where NOMBRE like '%" + Instruccion + "%'";
                        da = new MySqlDataAdapter(query, conexionBD);
                        ds = new DataSet();
                        da.Fill(ds);
                        conexionBD.Close();

                        if (ds.Tables[0].Rows.Count != 0)
                        {
                            dataGridView1.DataSource = ds.Tables[0];
                            Token = dataGridView1.Rows[0].Cells[0].Value.ToString();
                            txtToken.Text = txtToken.Text + Token + " ";

                        }
                    
               
                }

            Instruccion = "";


        }

        public void BuscarTokenCONE()
        {
            MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);
            MySqlDataAdapter da;
            DataSet ds;
            string query;
            string query2;
            int queryid;
            string Token = "";

            query = "select TOKEN from CONSTANTE where NOMBRE like '%" + cadeNum + "%'";
            da = new MySqlDataAdapter(query, conexionBD);
            ds = new DataSet();
            da.Fill(ds);
            conexionBD.Close();

            if (ds.Tables[0].Rows.Count != 0)
            {
                dataGridView1.DataSource = ds.Tables[0];
                Token = dataGridView1.Rows[0].Cells[0].Value.ToString();
                txtToken.Text = txtToken.Text + Token + " ";

            }

            else
            {
                //aqui tomamos el ultimo id
                query2 = "select MAX(ID) from CONSTANTE";
                da = new MySqlDataAdapter(query2, conexionBD);
                ds = new DataSet();
                da.Fill(ds);
                conexionBD.Close();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    query2 = dataGridView1.Rows[0].Cells[0].Value.ToString();
                    queryid = Convert.ToInt32(query2) + 1;
                    query2 = Convert.ToString(queryid);


                }
                query = "insert into CONSTANTE (ID, NOMBRE, TOKEN) values (" + query2 + ", '" + cadeNum + "', 'CONE" + query2 + "')";
                da = new MySqlDataAdapter(query, conexionBD);
                ds = new DataSet();
                da.Fill(ds);
                conexionBD.Close();

                query = "select TOKEN from CONSTANTE where NOMBRE like '%" + cadeNum + "%'";
                da = new MySqlDataAdapter(query, conexionBD);
                ds = new DataSet();
                da.Fill(ds);
                conexionBD.Close();

                if (ds.Tables[0].Rows.Count != 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    Token = dataGridView1.Rows[0].Cells[0].Value.ToString();
                    txtToken.Text = txtToken.Text + Token + " ";

                }


            }

            cadeNum = "";


        }

        private void btnArchivo_Click(object sender, EventArgs e)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@""+cmbArchivo.Text);
                txtFrase.Text = text;
            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message);
               
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtArchivo.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            actualizar();
        }
        public void actualizar()
        {
            MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);
            MySqlDataAdapter da;
            DataSet ds;
            string conecta;

            conecta = "select * from CONSTANTE";
            da = new MySqlDataAdapter(conecta, conexionBD);
            ds = new DataSet();
            da.Fill(ds);
            conexionBD.Close();

            if (ds.Tables[0].Rows.Count != 0)
            {
                dgvConstantes.DataSource = ds.Tables[0];
            }

            conecta = "select * from IDENTIFICADOR";
            da = new MySqlDataAdapter(conecta, conexionBD);
            ds = new DataSet();
            da.Fill(ds);
            conexionBD.Close();

            if (ds.Tables[0].Rows.Count != 0)
            {
                dgvIdentificador.DataSource = ds.Tables[0];
            }
        }

    }
}

