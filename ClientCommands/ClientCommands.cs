using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ClientCommands
{
     public class ClientServerCommand
    {
        Socket client { get; set; }
        Socket server { get; set; }

        public string _answer = "";

        string error = "";
        
        byte[] buffer = new byte[1024];
        public async Task ConnectServer(IPEndPoint point, Socket clientSocket)
        {
            await Task.Run(async () =>
            {

                try
                {
                    client = clientSocket;
                    Mutex mutex = new Mutex();
                    await Task.Run(async () =>
                    client.BeginConnect(point, (IAsyncResult result) =>
                    {
                        server = (Socket)result.AsyncState;
                        if (server.Connected)
                        {

                            Task.Run(() => server.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReciveMessage, server);
                        }
                    }, client)
                );
                    mutex.WaitOne(1000);
                    mutex.ReleaseMutex();
                    mutex.Close();

                 }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            });
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

        void ReciveMessage(IAsyncResult result)
        {
            Socket server = (Socket)result.AsyncState;
            server.EndReceive(result);
            _answer = Encoding.UTF8.GetString(buffer);

        }
    }

}
