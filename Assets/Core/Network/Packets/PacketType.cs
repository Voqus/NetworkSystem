namespace Core.Network
{
    public enum PacketType : short
    {
        INVALID     = -0x01,
        LOGIN       = 0x00,
        DISCONNECT  = 0x01,
        MESSAGE     = 0x02,
        MOVE        = 0x03,
        SHOOT       = 0x04,
        EXIT        = 0x05,
        SPAWN       = 0x06
    }
}