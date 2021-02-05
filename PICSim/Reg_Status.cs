namespace PICSim
{
    /// <summary>
    /// Status register
    /// </summary>
    public class Reg_Status
    {
        public Reg_Generic baseRegister = new Reg_Generic();
        public bool IRP { get => baseRegister.Bit7; set { baseRegister.Bit7 = value; } }
        public bool RP1 { get => baseRegister.Bit6; set { baseRegister.Bit6 = value; } }
        public bool RP0 { get => baseRegister.Bit5; set { baseRegister.Bit5 = value; } }
        public bool nTO { get => baseRegister.Bit4; set { baseRegister.Bit4 = value; } }
        public bool nPD { get => baseRegister.Bit3; set { baseRegister.Bit3 = value; } }
        public bool Z { get => baseRegister.Bit2; set { baseRegister.Bit2 = value; } }
        public bool DC { get => baseRegister.Bit1; set { baseRegister.Bit1 = value; } }
        public bool C { get => baseRegister.Bit0; set { baseRegister.Bit0 = value; } }

        public byte Register { get => baseRegister.Register; set => baseRegister.Register = value; }

        public Reg_Status(Reg_Generic register)
        {
            baseRegister = register;
        }

        public new string ToString()
        {
            return $"IRP: {IRP}, RP1 {RP1}, RP0: {RP0}, nTO: {nTO}, nPD: {nPD}, Z: {Z}, DC: {DC}, C: {C}";
        }
    }
}


