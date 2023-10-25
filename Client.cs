using System;
using System.Net.Sockets;

namespace GrafittiServer
{
    public class Client
    {
        public int connectionID;
        //public string ip;
        public TcpClient socket;
        public NetworkStream stream;
        private byte[] recBuff;
        public ByteBuffer buffer;

        public void Start()
        {
            socket.ReceiveBufferSize = 4096;
            socket.SendBufferSize = 4096;
            stream = socket.GetStream();
            recBuff = new byte[4096];
            stream.BeginRead(recBuff, 0, socket.ReceiveBufferSize, OnReceiveData, null);
            Console.WriteLine("Incomming connection from'{0}'.", socket.Client.RemoteEndPoint.ToString());
        }

        private void OnReceiveData(IAsyncResult result)
        {
            try
            {
                int length = stream.EndRead(result);
                if (length <= 0)
                {
                    CloseConnection();
                    return;
                }
                byte[] newBytes = new byte[length];
                Array.Copy(recBuff, newBytes, length);
                ServerHandleData.HandleData(connectionID, newBytes);
                stream.BeginRead(recBuff, 0, socket.ReceiveBufferSize, OnReceiveData, null);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
                CloseConnection();
                return;
            }
        }

        private void CloseConnection()
        {
            Console.WriteLine("Connection from '{0}' has been terminated.", socket.Client.RemoteEndPoint.ToString());
            Types.tempPlayer.Remove(connectionID);
            Types.player.Remove(connectionID);
            socket.Close();
            socket = null;
        }

       
    }
}
