using System;

namespace PICSim
{
    /// <summary>
    ///  Class to define an instruction
    /// </summary>
    public class PIC16FInstruction
    {
        public string Literal { get; set; }
        public UInt16 OPCode { get; set; }
        public UInt16 Operation { get => (UInt16)(OPCode & OPCodeMask); }
        public byte Operand { get => (byte)(OPCode & ~OPCodeMask); }
        public OpcodeDestinations Destination
        {
            get
            {
                return (OPCode & 0x0080) == 0 ? OpcodeDestinations.WRegister : OpcodeDestinations.FileRegister;
            }
        }

        public UInt16 AffectedBit { get => (UInt16)((OPCode >> 7) & 0x07); }
        public UInt16 DestinationAddress { get => (UInt16)(Operand & 0x7f); }

        public OpcodeType OpcodeType { get => (OpcodeType)((OPCode & 0x3000) >> 12); }
        public UInt16 OPCodeMask { get; set; } = (UInt16)0xff80;
        public Action<PIC16FInstruction, PIC16FCpu> InstructionAction { get; set; }

        public PIC16FInstruction(string literal, UInt16 oPCode, UInt16 opCodeMask = 0xff00)
        {
            OPCode = oPCode;
            Literal = literal;
            OPCodeMask = opCodeMask;
        }

        public new string ToString()
        {
            return $"{Literal} - {OpcodeType}";
        }
    }
}


