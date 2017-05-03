namespace Core.Network.Packets
{
    public class Message
    {
        public MessageType MessageType;
        public string Data;

        public Message(MessageType messageType, string data)
        {
            MessageType = messageType;
            Data = data;
        }
    }
}
