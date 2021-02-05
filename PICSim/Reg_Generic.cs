using System;

namespace PICSim
{
    public class Reg_Generic
    {
        public bool Bit0 { get => 0x00 != (Register & 0x01); set { Register = (value) ? (byte)(Register | (0x01 << 0)) : (byte)(Register & ~(0x01 << 0)); } }
        public bool Bit1 { get => 0x00 != (Register & 0x02); set { Register = (value) ? (byte)(Register | (0x01 << 1)) : (byte)(Register & ~(0x01 << 1)); } }
        public bool Bit2 { get => 0x00 != (Register & 0x04); set { Register = (value) ? (byte)(Register | (0x01 << 2)) : (byte)(Register & ~(0x01 << 2)); } }
        public bool Bit3 { get => 0x00 != (Register & 0x08); set { Register = (value) ? (byte)(Register | (0x01 << 3)) : (byte)(Register & ~(0x01 << 3)); } }
        public bool Bit4 { get => 0x00 != (Register & 0x10); set { Register = (value) ? (byte)(Register | (0x01 << 4)) : (byte)(Register & ~(0x01 << 4)); } }
        public bool Bit5 { get => 0x00 != (Register & 0x20); set { Register = (value) ? (byte)(Register | (0x01 << 5)) : (byte)(Register & ~(0x01 << 5)); } }
        public bool Bit6 { get => 0x00 != (Register & 0x40); set { Register = (value) ? (byte)(Register | (0x01 << 6)) : (byte)(Register & ~(0x01 << 6)); } }
        public bool Bit7 { get => 0x00 != (Register & 0x80); set { Register = (value) ? (byte)(Register | (0x01 << 7)) : (byte)(Register & ~(0x01 << 7)); } }

        public byte Register { get; set; }

        public new string ToString()
        {
            return Convert.ToString(Register, 2);
        }
    }
}


