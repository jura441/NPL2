using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;


namespace ClientCommands
{
     public class ClientServerCommand
    {
        Socket client { get; set; }
        Socket server { get; set; }

        public string _answer = "";

        string error = "";
        public bool ConnectServer(IPEndPoint point, Socket clientSocket)
        {
            try
            {
                client = clientSocket;
                client.BeginConnect(point, (IAsyncResult result) =>
                {
                    server = (Socket)result.AsyncState;
                    if (server.Connected)
                    {

                        byte[] buffer = new byte[1024];
                        ArraySegment<byte> segment = new ArraySegment<byte>(buffer, 0, buffer.Length);
                        Task<int> message = server.ReceiveAsync(segment, SocketFlags.None);
                        if (message.IsCompleted)
                        { 
                            _answer += Encoding.UTF8.GetString(buffer);
                        }
                        
                    }
                }, client);
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
            return _answer == "Успешное подключение." ? true : false;
        }
        public void SendMessage(string message)
        {
            if (server.Connected)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                ArraySegment<byte> segment = new ArraySegment<byte>(buffer, 0, buffer.Length);
                client.SendAsync(segment, SocketFlags.None);
            }
        }
        public bool ServerIsConnected()
        {
            return server != null ? true : false;
        }
    }
}
