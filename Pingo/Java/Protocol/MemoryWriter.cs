﻿using System.Buffers.Binary;
using System.Text;

namespace Pingo.Java.Protocol;

internal ref struct MemoryWriter(Memory<byte> memory)
{
    public int Position { get; private set; }

    private readonly Span<byte> span = memory.Span;

    public void Write(ReadOnlySpan<byte> value)
    {
        value.CopyTo(span[Position..(Position += value.Length)]);
    }

    public void WriteVariableInteger(int value)
    {
        var unsigned = (uint) value;

        do
        {
            var current = (byte) (unsigned & 127);
            unsigned >>= 7;

            if (unsigned != 0)
            {
                current |= 128;
            }

            span[Position++] = current;
        } while (unsigned != 0);
    }

    public void WriteVariableString(string value)
    {
        WriteVariableInteger(Encoding.UTF8.GetByteCount(value));
        Position += Encoding.UTF8.GetBytes(value, span[Position..]);
    }

    public void WriteUnsignedShort(ushort value)
    {
        BinaryPrimitives.WriteUInt16BigEndian(span[Position..(Position += sizeof(ushort))], value);
    }
}