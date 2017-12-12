using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controlador
{
    public partial class Principal : Form
    {
        Graficas graficas = new Graficas();
        private Conexion conexion;
        private Conexion_lectura conexion_lectura;
        private Queue<String> comandos;
        private List<nodo> lista_nodos = new List<nodo>();
        private List<vector> vectores = new List<vector>();
        private List<vector> depurado = new List<vector>();
        /// <summary>
        /// Distancia del modulo de ranuradoa las espatulas en el modulo de doblado.
        /// en milimetros (mm)
        /// </summary>
        private const float distancia_espatula=750; 
        /// <summary>
        /// Distancia minima en la que se agrupan los vetores de una circunferencia
        /// </summary>
        private int resolucion = 20;
        private int distancia_mv = 5;
        private const double min_v = 1;
        /// <summary>
        /// Variables para compensar la resistencia de la lámina
        /// </summary>
        private int compensadorA = 25;
        private int compensadorB = 25;
        Graphics dibujo;        
        Pen plumaBlanca = new Pen(Color.White);
        Pen plumaAzul = new Pen(Color.Blue);
        Pen plumaRoja = new Pen(Color.Red);
        /// <summary>
        /// Esta estructura almacena el punto inicial y final de dos vectores
        /// y la distancia entre ellos
        /// </summary>
        public struct vector
        {            
            public nodo inicial; 
            public nodo final;   
            /// <summary>
            /// Distancia entre dos puntos. Alacena la suma de las distancias de los vectores contenidos en ella.
            /// </summary>
            public double distancia;
            /// <summary>
            /// 
            /// </summary>
            public double puntoenlarecta;
            public bool circunferencia;
        }
        public struct nodo
        {
            public double x;
            public double y;
        }
        public Principal()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Esta funcion se encarga de volver a llamar a la funcion enviar cuando existe respuesta de la tablilla de control
        /// </summary>
        /// <param name="mensajeSerial"></param>
        public void nvo_dato(string mensajeSerial)
        {            
            if (mensajeSerial.ToString().IndexOf("Listo", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                enviar();
            }
            
        }
        /// <summary>
        /// Funcion que se carga al iniciar la ventana
        /// En ella se activan los botones, se crea la instancia para crear la conexion a la  tablilla de control
        /// se Buscan los puertos COM disponibles y sem muestran las graficas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {            
            btn_dpp.Enabled = false;
            button1.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;
            conexion = Conexion.Instancia;            
            conexion.Principal = this;
            conexion_lectura = Conexion_lectura.Instancia;
            conexion_lectura.Graficas = graficas;
            string[] ports = SerialPort.GetPortNames();            
            graficas.Show();  
            graficas.Focus();                   
            foreach (String st in ports)
            {
                cb_menu.Items.Add(st);
                cb_menu_lectura.Items.Add(st);
            }
        }
        /// <summary>
        /// boton que inicia el trabajo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (lb_segmentos.Items.Count != 0)
            {
                lb_proceso.Items.Clear();
                comandos = new Queue<string>();
                foreach (object x in lb_segmentos.Items)
                {
                    comandos.Enqueue(x.ToString());
                }
                button1.Enabled = false;
                txt_info.Enabled = false;

                enviar();

                button1.Enabled = true;
                txt_info.Enabled = true;
            }
            else
            {
                MessageBox.Show("No hay nada que procesar");
            }
        }
        /// <summary>
        /// Funcion que se encarga de eviar los comandos disponibles en la lista de comandos
        /// </summary>
        public void enviar()
        {
            string comando;
            if (comandos.Count() > 0)
            {
                comando = comandos.Dequeue();
                lb_proceso.Items.Add(comando);
                conexion.enviarComando(comando);
            }
            else
            {
                MessageBox.Show("Procesamiento terminado");
            }
                  
        }
        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void btn_conectar_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.crearConexion(cb_menu.SelectedItem.ToString());
                button1.Enabled = true;
                btn_dpp.Enabled = true;
            }
            catch(Exception) {
                MessageBox.Show("No se ha seleccionado el puerto adecuado");
            }
        }
        /// <summary>
        /// Funcion que convierte G-code a los coomandos de la maquina
        /// 1.- Convierte los puntos en el plano de corte en vectores
        /// 2.- Busca circunferencias y las agrupa en estructuras mayores, con punto inicial y fina. Ademas agrega la distancia entre ambos
        /// 3.- Busca los puntos en que se hara una operacion
        /// 4.- Agrega la distancia a la espatula
        /// 5.- Reubica operaciones
        /// 6.- Enlista las operaciones necesarias para el trabajo. 
        /// </summary>
        private void traducir()
        {
            lb_angulos.Items.Clear();
            lb_diferencia.Items.Clear();
            lb_gcode.Items.Clear();
            lb_proceso.Items.Clear();
            lb_segmentos.Items.Clear();
            lb_transformado.Items.Clear();
            lista_nodos = new List<nodo>();
            vectores = new List<vector>();

            openFileDialog1.Filter = "Archivo GCode |*.tap;*.txt";
            openFileDialog1.ShowDialog();
            String dir = openFileDialog1.FileName;
            if (dir != "openFileDialog1")
            {
                String linea = "";
                txt_dirgcode.Text = dir;
                StreamReader archivoGcode = new StreamReader(dir, false);
                Double x1 = 0;
                Double y1 = 0;
                String key;
                String patron;
                int busqueda;
                Boolean planoCorte = false;
                nodo inicial, anterior, actual, preanterior;
                inicial = new nodo();
                actual = new nodo();
                preanterior = new nodo();
                anterior = new nodo();
                double distancia;
                double ang_vanterior = 0;
                double ang_vactual = 0;
                double angulo;
                vector vanterior = new vector();
                vector vactual = new vector();
                bool nvaPosicion = false;
                while ((linea = archivoGcode.ReadLine()) != null)
                {
                    if (linea != "")
                    {
                       // x1 = 0;
                       // y1 = 0;
                        lb_gcode.Items.Add(linea);
                        patron = "G0Z";//FIN CORTE o CONFIGURACION CORTE
                        busqueda = linea.IndexOf(patron);
                        if (busqueda == 0)
                        {
                            planoCorte = false;
                        }
                        Match bx = Regex.Match(linea, @"X([\-]?[0-9]+.[0-9]+)\d",
                                              RegexOptions.IgnoreCase);
                        Match by = Regex.Match(linea, @"Y([\-]?[0-9]+.[0-9]+)\d",
                                                                  RegexOptions.IgnoreCase);
                        if (bx.Success)
                        {
                            nvaPosicion = true;
                            key = bx.Groups[1].Value;
                            if (key != null)
                            {
                                x1 = Math.Round(Convert.ToDouble(key), 3);
                            }
                        }
                        if (by.Success)
                        {
                            nvaPosicion = true;
                            key = by.Groups[1].Value;
                            if (key != null)
                            {
                                y1 = Math.Round(Convert.ToDouble(key), 3);
                            }
                        }
                        if (planoCorte && nvaPosicion)
                        {
                            nvaPosicion = false;
                            nodo n = new nodo();
                            n.x = x1;
                            n.y = y1;
                            lista_nodos.Add(n);
                            anterior = n;
                        }
                        patron = "G1Z";//INICIO CORTE
                        busqueda = linea.IndexOf(patron);
                        if (busqueda == 0)
                        {
                            planoCorte = true;
                            inicial.x = x1;
                            inicial.y = y1;
                            lista_nodos.Add(inicial);
                          //  anterior = inicial;
                        }                        
                    }
                }
                vector segmento = new vector();
                vector segmento_ant = new vector();
                Boolean ini = true;
                Boolean ini2 = true;
                distancia = 0;
                double distancia2 = 0;
                bool circ=false;
                //double[] distann = new double[1550];
                //int cc=0;
                foreach (nodo n in lista_nodos)
                {
                    if (ini && ini2)
                    {
                        segmento.inicial = n;
                        ini = false;
                        preanterior = n;
                    }
                    else if (!ini && ini2)
                    {
                        anterior = n;
                        ini2 = false;
                    }
                    else
                    {
                        actual = n;
                        distancia += Math.Sqrt(Math.Pow((anterior.x - preanterior.x), 2) + Math.Pow((anterior.y - preanterior.y), 2));
                        distancia2 = Math.Sqrt(Math.Pow((actual.x - anterior.x), 2) + Math.Pow((actual.y - anterior.y), 2));

                        if (distancia < 5 && !circ)
                        {
                            circ = true;
                        }
                        
                        ///Existe una limitante                           
                        ///Todos los nodos pequeños los toma como circunferencias. 
                        if (distancia2 > distancia_mv || distancia >= resolucion)
                        {
                            segmento.final = anterior;
                            segmento.circunferencia = circ;
                            segmento.distancia = distancia;
                            distancia = 0;
                            vectores.Add(segmento);
                            segmento_ant = segmento;
                            segmento = new vector();
                            segmento.inicial = anterior;
                            circ = false;
                        }
                        preanterior = anterior;
                        anterior = actual;
                    }
                }
                if (distancia != 0)
                {
                    distancia += Math.Sqrt(Math.Pow((anterior.x - preanterior.x), 2) + Math.Pow((anterior.y - preanterior.y), 2));
                    segmento.final = anterior;
                    segmento.distancia = distancia;
                    segmento.circunferencia = circ;
                    vectores.Add(segmento);
                }
                else
                {
                    segmento.final = anterior;
                    segmento.distancia = Math.Sqrt(Math.Pow((anterior.x - preanterior.x), 2) + Math.Pow((anterior.y - preanterior.y), 2));
                    segmento.circunferencia = circ;
                    vectores.Add(segmento);
                }
                ini = true;
                distancia = 0;
                dibujo = pictureBox1.CreateGraphics();
                dibujo.Clear(Color.Black);
                Boolean cambio = true;
                int nn = 0;
                Font drawFont = new Font("Arial", 5);
                SolidBrush drawBrush = new SolidBrush(Color.Red);
                foreach (vector ve in vectores)
                {
                    dibujo.DrawString(nn.ToString(), drawFont, drawBrush, Convert.ToSingle(ve.final.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.final.y) - 40);
                    nn++;
                    /*if (cambio)
                    {
                        dibujo.DrawLine(plumaBlanca, Convert.ToSingle(ve.inicial.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.inicial.y) - 40,
                            Convert.ToSingle(ve.final.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.final.y) - 40);
                        cambio = false;
                    }
                    else
                    {
                        dibujo.DrawLine(plumaAzul, Convert.ToSingle(ve.inicial.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.inicial.y) - 40,
                            Convert.ToSingle(ve.final.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.final.y) - 40);
                        cambio = true;
                    }*/
                    if (ve.circunferencia)
                    {
                        dibujo.DrawLine(plumaRoja, Convert.ToSingle(ve.inicial.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.inicial.y) - 40,
                            Convert.ToSingle(ve.final.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.final.y) - 40);
                        cambio = false;
                    }
                    else
                    {
                        dibujo.DrawLine(plumaAzul, Convert.ToSingle(ve.inicial.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.inicial.y) - 40,
                            Convert.ToSingle(ve.final.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.final.y) - 40);
                        cambio = false;
                    }
                    distancia += ve.distancia;
                    if (ini)
                    {
                        vanterior = ve;
                    //    lb_diferencia.Items.Add(ve.distancia);
                    //    lb_transformado.Items.Add(String.Format("{{({0},{1}),({2},{3})}}", ve.inicial.x, ve.inicial.y, ve.final.x, ve.final.y));
                        ini = false;
                    }
                    else
                    {
                        vactual = ve;
                        /*ang_vanterior = Math.Round(Math.Atan2(
                            (vanterior.final.x - vanterior.inicial.x),
                            (vanterior.final.y - vanterior.inicial.y)
                            )*(180/Math.PI),0);*/
                        ang_vanterior = Math.Atan2(
                            (vanterior.final.y - vanterior.inicial.y),
                            (vanterior.final.x - vanterior.inicial.x)
                            );
                        ang_vanterior = checarcuadrante(ang_vanterior);
                        /*  if (ang_vanterior > 0 && ang_vanterior < (Math.PI/2))
                          {
                              ang_vanterior = ang_vanterior -360;
                          }                        */
                        //ang_vanterior= (ang_vanterior > 0 ? ang_vanterior : (2 * Math.PI + ang_vanterior)) * 360 / (2 * Math.PI);
                        //if(ang_vanterior>180)
                        //ang_vanterior=
                        /* if (ang_vanterior < 0)
                             ang_vanterior = 180 + (ang_vanterior+180);*/
                        /* ang_vactual = Math.Round(Math.Atan2(
                             (vactual.final.x - vactual.inicial.x),
                             (vactual.final.y - vactual.inicial.y)
                             )*(180/Math.PI),0);*/
                        ang_vactual = Math.Atan2(
                        (vactual.final.y - vactual.inicial.y),
                        (vactual.final.x - vactual.inicial.x)
                        );
                        ang_vactual = checarcuadrante(ang_vactual);
                        angulo = Math.Round((ang_vactual - ang_vanterior), 0);

                        if (angulo > 180)
                        {
                            angulo = angulo - 360;
                        }
                        if (angulo < -180)
                        {
                            angulo = angulo + 360;
                        }

                        lb_angulos.Items.Add(String.Format("{0}", angulo));
                        // lb_angulos.Items.Add(String.Format("[{0},{1}]:[{2}]::{3}", ang_vanterior, ang_vactual, angulo, nn - 2));
                        //  lb_diferencia.Items.Add(vactual.distancia);
                        //  lb_transformado.Items.Add(String.Format("{{({0},{1}),({2},{3})}}", vactual.inicial.x, vactual.inicial.y, vactual.final.x, vactual.final.y));
                        ///////////////////////////////////////////////////////
                        ////////////INICIA GEERACION DE CODIGO///////////////
                        ///////////////////////////////////////////////////
                        /*
                         No se pueden trabajar con distancias superiores al metro
                         9999 milimetros para ser exactos
                         */
                        if (angulo > 340 || angulo < -340)
                        {
                            angulo = 0;
                        }
                        lb_segmentos.Items.Add(String.Format("C{0}", Math.Round(vanterior.distancia, 2)));
                        if (angulo > 0)
                        {
                            angulo = angulo + 90 + compensadorA;
                            if (angulo > 165)
                                angulo = 165;
                            lb_segmentos.Items.Add(String.Format("B000", Math.Abs(angulo)));
                            lb_segmentos.Items.Add(String.Format("A{0}", Math.Abs(angulo)));
                            lb_segmentos.Items.Add(String.Format("A085", Math.Abs(angulo)));
                        }
                        if (angulo < 0)
                        {
                            angulo = angulo - 90 - compensadorB;
                            if (angulo < -165)
                                angulo = -165;
                            lb_segmentos.Items.Add(String.Format("A000", Math.Abs(angulo)));
                            lb_segmentos.Items.Add(String.Format("B{0}", Math.Abs(angulo)));
                            lb_segmentos.Items.Add(String.Format("B085", Math.Abs(angulo)));
                        }
                        ///////////////////////////////////////////////////////
                        ///////////TERMINA GENERACION DE CODIGO//////////////
                        ///////////////////////////////////////////////////
                        vanterior = vactual;
                    }
                }
                lb_segmentos.Items.Add(String.Format("C{0}", Math.Round(vanterior.distancia, 2)));
                txt_dist.Text = distancia.ToString();
                foreach (nodo no in lista_nodos)
                {
                    lb_transformado.Items.Add(String.Format("{0}:{1}",no.x,no.y));
                }
                foreach (vector ve in vectores)
                {
                    lb_diferencia.Items.Add(String.Format("{0}-{1},{2}-{3}:{4}:{5}",ve.inicial.x,ve.final.x,ve.inicial.y,ve.final.y,ve.distancia,ve.circunferencia));
                }
                lbl_cant_angulos.Text = lb_angulos.Items.Count.ToString();
                lbl_cant_vectores.Text = vectores.Count.ToString();
            }

        }
        /// <summary>
        /// Boton de carga de archivos en formato G-code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cargar_Click(object sender, EventArgs e)
        {
            traducir();
        }       
        /// <summary>
        /// Funcion que rectifica los angulos.
        /// </summary>
        /// <param name="angulo"></param>
        /// <returns></returns>
                        
        private double checarcuadrante(double angulo)
        {
            //cuandrante 1
            if (angulo > 0 && angulo < (Math.PI / 2))
            {
                angulo = 90 - angulo * 180 / Math.PI;
            }
            //cuadrante 2
            else if (angulo > (Math.PI / 2) && angulo < Math.PI)
            {
                angulo = -((angulo * 180 / Math.PI) - 90);
            }
            //cuadrante 3
            else if (angulo < -(Math.PI / 2) && angulo > -Math.PI)
            {
                angulo = -((angulo * 180 / Math.PI) + 90) - 180;
            }
            //cuadrante 4
            else if (angulo < 0 && angulo > -(Math.PI / 2))
            {
                angulo = 90 - (angulo * 180 / Math.PI);
            }
            //angulo vertical
            else if (angulo == (Math.PI / 2))
            {
                angulo = 0;
            }
            else if (angulo == -(Math.PI / 2))
            {
                angulo = 180;
            }
            //inicio
            else if (angulo == 0)
            {
                angulo = 90;
            }
            //angulo horizontal PI 180 grados
            else if (angulo == Math.PI || angulo == -Math.PI)
            {
                angulo = -90;
            }
            return angulo;
        }
        /// <summary>
        /// Boton para enviar comando individual
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_dpp_Click(object sender, EventArgs e)
        {
            String resp = tx_dpp.Text;

        //    resp = conexion.enviarComando(resp);
            
            lb_proceso.Items.Clear();
            comandos = new Queue<string>();
            
            comandos.Enqueue(String.Format("{0}",resp));
            
            button1.Enabled = false;
            txt_info.Enabled = false;
            btn_dpp.Enabled = false;
            tx_dpp.Enabled = false;

            enviar();

            button1.Enabled = true;
            txt_info.Enabled = true;
            btn_dpp.Enabled = true;
            tx_dpp.Enabled = true;
        }

        private void cb_menu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// boton para recargar los puertos COM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            cb_menu.Items.Clear();
            cb_menu_lectura.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (String st in ports)
            {
                cb_menu.Items.Add(st);
                cb_menu_lectura.Items.Add(st);
            }
        }
        /// <summary>
        /// Boton para establecer conexion con la tablilla de información
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_conectar_lectura_Click(object sender, EventArgs e)
        {
            try
            {
                conexion_lectura.crearConexion(cb_menu_lectura.SelectedItem.ToString());
                //button1.Enabled = true;
                //btn_dpp.Enabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("No se ha seleccionado el puerto adecuado");
            }
        }
        /// <summary>
        /// Boton que incia la captura de informacion en la tablilla de informacion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_iniciar_captura_Click(object sender, EventArgs e)
        {
            graficas.Medicion(true);
        }
        /// <summary>
        /// Boton que detiene la lectura de la tablilla de información
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bnt_detener_lectura_Click(object sender, EventArgs e)
        {
            graficas.Medicion(false);
        }

        private void lb_transformado_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }
        
    }    
}
