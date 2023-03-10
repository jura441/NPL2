using System.Net.Sockets;
using System.Net;
using System.Text;

namespace NPLesson2
{
    public partial class Form1 : Form
    {
        Socket client;
        IPEndPoint point;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_connectServer_Click(object sender, EventArgs e)
        {
            try
            { 
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                point = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 80);
                client.BeginConnect(point, (IAsyncResult result) => {
                    Socket clientAsync = (Socket)result.AsyncState;
                    if (clientAsync.Connected)
                    {
                        byte[] buffer = new byte[1024];
                        int answerServer = clientAsync.Receive(buffer);
                        while (answerServer > 0)
                        {
                            rtb_chat.Text += Encoding.UTF8.GetString(buffer);
                            answerServer = clientAsync.Receive(buffer);
                        }
                    }
                    client.EndConnect(result);

                }, client);
                
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_disconnectServer_Click(object sender, EventArgs e)
        {
            if (client != null) 
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }

        private void btn_choiceContact_Click(object sender, EventArgs e)
        {

        }

        private void btn_sendMessage_Click(object sender, EventArgs e)
        {

        }

        private void tb_message_TextChanged(object sender, EventArgs e)
        {

        }

        private void tmr_refreshConnection_Tick(object sender, EventArgs e)
        {

        }
    }
}