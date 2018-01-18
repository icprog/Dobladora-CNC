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
        /// <summary>
        /// Variables para la conexion a la tablilla de control
        /// </summary>
        private Conexion conexion;
        private Conexion_lectura conexion_lectura;
        /// <summary>
        /// Listas de los comandos a ejecutar, de los nodos y de los vectores. 
        /// </summary>
        private Queue<String> comandos;
        private List<nodo> lista_nodos = new List<nodo>();
        private List<vector> vectores = new List<vector>();
        private List<puntos_criticos> p_criticos_D = new List<puntos_criticos>();
        private List<puntos_criticos> p_criticos_R = new List<puntos_criticos>();
        private List<puntos_criticos> p_criticos_Union = new List<puntos_criticos>();
        //private List<vector> depurado = new List<vector>();
        /// <summary>
        /// Distancia del modulo de ranuradoa las espatulas en el modulo de doblado.
        /// en milimetros (mm)
        /// </summary>
        private const float distancia_espatula=150;
        /// <summary>
        /// Aqui se agrega la distancia que se requiere para que el carrito del modulo de ranurado
        /// logre su recorrido (El modulo deberia modificarse para usar switch y el programa en el 
        /// microcontrolador para no requerir de la distancia)
        /// </summary>
        private const float distancia_ranurado = 300;
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
        /// <summary>
        /// Variables para la grafica de la figura
        /// </summary>
        Graficas graficas = new Graficas();
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
           // public double puntoenlarecta;
            public bool circunferencia;
        }
        /// <summary>
        /// Esta estructura almacena todos los nodos obtenidos del archivo en G-code
        /// </summary>
        public struct nodo
        {
            public double x;
            public double y;
        }
        /// <summary>
        /// Esta estructura almacena todos los puntos donde se realizara algun tipo de 
        /// accion:
        /// comando - acción
        /// D - Doblado
        /// R - Ranurado
        /// </summary>
        public struct puntos_criticos
        {
            public double punto;
            public char accion;
            public double angulo;
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
        /// 
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
        /// 1.- Convierte los puntos en el plano de corte en vectores (Actualmente solo soporta archivos con un solo corte)
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

                bool nvaPosicion = false;
                while ((linea = archivoGcode.ReadLine()) != null)
                {
                    if (linea != "")
                    {
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
                bool circ = false;
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
                //Boolean cambio = true;
                int nn = 0;
                Font drawFont = new Font("Arial", 5);
                SolidBrush drawBrush = new SolidBrush(Color.Red);
                ///////////////////////////////////////////////////////////////
                //////////////////////// Dibujar figura //////////////////////
                /////////////////////////////////////////////////////////////
                foreach (vector ve in vectores)
                {
                    dibujo.DrawString(nn.ToString(), drawFont, drawBrush, Convert.ToSingle(ve.final.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.final.y) - 40);
                    nn++;
                    if (ve.circunferencia)
                    {
                        dibujo.DrawLine(plumaRoja, Convert.ToSingle(ve.inicial.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.inicial.y) - 40,
                            Convert.ToSingle(ve.final.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.final.y) - 40);
                    }
                    else
                    {
                        dibujo.DrawLine(plumaAzul, Convert.ToSingle(ve.inicial.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.inicial.y) - 40,
                            Convert.ToSingle(ve.final.x) + 40, pictureBox1.Height - Convert.ToSingle(ve.final.y) - 40);
                    }
                    distancia += ve.distancia;
                }
                ///////////////////////////////////////////////////////////////
                /////////////// Obtener puntos criticos //////////////////////
                /////////////////////////////////////////////////////////////
                int cv = vectores.Count();
                double ang_vanterior = 0;
                double ang_vactual = 0;
                double angulo;
                vector vanterior = new vector();
                vector vactual = new vector();
                vector vsiguiente = new vector();
                double distancia_acumulada = 0;
                puntos_criticos pc_intercambio;
                for (int x = 1; x < cv; x++)
                {
                    vanterior = vectores.ElementAt(x - 1);
                    vactual = vectores.ElementAt(x);
                    if (x == 1)
                    {
                        pc_intercambio = new puntos_criticos();
                        pc_intercambio.punto = 0;
                        pc_intercambio.accion = 'R';
                        pc_intercambio.angulo = 0;
                        p_criticos_R.Add(pc_intercambio);
                        distancia_acumulada += vanterior.distancia;
                    }
                    if (x < (cv - 1))
                    {
                        vsiguiente = vectores.ElementAt(x + 1);
                    }
                    else
                    {
                        vector vn = new vector();
                        vn.inicial = vactual.final;
                        if (vactual.final.x > 0)
                            vn.final.x = vactual.final.x + 1;
                        else
                            vn.final.x = vactual.final.x - 1;
                        if (vactual.final.y > 0)
                            vn.final.y = vactual.final.y + 1;
                        else
                            vn.final.y = vactual.final.y - 1;
                        vn.distancia = 1.4142;
                        vn.circunferencia = vactual.circunferencia;
                        vsiguiente = vn;
                    }
                    ang_vanterior = Math.Atan2
                    (
                            (vanterior.final.y - vanterior.inicial.y),
                            (vanterior.final.x - vanterior.inicial.x)
                    );
                    ang_vanterior = checarcuadrante(ang_vanterior);
                    ang_vactual = Math.Atan2
                    (
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
                    if (vanterior.circunferencia && vactual.circunferencia && vsiguiente.circunferencia)
                    {
                        /*  utilizar distancia y otra variable para acumular la distancia*/
                        pc_intercambio = new puntos_criticos();
                        pc_intercambio.punto = distancia_acumulada + distancia_espatula;
                        pc_intercambio.accion = 'D';
                        pc_intercambio.angulo = angulo;
                        p_criticos_D.Add(pc_intercambio);
                    }
                    else if (!vanterior.circunferencia && vactual.circunferencia && vsiguiente.circunferencia)
                    {
                        pc_intercambio = new puntos_criticos();
                        pc_intercambio.punto = distancia_acumulada;
                        pc_intercambio.accion = 'R';
                        pc_intercambio.angulo = 0;
                        p_criticos_R.Add(pc_intercambio);
                        pc_intercambio = new puntos_criticos();
                        pc_intercambio.punto = distancia_acumulada + distancia_espatula;
                        pc_intercambio.accion = 'D';
                        pc_intercambio.angulo = angulo;
                        p_criticos_D.Add(pc_intercambio);
                    }
                    else if ((!vanterior.circunferencia && vactual.circunferencia && !vsiguiente.circunferencia) ||
                             (!vanterior.circunferencia && !vactual.circunferencia && !vsiguiente.circunferencia) || // (!vanterior.circunferencia && !vactual.circunferencia) <-suficiente
                             (!vanterior.circunferencia && !vactual.circunferencia && vsiguiente.circunferencia) ||
                             (vanterior.circunferencia && !vactual.circunferencia && !vsiguiente.circunferencia) ||
                              (vanterior.circunferencia && !vactual.circunferencia && vsiguiente.circunferencia)
                             )
                    {
                        pc_intercambio = new puntos_criticos();
                        pc_intercambio.punto = distancia_acumulada;
                        pc_intercambio.accion = 'R';
                        pc_intercambio.angulo = 0;
                        p_criticos_R.Add(pc_intercambio);
                    }
                    else if (vanterior.circunferencia && vactual.circunferencia && !vsiguiente.circunferencia)
                    {
                        /*pc_intercambio = new puntos_criticos();
                        pc_intercambio.punto = distancia_acumulada+vactual.distancia;
                        pc_intercambio.accion = 'R';
                        pc_intercambio.angulo = 0;
                        p_criticos.Add(pc_intercambio);*/
                        pc_intercambio = new puntos_criticos();
                        pc_intercambio.punto = distancia_acumulada + distancia_espatula;
                        pc_intercambio.accion = 'D';
                        pc_intercambio.angulo = angulo;
                        p_criticos_D.Add(pc_intercambio);
                    }
                    else
                    {
                        MessageBox.Show(String.Format("a JA!,VANC{0}-VACC_{1}:VSC{2}", vanterior.circunferencia, vactual.circunferencia, vsiguiente.circunferencia));
                    }
                    distancia_acumulada += vactual.distancia;
                }
                pc_intercambio = new puntos_criticos();
                pc_intercambio.punto = distancia_acumulada;
                pc_intercambio.accion = 'R';
                pc_intercambio.angulo = 0;
                p_criticos_R.Add(pc_intercambio);

                //////////////////////////////////////
                //////Ordenar puntos criticos/////////
                //////////////////////////////////////
                p_criticos_Union = new List<puntos_criticos>();
                int cantidad_doblados = p_criticos_D.Count();
                int cantidad_ranurados = p_criticos_R.Count();
                for (int x = 0; x < cantidad_ranurados; x++)
                {                    
                    for(int y=0; y < cantidad_doblados ; y++)
                    {
                        if (p_criticos_D.Count()==0 )
                        {
                            break;
                        }
                        if (p_criticos_D.ElementAt(0).punto < p_criticos_R.ElementAt(0).punto)
                        {
                            p_criticos_Union.Add(p_criticos_D.ElementAt(0));
                            p_criticos_D.RemoveAt(0);                            
                        }
                        else
                        {
                            p_criticos_Union.Add(p_criticos_R.ElementAt(0));
                            p_criticos_R.RemoveAt(0);                            
                            break;
                        }
                    }
                    if (p_criticos_R.Count()==0 || p_criticos_D.Count() == 0)
                    {
                        break;
                    }
                }      
                if(p_criticos_D.Count()>0)
                {
                    foreach (puntos_criticos pc in p_criticos_D)
                    {
                        p_criticos_Union.Add(pc);
                    }
                    
                }
                if (p_criticos_R.Count() > 0)
                {
                    foreach (puntos_criticos pc in p_criticos_R)
                    {
                        p_criticos_Union.Add(pc);
                    }                    
                }
                p_criticos_D = new List<puntos_criticos>();
                p_criticos_R = new List<puntos_criticos>();
                txt_dist.Text = distancia.ToString()+" + ("+ distancia_espatula.ToString() +")";
                //////////////////////////////////////
                /////Generar Codigo de la maquina/////
                //////////////////////////////////////
                int cantidad_puntos_criticos = p_criticos_Union.Count();
                for(int x=0; x < cantidad_puntos_criticos; x++)
                {
                    if(x==0)
                    {
                        p_criticos_Union.ElementAt(x);
                        lb_segmentos.Items.Add("D"+distancia_ranurado);
                    }
                    else
                    {
                        double recorrido_avance = Math.Round((p_criticos_Union.ElementAt(x).punto - p_criticos_Union.ElementAt(x - 1).punto),2);
                        lb_segmentos.Items.Add("C" + recorrido_avance);
                        if(p_criticos_Union.ElementAt(x).accion=='R')
                        {
                            lb_segmentos.Items.Add("D" + distancia_ranurado);
                        }
                        else if(p_criticos_Union.ElementAt(x).accion=='D')
                        {
                            if (p_criticos_Union.ElementAt(x).angulo > 0)
                            {
                                angulo = p_criticos_Union.ElementAt(x).angulo + 90 + compensadorA;
                                if (angulo > 165)
                                    angulo = 165;
                                lb_segmentos.Items.Add(String.Format("B000"));
                                lb_segmentos.Items.Add(String.Format("A{0}", Math.Abs(angulo)));
                                lb_segmentos.Items.Add(String.Format("A085"));
                            }
                            if (p_criticos_Union.ElementAt(x).angulo < 0)
                            {
                                angulo = p_criticos_Union.ElementAt(x).angulo - 90 - compensadorB;
                                if (angulo < -165)
                                    angulo = -165;
                                lb_segmentos.Items.Add(String.Format("A000"));
                                lb_segmentos.Items.Add(String.Format("B{0}", Math.Abs(angulo)));
                                lb_segmentos.Items.Add(String.Format("B085"));
                            }
                        }

                    }
                }
                //////////////////////////////////////
                //////////////BORRAR LUEGO////////////
                //////////////////////////////////////                
                lb_union.Items.Clear();
               /* foreach (puntos_criticos pc in p_criticos_R)
                {
                    lb_R.Items.Add(pc.punto.ToString());
                }
                foreach (puntos_criticos pc in p_criticos_D)
                {
                    lb_D.Items.Add(pc.punto.ToString());
                }*/
                foreach (puntos_criticos pc in p_criticos_Union)
                {
                    lb_union.Items.Add(pc.punto.ToString()+":"+pc.accion);
                }
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
                /////////////////////////////////////
                /////////////////////////////////////
                /////////////////////////////////////
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
