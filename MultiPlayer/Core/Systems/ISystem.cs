using MultiPlayer.Core.Messaging;

namespace MultiPlayer.Core.Systems
{
    public interface ISystem
    {
        void RecieveMessage(IMessage message);
    }
}
