using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using MultiPlayer.Core;

namespace MultiPlayer.GameComponents
{
    public class NetworkManager : IStartable, IHearsDestroy, IUpdateable
    {
        private const string APP_IDENTIFIER = "MultiPlayer";
        private const int PORT = 5001;

        public NetPeer Me;

        public NetworkManager()
        {
            var config = new NetPeerConfiguration(APP_IDENTIFIER);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.AcceptIncomingConnections = true;
            config.Port = PORT;

            Me = new NetPeer(config);
            Me.Start();

            Me.DiscoverLocalPeers(5002);
        }

        public void Start()
        {
        }

        public void OnDestroy()
        {
            Me.Shutdown("Bye!");
        }

        public void Update(float step)
        {
            NetIncomingMessage message;
            while ((message = Me.ReadMessage()) != null)
            {
                switch (message.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        //TODO parse whatever I senet
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        var response = Me.CreateMessage("Uuum hi..?");
                        Me.SendDiscoveryResponse(response, message.SenderEndPoint);
                        Logger.Log($"Someone at {message.SenderEndPoint.Address} is attempting to discover us!");
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        Me.Connect(message.SenderEndPoint);
                        Logger.Log($"Peer discovered at {message.SenderEndPoint.Address}. Attempting to connect.");
                        //TODO connect?
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        message.SenderConnection.Approve();
                        Logger.Log($"Uuum. Someone connected. I don't really know what to do now...");
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        //Logger.Log(message.ReadString());
                        break;
                }
            }
            if (Me.Connections.Count > 0)
                Me.Connections[0].SendMessage(Me.CreateMessage("hey server!!"), NetDeliveryMethod.ReliableOrdered, 0);

        }
    }
}
