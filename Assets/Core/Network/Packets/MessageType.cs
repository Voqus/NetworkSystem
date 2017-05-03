namespace Core.Network.Packets
{
    public enum MessageType : short
    {
        GLOBAL              = 0x01,
        WORLD               = 0x02,
        LOCAL               = 0x03,
        REGION              = 0x04,
        PERSONAL_MESSAGE    = 0x05
    }
}
