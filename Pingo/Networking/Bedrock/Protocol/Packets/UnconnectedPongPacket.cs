﻿namespace Pingo.Networking.Bedrock.Protocol.Packets;

internal sealed class UnconnectedPongPacket : IIngoingPacket<UnconnectedPongPacket>
{
    public static int Identifier => 0x1C;

    public required long Time { get; init; }

    public required long Server { get; init; }

    public required string Message { get; init; }

    public static UnconnectedPongPacket Read(MemoryReader reader)
    {
        var time = reader.ReadLong();
        var server = reader.ReadLong();
        reader.ReadMagic();

        return new UnconnectedPongPacket
        {
            Time = time,
            Server = server,
            Message = reader.ReadVariableString(false)
        };
    }
}