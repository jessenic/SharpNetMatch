using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpNetMatch
{
    public class Packet
    {
        public byte[] memBlock { get; internal set; }
        int offset;
        public int Length { get { return this.memBlock.Length; } }

        public int BytesLeft
        {
            get
            {
                return this.memBlock.Length - this.offset;
            }
        }

        public Packet(int input = 0)
        {
            System.Diagnostics.Debug.WriteLine(BitConverter.IsLittleEndian);
            this.memBlock = new byte[input + 4];
            this.offset = 4;
        }

        public void Resize(int size)
        {
            if (this.offset + size > this.memBlock.Length)
            {
                var newBuf = new byte[this.offset + size];
                Array.Copy(memBlock, newBuf, memBlock.Length);
                this.memBlock = newBuf;
            }
        }

        public int GetSize()
        {
            return (this.memBlock.Length - 4) < 0 ? 0 : this.memBlock.Length - 4;
        }
        public byte GetByte()
        {
            return this.memBlock[this.offset++];
        }

        public short GetShort()
        {
            this.offset += 2;
            return BitConverter.ToInt16(this.memBlock, this.offset - 2);
        }

        public ushort GetUShort()
        {
            this.offset += 2;
            return BitConverter.ToUInt16(this.memBlock, this.offset - 2);
        }

        public int GetInt()
        {
            this.offset += 4;
            return BitConverter.ToInt32(this.memBlock, this.offset - 4);
        }

        public float GetFloat()
        {
            this.offset += 4;
            return BitConverter.ToSingle(this.memBlock, this.offset - 4);
        }

        public string GetString()
        {
            var len = this.GetInt();
            this.offset += len;
            return ASCIIEncoding.ASCII.GetString(this.memBlock, this.offset - len, len);
        }

        public int ClientId
        {
            get
            {
                return BitConverter.ToInt32(this.memBlock, 0);
            }
            set
            {
                BitConverter.GetBytes(value).CopyTo(this.memBlock, 0);
            }
        }

        public void PutByte(byte value)
        {
            this.Resize(1); // Resize memBlock if needed
            this.memBlock[this.offset++] = value;
        }

        public void PutByte(PacketType value)
        {
            this.PutByte((byte)value);
        }

        public void PutShort(short value)
        {
            this.Resize(2); // Resize memBlock if needed
            BitConverter.GetBytes(value).CopyTo(this.memBlock, this.offset);
            this.offset += 2;
        }

        public void PutUShort(ushort value)
        {
            this.Resize(2); // Resize memBlock if needed
            BitConverter.GetBytes(value).CopyTo(this.memBlock, this.offset);
            this.offset += 2;
        }

        public void PutInt(int value)
        {
            this.Resize(4); // Resize memBlock if needed
            BitConverter.GetBytes(value).CopyTo(this.memBlock, this.offset);
            this.offset += 4;
        }

        public void PutFloat(float value)
        {
            this.Resize(4); // Resize memBlock if needed
            BitConverter.GetBytes(value).CopyTo(this.memBlock, this.offset);
            this.offset += 4;
        }

        public void PutString(string value)
        {
            var len = value.Length;
            this.PutInt(len);
            this.Resize(len); // Resize memBlock if needed
            ASCIIEncoding.ASCII.GetBytes(value).CopyTo(this.memBlock, this.offset);
            this.offset += len;
        }
    }
}
