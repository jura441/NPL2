using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NPLesson2Server
{
    public partial class Form1 : Form
    {
        Socket server;
        IPEndPoint point;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 80);
        }

        private void btn_startServer_Click(object sender, EventArgs e)
        {
            if (server == null)
                return;
            server.Bind(point);
            server.Listen(100);
            tmr_refreshConnection.Start();            
        }

        private void btn_stopServer_Click(object sender, EventArgs e)
        {
            tmr_refreshConnection.Stop();            
        }

        private void tmr_refreshConnection_Tick(object sender, EventArgs e)
        {
            try
            {                
                server.BeginAccept(ServerAcceptDelegate, server);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Close();
        }

        void ServerAcceptDelegate(IAsyncResult result)
        {
            if (result != null)
            {
                Socket serv = (Socket)result.AsyncState;
                Socket clientsocket = serv.EndAccept(result);
                clientsocket.Send(Encoding.UTF8.GetBytes("Успешное подключение."));
                clientsocket.Shutdown(SocketShutdown.Send);
                clientsocket.Close();
                server.BeginAccept(ServerAcceptDelegate, serv);
            }
        }

    }
}