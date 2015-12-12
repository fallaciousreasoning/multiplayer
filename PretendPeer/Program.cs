using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace PretendPeer
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new NetPeerConfiguration("MultiPlayer");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.AcceptIncomingConnections = true;
            config.Port = 5002;

            var Me = new NetPeer(config);
            Me.Start();
            Me.DiscoverLocalPeers(5001);

            while (true)
            {
                NetIncomingMessage message;
                while ((message = Me.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.Data:
                            Console.WriteLine(message.ReadString());
                            break;
                        case NetIncomingMessageType.DiscoveryRequest:
                            var response = Me.CreateMessage("Uuum hi..?");
                            Me.SendDiscoveryResponse(response, message.SenderEndPoint);
                            Console.WriteLine($"Someone at {message.SenderEndPoint.Address} is attempting to discover us!");
                            break;
                        case NetIncomingMessageType.DiscoveryResponse:
                            Me.Connect(message.SenderEndPoint);
                            Console.WriteLine($"Peer discovered at {message.SenderEndPoint.Address}. Attempting to connect.");
                            //TODO connect?
                            break;
                        case NetIncomingMessageType.ConnectionApproval:
                            message.SenderConnection.Approve();
                            Console.WriteLine($"Uuum. Someone connected. I don't really know what to do now...");
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            Console.WriteLine(message.ReadString());
                            break;
                        default:
                            Console.WriteLine(message.ReadString());
                            break;
                    }
                }
            }
        }
    }
}
