using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpNetMatch
{
    class CbNetwork
    {
        public UdpClient Client { get; private set; }


        public int Uid { get; internal set; }

        public CbNetwork()
        {

        }

        public Packet ClientRead()
        {
            Packet p = new Packet();
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 29929);
            p.memBlock = Client.Receive(ref ep);
            return p;
        }
        public delegate void PacketReceivedEventHandler(object sender, PacketEventArgs e);
        public event PacketReceivedEventHandler PacketReceived;

        public async void ClientReadInternal()
        {
            var d = await Client.ReceiveAsync();
            if (d.RemoteEndPoint.Port == 29929)
            {
                Packet p = new Packet();
                p.memBlock = d.Buffer;
                if (PacketReceived != null)
                {
                    PacketReceived(this, new PacketEventArgs(p));
                }
            }
            ClientReadInternal();
        }

        public void ClientSend(Packet data)
        {
            data.ClientId = this.Uid;
            System.Diagnostics.Debug.WriteLine("s " + BitConverter.ToString(data.memBlock));
            this.Client.Send(data.memBlock, data.memBlock.Length);
        }

        public void ClientSendBack()
        {

        }

        public void ClientState()
        {

        }

        public void CloseClient()
        {
            Client.Client.Close();
        }

        public void InitClient(string host, int port)
        {
            Client = new UdpClient(host, port);
            Client.Connect(host, port);
            Client.Send(new byte[] { 0x0, 0x0 }, 2);
            ClientReadInternal();
        }
    }
    public class PacketEventArgs : EventArgs
    {
        public Packet p { get; private set; }
        public PacketEventArgs(Packet p)
        {
            this.p = p;
        }
    }
}
