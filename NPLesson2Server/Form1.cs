using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NPLesson2Server
{
    public partial class Form1 : Form
    {
        Socket? server;
        IPEndPoint? point;
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
            tmr_refreshConnection.Start();
            server.Bind(point);
            server.Listen(100);
        }

        private void btn_stopServer_Click(object sender, EventArgs e)
        {
            tmr_refreshConnection.Stop();  
            
        }

        private void tmr_refreshConnection_Tick(object sender, EventArgs e)
        {
            try
            {                
                // проверка на пустоту принимаемых Socket
                server.BeginAccept((IAsyncResult result) => {
                    if (result != null)
                    {
                        Socket clientsocket = server.EndAccept(result);                    
                        //rtb_clients.Text += clientsocket.RemoteEndPoint.ToString();
                        clientsocket.Send(Encoding.UTF8.GetBytes("Успешное подключение."));
                       // 
                    }
                }, server);
                /*ArraySegment<byte> buffer = new ArraySegment<byte>();
                client.SendAsync(buffer, SocketFlags.None);
                Thread.Sleep(100);*/

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
    }
}