using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controlador
{
    public partial class Graficas : Form
    {
        //borrar
       // int tempsum = 26;

        private Conexion_lectura conexion;
        private bool encendido = false;
        
        int contador = 0;
        int contadorPuntos = 0;

        /// <summary>
        /// Coenficientes ara el filtro FIR
        /// </summary>
        double[] b = new double[]
                    {
                           /* 0.0285798399416968,
                            0.07328836181028257,
                            0.04512928732568178,
                            -0.03422632401030227,
                            -0.03472426238662957,
                            0.053430907613764095,
                            0.032914528649623485,
                            -0.0988081824627219,
                            -0.03413542207884341,
                            0.3160339484471911,
                            0.5341936566511764,
                            0.3160339484471911,
                            -0.03413542207884341,
                            -0.0988081824627219,
                            0.032914528649623485,
                            0.053430907613764095,
                            -0.03472426238662957,
                            -0.03422632401030227,
                            0.04512928732568178,
                            0.07328836181028257,
                            0.0285798399416968*/
                        0.03691010922015711,
                        0.012988281252209015,
                       -0.12838225565887149,
                       -0.0979382552688825,
                        0.2873486569764788,
                        0.5520677231955425,
                        0.2873486569764788,
                       -0.0979382552688825,
                       -0.12838225565887149,
                        0.012988281252209015,
                        0.03691010922015711
                    };
        /////////////////////////////////////////

        double limitesuperior = 0.65;
        double limiteinferior = -0.65;

        double ejeXant = 0;
        double ejeYant = 0;
        double ejeZant = 0;

        double velXant = 0;
        double velYant = 0;
        double velZant = 0;

        double velXnva = 0;
        double velYnva = 0;
        double velZnva = 0;

        double disXant = 0;
        double disYant = 0;
        double disZant = 0;

        double disXnva = 0;
        double disYnva = 0;
        double disZnva = 0;

        double[] arrayMuestrasAX = new double[100];
        double[] arrayMuestrasAY = new double[100];
        double[] arrayMuestrasAZ = new double[100];        

        public void nvo_dato(string mensajeSerial)
        {
          //  lb_temp.Items.Add(String.Format("{0} - {1}",
           //     mensajeSerial,DateTime.Now.Millisecond.ToString()));
            if (encendido)
            {
                string[] valor = mensajeSerial.Split(':');
                
                if (valor.Count() == 4 && valor[0] == "vibr")
                {
                    try
                    {
                        arrayMuestrasAX[contador] = double.Parse(valor[1]);
                        arrayMuestrasAY[contador] = double.Parse(valor[2]);
                        arrayMuestrasAZ[contador] = double.Parse(valor[3]);
                        contador++;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                if (valor.Count() == 4 && valor[0] == "temp")
                {
                    listBox1.Items.Add(String.Format("{0}:{1}:{2}", valor[0], valor.Count(), mensajeSerial));
                    double temp1 = double.Parse(valor[1]);
                    double temp2 = double.Parse(valor[2]);
                    double temp3 = double.Parse(valor[3]);
                    if ((temp1 > 0 && temp1 < 120) &&
                        (temp2 > 0 && temp2 < 120) &&
                        (temp3 > 0 && temp3 < 120))
                    {
                       /* if (graficaTemp.Series[0].Points.Count() == 250)
                        {
                            graficaTemp.Series[0].Points.RemoveAt(0);
                            graficaTemp.Series[1].Points.RemoveAt(0);
                            graficaTemp.Series[2].Points.RemoveAt(0);
                        }*/
                        graficaTemp.Series[0].Points.Add(temp1);
                        graficaTemp.Series[1].Points.Add(temp2);
                        graficaTemp.Series[2].Points.Add(temp3);
                        //MessageBox.Show(temp1.ToString());
                    }
                }
            }
        }
        public void Medicion(bool encender)
        {
            if (encender)
            {
                encendido = true;
                //conexion = Conexion_lectura.Instancia;
                //conexion.Graficas = this;
                conexion.enviarComando("T0\r");
                conexion.enviarComando("L0\r");
                timer1.Enabled = true;
            }
            else
            {
                encendido = false;
                conexion.enviarComando("T1\r");
                conexion.enviarComando("L1\r");
                timer1.Enabled = false;
            }

        }

        public Graficas()
        {
            InitializeComponent();
        }

        private void Graficas_Load(object sender, EventArgs e)
        {            
            CheckForIllegalCrossThreadCalls = false;
            conexion = Conexion_lectura.Instancia;
            //conexion = Conexion.Instancia;
            //conexion.Principal = this;
            /* string[] ports = SerialPort.GetPortNames();
             foreach (String st in ports)
             {
                 cb_menu.Items.Add(st);
             }
             */
            ///////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////

            chart1.Series[3].Color = System.Drawing.Color.Red;
            chart1.Series[4].Color = System.Drawing.Color.Red;
            chart1.Series[3].BorderWidth = 3;
            chart1.Series[4].BorderWidth = 3;
            chart1.ChartAreas[0].AxisY.Minimum = -0.5;
            chart1.ChartAreas[0].AxisY.Maximum = 0.5;

            graficaTemp.ChartAreas[0].AxisY.Minimum = 15;
            graficaTemp.ChartAreas[0].AxisY.Maximum = 90;
            ///////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!encendido)
            {
               // button1.Text = "Vibración Off";
                encendido = true;
              //  timer1.Enabled = true;
                contadorPuntos = 0;
                conexion.enviarComando("L0\r");
                conexion.enviarComando("T0\r");

            }
            else
            {
             //   button1.Text = "Vibración On";
                conexion.enviarComando("L1\r");
                conexion.enviarComando("T1\r");

                encendido = false;
             //   timer1.Enabled = false;
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (this.contador > 0)
            {
                int contador = this.contador;

                double[] arrayFiltradoX = new double[contador];
                double[] arrayFiltradoY = new double[contador];
                double[] arrayFiltradoZ = new double[contador];

                /*double[] arrayMuestrasVelX = new double[contador];
                double[] arrayMuestrasVelY = new double[contador];
                double[] arrayMuestrasVelZ = new double[contador];

                double[] arrayMuestrasDisX = new double[contador];
                double[] arrayMuestrasDisY = new double[contador];
                double[] arrayMuestrasDisZ = new double[contador];
                */

                double mediaX = 0;
                double mediaY = 0;
                double mediaZ = 0;

                if (contadorPuntos > 350)
                {
                    for (int x = 0; x < (contador); x++)
                    {
                        chart1.Series[0].Points.RemoveAt(0);
                        chart1.Series[1].Points.RemoveAt(0);
                        chart1.Series[2].Points.RemoveAt(0);
                    }
                    //chart1.ChartAreas[0].AxisX.Minimum = chart1.ChartAreas[0].AxisX.Minimum + contador;
                }
                Array.Copy(arrayMuestrasAX, arrayFiltradoX, contador);
                Array.Copy(arrayMuestrasAY, arrayFiltradoY, contador);
                Array.Copy(arrayMuestrasAZ, arrayFiltradoZ, contador);
                for (int x = 0; x < contador; x++)
                {
                    mediaX += arrayFiltradoX[x];
                    mediaY += arrayFiltradoY[x];
                    mediaZ += arrayFiltradoZ[x];
                }
                mediaX = mediaX / contador;
                mediaY = mediaY / contador;
                mediaZ = mediaZ / contador;
                ///////////////////////////////////////////////////////////////////////
                //////////////////// APLICANDO FILTRO  ///////////////////////////////
                ////////////////  1er filtro Aceleracion  ///////////////////////////
                ////////////////////////////////////////////////////////////////////
                /*
                Filtro pasa baja Butterworth
                frecencia de corte 25hz
                */
                // arrayFiltradoX = func_FIR(arrayFiltradoX, b);
                // arrayFiltradoY = func_FIR(arrayFiltradoY, b);
                // arrayFiltradoZ = func_FIR(arrayFiltradoZ, b);
                //   arrayFiltradoX = Butterworth(arrayFiltradoX, .01, 25);
                //   arrayFiltradoY = Butterworth(arrayFiltradoY, .01, 25);
                //   arrayFiltradoZ = Butterworth(arrayFiltradoZ, .01, 25);
                ///////////////////////////////////////////////////////////////////////
                //////////////////// Calculando 1era integral (A -> V) ///////////////
                /////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////
                /*for (int x = 0; x < contador; x++)
                {
                    velXnva = velXant + ((arrayFiltradoX[x] + ejeXant) / 2) * .01;
                    arrayMuestrasVelX[x] = velXnva;
                    velXant = velXnva;
                    ejeXant = arrayFiltradoX[x];

                    velYnva = velYant + ((arrayFiltradoY[x] + ejeYant) / 2) * .01;
                    arrayMuestrasVelY[x] = velYnva;
                    velYant = velYnva;
                    ejeYant = arrayFiltradoY[x];

                    velZnva = velZant + ((arrayFiltradoZ[x] + ejeZant) / 2) * .01;
                    arrayMuestrasVelZ[x] = velZnva;
                    velZant = velZnva;
                    ejeZant = arrayFiltradoZ[x];
                }*/
                ///////////////////////////////////////////////////////////////////////
                //////////////////// APLICANDO FILTRO   //////////////////////////////
                ////////////////  2d  filtro Velocidad  /////////////////////////////
                ////////////////////////////////////////////////////////////////////
                //arrayMuestrasVelX = func_FIR(arrayMuestrasVelX, b);
                //arrayMuestrasVelY = func_FIR(arrayMuestrasVelY, b);
                //arrayMuestrasVelZ = func_FIR(arrayMuestrasVelZ, b);
                //    arrayMuestrasVelX = Butterworth(arrayMuestrasVelX, .01, 25);
                //    arrayMuestrasVelY = Butterworth(arrayMuestrasVelY, .01, 25);
                //    arrayMuestrasVelZ = Butterworth(arrayMuestrasVelZ, .01, 25);
                ///////////////////////////////////////////////////////////////////////
                //////////////////// Calculando 2da integral (V -> D) ////////////////
                /////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////
                /*for (int x = 0; x < contador; x++)
                {
                    //disXnva = disXant + ((arrayMuestrasVelX[x] + velXant) / 2) * .01;
                    //arrayMuestrasDisX[x] = disXnva;
                    //velXant = arrayMuestrasVelX[x];
                    //disXant = disXnva;

                    //disYnva = disYant + ((arrayMuestrasVelY[x] + velYant) / 2) * .01;
                    //arrayMuestrasDisY[x] = disYnva;
                    //velYant = arrayMuestrasVelY[x];
                    //disYant = disYnva;

                     //disZnva = disZant + ((arrayMuestrasVelZ[x] + velZant) / 2) * .01;
                    //arrayMuestrasDisZ[x] = disZnva;
                    //velZant = arrayMuestrasVelZ[x];
                    ///disZant = disZnva;/
                    disXnva = disXant + ((arrayMuestrasVelX[x] + velXant) / 2) * .01;
                    arrayMuestrasDisX[x] = disXnva;
                    velXant = arrayMuestrasVelX[x];
                    disXant = disXnva;

                    disYnva = disYant + ((arrayMuestrasVelY[x] + velYant) / 2) * .01;
                    arrayMuestrasDisY[x] = disYnva;
                    velYant = arrayMuestrasVelY[x];
                    disYant = disYnva;

                    disZnva = disZant + ((arrayMuestrasVelZ[x] + velZant) / 2) * .01;
                    arrayMuestrasDisZ[x] = disZnva;
                    velZant = arrayMuestrasVelZ[x];
                    disZant = disZnva;
                }*/
                ///////////////////////////////////////////////////////////////////////
                //////////////////// APLICANDO FILTRO   //////////////////////////////
                ////////////////  3er  filtro distancia  ////////////////////////////
                ////////////////////////////////////////////////////////////////////
                //arrayMuestrasDisX = func_FIR(arrayMuestrasDisX, b);
                //arrayMuestrasDisY = func_FIR(arrayMuestrasDisY, b);
                //arrayMuestrasDisZ = func_FIR(arrayMuestrasDisZ, b);
                //    arrayMuestrasDisX = Butterworth(arrayMuestrasDisX, .01, 25);
                //    arrayMuestrasDisY = Butterworth(arrayMuestrasDisY, .01, 25);
                //    arrayMuestrasDisZ = Butterworth(arrayMuestrasDisZ, .01, 25);
                ///////////////////////////////////////////////////////////////////////
                //////////////////// AGREGANDO PUNTOS ////////////////////////////////            
                /////////////////////////////////////////////////////////////////////
                for (int x = 0; x < contador; x++)
                {
                    chart1.Series[0].Points.Add(arrayFiltradoX[x] - mediaX);
                    chart1.Series[1].Points.Add(arrayFiltradoY[x] - mediaY);
                    chart1.Series[2].Points.Add(arrayFiltradoZ[x] - mediaZ);
                    contadorPuntos++;
                }
             //   graficaTemp.Series[0].Points.Add(tempsum);
               // graficaTemp.Series[1].Points.Add(tempsum+1);
                //graficaTemp.Series[2].Points.Add(tempsum-1.2);
                
               // tempsum = tempsum + 1;
              //  if (tempsum == 32) tempsum = 26;
                this.contador = 0;
            }
        }
        //--------------------------------------------------------------------------
        // This function returns the data filtered. Converted to C# 2 July 2014.
        // Original source written in VBA for Microsoft Excel, 2000 by Sam Van
        // Wassenbergh (University of Antwerp), 6 june 2007.
        // https://www.codeproject.com/Tips/1092012/A-Butterworth-Filter-in-Csharp
        //--------------------------------------------------------------------------
        public static double[] Butterworth(double[] indata, double deltaTimeinsec, double CutOff)
        {
            if (indata == null) return null;
            if (CutOff == 0) return indata;

            double Samplingrate = 1 / deltaTimeinsec;
            long dF2 = indata.Length - 1;        // The data range is set with dF2
            double[] Dat2 = new double[dF2 + 4]; // Array with 4 extra points front and back
            double[] data = indata; // Ptr., changes passed data

            // Copy indata to Dat2
            for (long r = 0; r < dF2; r++)
            {
                Dat2[2 + r] = indata[r];
            }
            Dat2[1] = Dat2[0] = indata[0];
            Dat2[dF2 + 3] = Dat2[dF2 + 2] = indata[dF2];

            const double pi = 3.14159265358979;
            double wc = Math.Tan(CutOff * pi / Samplingrate);
            double k1 = 1.414213562 * wc; // Sqrt(2) * wc
            double k2 = wc * wc;
            double a = k2 / (1 + k1 + k2);
            double b = 2 * a;
            double c = a;
            double k3 = b / k2;
            double d = -2 * a + k3;
            double e = 1 - (2 * a) - k3;

            // RECURSIVE TRIGGERS - ENABLE filter is performed (first, last points constant)
            double[] DatYt = new double[dF2 + 4];
            DatYt[1] = DatYt[0] = indata[0];
            for (long s = 2; s < dF2 + 2; s++)
            {
                DatYt[s] = a * Dat2[s] + b * Dat2[s - 1] + c * Dat2[s - 2]
                           + d * DatYt[s - 1] + e * DatYt[s - 2];
            }
            DatYt[dF2 + 3] = DatYt[dF2 + 2] = DatYt[dF2 + 1];

            // FORWARD filter
            double[] DatZt = new double[dF2 + 2];
            DatZt[dF2] = DatYt[dF2 + 2];
            DatZt[dF2 + 1] = DatYt[dF2 + 3];
            for (long t = -dF2 + 1; t <= 0; t++)
            {
                DatZt[-t] = a * DatYt[-t + 2] + b * DatYt[-t + 3] + c * DatYt[-t + 4]
                            + d * DatZt[-t + 1] + e * DatZt[-t + 2];
            }

            // Calculated points copied for return
            for (long p = 0; p < dF2; p++)
            {
                data[p] = DatZt[p];
            }

            return data;
        }

        public static double[] func_FIR(double[] x, double[] b)
        {
            //y[n]=b0x[n]+b1x[n-1]+....bmx[n-M]
            int M = b.Count();
            int n = x.Count();

            double t = 0.0;
            double[] y = new double[n];

            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < M; i++)
                {
                    t += b[i] * x[n - i - 1];
                }
                y[j] = t;
            }

            return y;
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void lb_temp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void graficaTemp_Click(object sender, EventArgs e)
        {

        }
    }
}