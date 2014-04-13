using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocketCore
{
    class SocketMessagePackage
    {
        public byte[] Buffer { get; private set; }
        public DateTime Timestamp { get; private set; }

        public SocketMessagePackage(DateTime timestamp, byte[] buffer, int size)
        {
            Timestamp = timestamp;
            Buffer = new byte[size];
            System.Buffer.BlockCopy(buffer, 0, Buffer, 0, size);
        }
    }
}