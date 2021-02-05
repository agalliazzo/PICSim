namespace PICSim
{
    /// <summary>
    /// Option register
    /// </summary>
    public class Reg_Option : Reg_Generic
    {
        public bool RBPU { get => Bit7; set { Bit7 = value; } }
        public bool INTEDG { get => Bit6; set { Bit6 = value; } }
        public bool T0CS { get => Bit5; set { Bit5 = value; } }
        public bool T0SE { get => Bit4; set { Bit4 = value; } }
        public bool PSA { get => Bit3; set { Bit3 = value; } }
        public bool PS2 { get => Bit2; set { Bit2 = value; } }
        public bool PS1 { get => Bit1; set { Bit1 = value; } }
        public bool PS0 { get => Bit0; set { Bit0 = value; } }


    }
}


