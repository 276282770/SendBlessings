using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientController
{
    public class Udp
    {
        UdpClient client = null;
        IPEndPoint remoteEndPoint;
        public Udp()
        {

        }
        public Udp(int port)
        {
            client = new UdpClient(port);
        }
        public void Send(byte[] data,string ip,int port)
        {
            if (client == null)
                return;

            client.SendAsync(data, data.Length, ip, port);
        }
        public void Send(string data)
        {
            if (remoteEndPoint == null)
                return;
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            client.SendAsync(buffer, buffer.Length, remoteEndPoint);
        }
        public async Task<string> Receive()
        {
            UdpReceiveResult rsvResult=await client.ReceiveAsync();
            string result= Encoding.UTF8.GetString(rsvResult.Buffer);
            remoteEndPoint = rsvResult.RemoteEndPoint;
            return result;
        }
        public void Close()
        {
            client.Close();
        }
    }


}