using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
namespace Controlador
{
    class Conexion
    {
        private static Conexion instancia;
        //private static Conexion instancia_lectura;
        private static SerialPort Maquina;
        //private static Queue<String> lista;
        private String resp;
        public Principal Principal { get; set; }

        private Conexion() { }        
        public static Conexion Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new Conexion();
                }
                return instancia;
            }
        }
        public void crearConexion(String Puerto) {
            if (Maquina != null)
            {
                Maquina.Close();
                Maquina = null;
            }            
            Maquina = new SerialPort(Puerto, 9600, Parity.None, 8, StopBits.One);
            Maquina.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            Maquina.Open();
        }
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //DataReceive = Maquina.ReadLine();
            //Principal.nvo_dato(Maquina.ReadLine());
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            int index;
            string envio;
            if (indata.ToString().IndexOf("\r", StringComparison.CurrentCultureIgnoreCase) != -1)
            {
                index = indata.ToString().IndexOf("\r", StringComparison.CurrentCultureIgnoreCase);
                for (int x=0; x<index;x++)
                {
                    resp += indata.ToString()[x];
                }
                envio = resp;
                resp = "";
                if (indata.ToString().Count()>index) {
                    for (int x = index;x< indata.ToString().Count();x++)
                    {
                        if (indata.ToString()[x] != '\r')
                        {
                            resp += indata.ToString()[x];
                        }
                    }
                }
                Principal.nvo_dato(envio);
                envio = "";
            }
            else
            {
                resp += indata.ToString();
            }
            //Console.WriteLine("Data Received:");
            //Console.Write(indata);

            //Principal.nvo_dato(indata.ToString());

        }
      /*  private static void port_OnReceiveDatazz(object sender,
                           SerialDataReceivedEventArgs e)
        {
            SerialPort spL = (SerialPort)sender;
            const int bufSize = 12;
            Byte[] buf = new Byte[bufSize];
            Console.WriteLine("DATA RECEIVED!");
            Console.WriteLine(spL.Read(buf, 0, bufSize));
        }
        */
        public void enviarComando(String comando)
        {
            try
            {                
                Maquina.WriteLine(comando);
                //return Maquina.ReadLine();
               // return "s";
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
                //return "Error al conectarse con la maquina";
            }
        }
    }
}
