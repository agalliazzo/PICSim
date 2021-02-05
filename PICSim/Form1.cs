using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using IntelHexFormatReader;

namespace PICSim
{
    public partial class Form1 : Form
    {
        PIC16FCpu _cpu;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Intel HEX File (*.hex) | *.hex",
                Multiselect = false,
                FileName = Properties.Settings.Default.LastOpenedFile
            };
            DialogResult result = dialog.ShowDialog();
            if (result != DialogResult.OK)
                return;
            _cpu = new PIC16FCpu(256, 1024);
            _cpu.FillProgramMemory(dialog.FileName);

            foreach(UInt16 cell in _cpu.ProgramMemory)
            {
                lstProgramMemory.Items.Add(cell.ToString("X"));
            }


            Properties.Settings.Default.LastOpenedFile = dialog.FileName;
            Properties.Settings.Default.Save();
        }

        private void btnStartProgram_Click(object sender, EventArgs e)
        {
            //while (true)
            //{
                _cpu.ExecuteInstruction();
                lblStatusReg.Text = "Status Reg: " + _cpu.StatusRegister.ToString();
                lblProgramCounter.Text = $"PC: {_cpu.ProgramCounter.ToString("X")} ";
            lblMem.Text = _cpu.RAMMemoryAndRegisters[0x0c].ToString("X");
            //}
        }
    }

    /// <summary>
    /// Class that define the 16F CPU
    /// </summary>
    public class PIC16FCpu
    {
        public UInt16 ProgramCounter = 0;

        // The CPU stack (only 8 levels)
        CPUStack Stack = new CPUStack();

        // A place to store all the instruction object
        PIC16FInstructionCollection Instructions = new PIC16FInstructionCollection()
        {
            //Byte oriented file register operations
                new PIC16FInstruction("ADDWF",  0x0700)         { InstructionAction = PIC16FCpu.ADDWF_Action},
                new PIC16FInstruction("ANDWF",  0x0500)         { InstructionAction = PIC16FCpu.ANDWF_Action},
                new PIC16FInstruction("CLRF",   0x0100, 0xff80) { InstructionAction = PIC16FCpu.CLRF_Action },
                new PIC16FInstruction("CLRW",   0x0100, 0xff80) { InstructionAction = PIC16FCpu.CLRWDT_Action },
                new PIC16FInstruction("COMF",   0x0900)         { InstructionAction = PIC16FCpu.COMF_Action },
                new PIC16FInstruction("DECF",   0x0300)         { InstructionAction = PIC16FCpu.DECF_Action },
                new PIC16FInstruction("DECFSZ", 0x0b00)         { InstructionAction = PIC16FCpu.DECFSZ_Action },
                new PIC16FInstruction("INCF",   0x0a00)         { InstructionAction = PIC16FCpu.INCF_Action },
                new PIC16FInstruction("INCFSZ", 0x0f00)         { InstructionAction = PIC16FCpu.INCFSZ_Action },
                new PIC16FInstruction("IORWF",  0x0400)         { InstructionAction = PIC16FCpu.IORWF_Action },
                new PIC16FInstruction("MOVF",   0x0800)         { InstructionAction = PIC16FCpu.MOVF_Action },
                new PIC16FInstruction("MOVWF",  0x0080, 0xff80) { InstructionAction = PIC16FCpu.MOVWF_Action },
                new PIC16FInstruction("NOP",    0x0000, 0xffff) { InstructionAction = PIC16FCpu.NOP_Action },
                new PIC16FInstruction("RLF",    0x0d00)         { InstructionAction = PIC16FCpu.RLF_Action },
                new PIC16FInstruction("RRF",    0x0c00)         { InstructionAction = PIC16FCpu.RRF_Action },
                new PIC16FInstruction("SUBWF",  0x0200)         { InstructionAction = PIC16FCpu.SUBWF_Action },
                new PIC16FInstruction("SWAPF",  0x0e00)         { InstructionAction = PIC16FCpu.SWAPF_Action },
                new PIC16FInstruction("XORWF",  0x0600)         { InstructionAction = PIC16FCpu.XORWF_Action },
                
                //Bit oriented file register operations
                new PIC16FInstruction("BCF",    0x1000, 0xfc00)     { InstructionAction = PIC16FCpu.BCF_Action },
                new PIC16FInstruction("BSF",    0x1100, 0xfc00)     { InstructionAction = PIC16FCpu.BSF_Action },
                new PIC16FInstruction("BTFSC",  0x1200, 0xfc00)     { InstructionAction = PIC16FCpu.BTFSC_Action },
                new PIC16FInstruction("BTFSS",  0x1300, 0xfc00)     { InstructionAction = PIC16FCpu.BTFSS_Action },

                //Literal and constant operations
                new PIC16FInstruction("ADDLW",  0x3e00, 0xfe00)     { InstructionAction = PIC16FCpu.ADDLW_Action},
                new PIC16FInstruction("ANDLW",  0x3900, 0xff00)     { InstructionAction = PIC16FCpu.ANDLW_Action},
                new PIC16FInstruction("CALL",   0x2000, 0xf800)     { InstructionAction = PIC16FCpu.CALL_Action },
                new PIC16FInstruction("CLRWDT", 0x0064, 0xffff)     { InstructionAction = PIC16FCpu.CLRWDT_Action },
                new PIC16FInstruction("GOTO",   0x2800, 0xf800)     { InstructionAction = PIC16FCpu.GOTO_Action },
                new PIC16FInstruction("IORLW",  0x3800, 0xff00)     { InstructionAction = PIC16FCpu.IORLW_Action },
                new PIC16FInstruction("MOVLW",  0x3000, 0xfc00)     { InstructionAction = PIC16FCpu.MOVLW_Action },
                new PIC16FInstruction("RETFIE", 0x0009, 0xffff)     { InstructionAction = PIC16FCpu.RETFIE_Action },
                new PIC16FInstruction("RETLW",  0x3100, 0xfc00)     { InstructionAction = PIC16FCpu.RETLW_Action },
                new PIC16FInstruction("RETURN", 0x0008, 0xffff)     { InstructionAction = PIC16FCpu.RETURN_Action },
                new PIC16FInstruction("SLEEP",  0x0063, 0xffff)     { InstructionAction = PIC16FCpu.SLEEP_Action },
                new PIC16FInstruction("SUBLW",  0x3c00, 0xff00)     { InstructionAction = PIC16FCpu.SUBLW_Action },
                new PIC16FInstruction("XORLW",  0x3a00, 0xff00)     { InstructionAction = PIC16FCpu.XORLW_Action },
        };

        public Reg_Status StatusRegister;
        public Reg_W WRegister;

        private UInt16 GetBankedAddress(PIC16FInstruction instruction)
        {
            return (UInt16)(instruction.DestinationAddress | ((StatusRegister.Register & 0x60) << 4));
        }

        #region "CPU Instruction functions"
        public static void ADDWF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];
            UInt16 result = (UInt16)(cpu.WRegister.Register + registerValue);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = (byte)result;
            }
            else
            {
                cpu.WRegister.Register = (byte)result;
            }
            cpu.StatusRegister.Z = result == 0;
            cpu.StatusRegister.C = (result > 0xff);
            cpu.StatusRegister.DC = (registerValue & 0x0f + cpu.WRegister.Register & 0x0f) > 0x0f;
            cpu.IncrementPC();
        }

        public static void ADDLW_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            byte value = instruction.Operand;
            UInt16 result = (UInt16)(cpu.WRegister.Register + value);
            cpu.WRegister.Register = (byte)result;
            cpu.StatusRegister.Z = result == 0;
            cpu.StatusRegister.C = (result > 0xff);
            cpu.StatusRegister.DC = (value & 0x0f + cpu.WRegister.Register & 0x0f) > 0x0f;
            cpu.IncrementPC();
        }

        public static void ANDLW_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            byte value = instruction.Operand;
            UInt16 result = (UInt16)(cpu.WRegister.Register & value);
            cpu.WRegister.Register = (byte)result;
            cpu.StatusRegister.Z = result == 0;
            cpu.IncrementPC();
        }

        public static void ANDWF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];
            UInt16 result = (UInt16)(cpu.WRegister.Register & registerValue);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = (byte)result;
            }
            else
            {
                cpu.WRegister.Register = (byte)result;
            }
            cpu.StatusRegister.Z = result == 0;
            cpu.IncrementPC();
        }

        public static void BCF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte reg = cpu.RAMMemoryAndRegisters[address];
            UInt16 result = (UInt16)(reg & ~(0x01 << instruction.AffectedBit));
            cpu.RAMMemoryAndRegisters[address] = (byte)result;
            cpu.IncrementPC();
        }

        public static void BSF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte reg = cpu.RAMMemoryAndRegisters[address];
            UInt16 result = (UInt16)(reg | (0x01 << instruction.AffectedBit));
            cpu.RAMMemoryAndRegisters[address] = (byte)result;
            cpu.IncrementPC();
        }

        public static void BTFSS_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte reg = cpu.RAMMemoryAndRegisters[address];
            if ((reg & (0x01 << instruction.AffectedBit)) != 0) // Do 2 increment of PC if the bit is set
            {
                cpu.IncrementPC();
            }
            cpu.IncrementPC();
        }

        public static void BTFSC_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte reg = cpu.RAMMemoryAndRegisters[address];
            if ((reg & (0x01 << instruction.AffectedBit)) == 0) // Do 2 increment of PC if the bit is clear
            {
                cpu.IncrementPC();
            }
            cpu.IncrementPC();
        }

        public static void CALL_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            cpu.Stack.Push((UInt16)(cpu.ProgramCounter + 1)); // Add the return address to the stack
            cpu.ProgramCounter = instruction.Operand;
        }

        public static void CLRF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            cpu.RAMMemoryAndRegisters[address] = 0;
            cpu.StatusRegister.Z = true;
            cpu.IncrementPC();
        }

        public static void CLRW_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            cpu.WRegister.Register = 0;
            cpu.StatusRegister.Z = true;
            cpu.IncrementPC();
        }
        public static void CLRWDT_Action(PIC16FInstruction instruction, PIC16FCpu cpu) { cpu.IncrementPC(); } // Nothing to do here until WDT is implemented

        public static void COMF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];
            byte result = (byte)~registerValue;

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = result;
            }
            else
            {
                cpu.WRegister.Register = result;
            }
            cpu.StatusRegister.Z = result == 0;
            cpu.IncrementPC();
        }
        public static void DECF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];
            byte result = (byte)(registerValue - 1);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = result;
            }
            else
            {
                cpu.WRegister.Register = result;
            }
            cpu.StatusRegister.Z = result == 0;
            cpu.IncrementPC();
        }

        public static void DECFSZ_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            DECF_Action(instruction, cpu);
            if (cpu.StatusRegister.Z)
                cpu.IncrementPC();
            cpu.IncrementPC();
        }

        public static void GOTO_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            cpu.ProgramCounter = instruction.Operand;
        }

        public static void INCF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];
            byte result = (byte)(registerValue + 1);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = result;
            }
            else
            {
                cpu.WRegister.Register = result;
            }
            cpu.StatusRegister.Z = result == 0;
            cpu.IncrementPC();
        }

        public static void INCFSZ_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            INCF_Action(instruction, cpu);
            if (cpu.StatusRegister.Z)
                cpu.IncrementPC();
            cpu.IncrementPC();
        }

        public static void IORLW_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            byte value = instruction.Operand;
            UInt16 result = (UInt16)(cpu.WRegister.Register | value);
            cpu.WRegister.Register = (byte)result;
            cpu.StatusRegister.Z = result == 0;
            cpu.IncrementPC();
        }

        public static void IORWF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];
            UInt16 result = (UInt16)(cpu.WRegister.Register | registerValue);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = (byte)result;
            }
            else
            {
                cpu.WRegister.Register = (byte)result;
            }
            cpu.StatusRegister.Z = result == 0;
            cpu.IncrementPC();
        }

        public static void MOVF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = registerValue;
            }
            else
            {
                cpu.WRegister.Register = registerValue;
            }
            cpu.StatusRegister.Z = registerValue == 0;
            cpu.IncrementPC();
        }

        public static void MOVLW_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            cpu.WRegister.Register = instruction.Operand;
            cpu.IncrementPC();
        }

        public static void MOVWF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            cpu.RAMMemoryAndRegisters[address] = cpu.WRegister.Register;
            cpu.IncrementPC();
        }

        public static void NOP_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            cpu.IncrementPC();
        }
        public static void RETFIE_Action(PIC16FInstruction instruction, PIC16FCpu cpu) // TODO: Implement RETFIE
        {
            NOP_Action(instruction, cpu);
        }
        public static void RETLW_Action(PIC16FInstruction instruction, PIC16FCpu cpu) 
        {
            cpu.WRegister.Register = instruction.Operand;
            cpu.ProgramCounter = cpu.Stack.Pop();
        }
        public static void RETURN_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            cpu.ProgramCounter = cpu.Stack.Pop();
        }

        public static void RLF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];
            UInt16 result = (UInt16)(registerValue << 1);
            result |= cpu.StatusRegister.C ? (UInt16)0x01 : (UInt16)0x00;

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = (byte)result;
            }
            else
            {
                cpu.WRegister.Register = (byte)result;
            }
            cpu.StatusRegister.C = (registerValue & 0x80) != 0;
            cpu.IncrementPC();
        }

        public static void RRF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];
            UInt16 result = (UInt16)(registerValue >> 1);
            result |= cpu.StatusRegister.C ? (UInt16)0x80 : (UInt16)0x00;

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = (byte)result;
            }
            else
            {
                cpu.WRegister.Register = (byte)result;
            }
            cpu.StatusRegister.C = (registerValue & 0x01) != 0;
            cpu.IncrementPC();
        }

        public static void SLEEP_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            // TODO: Manage sleep in some way
            NOP_Action(instruction, cpu);
        }

        public static void SUBWF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];
            UInt16 result = (UInt16)(registerValue - cpu.WRegister.Register );

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = (byte)result;
            }
            else
            {
                cpu.WRegister.Register = (byte)result;
            }
            cpu.StatusRegister.Z = result == 0;
            cpu.StatusRegister.C = (result > 0xff);
            cpu.StatusRegister.DC = (registerValue & 0x0f + cpu.WRegister.Register & 0x0f) > 0x0f;
            cpu.IncrementPC();
        }

        public static void SUBLW_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            byte value = instruction.Operand;
            UInt16 result = (UInt16)(value - cpu.WRegister.Register);
            cpu.WRegister.Register = (byte)result;
            cpu.StatusRegister.Z = result == 0;
            cpu.StatusRegister.C = (result > 0xff);
            cpu.StatusRegister.DC = (value & 0x0f + cpu.WRegister.Register & 0x0f) > 0x0f;
            cpu.IncrementPC();
        }

        public static void SWAPF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];
            byte highNibble = (byte)(registerValue >> 4);
            byte lowNibble = (byte)(registerValue & 0x0f);
            UInt16 result = (UInt16)(lowNibble << 4 | highNibble);
            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = (byte)result;
            }
            else
            {
                cpu.WRegister.Register = (byte)result;
            }
            cpu.IncrementPC();
        }

        public static void XORWF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address];
            UInt16 result = (UInt16)(registerValue ^ cpu.WRegister.Register);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address] = (byte)result;
            }
            else
            {
                cpu.WRegister.Register = (byte)result;
            }
            cpu.StatusRegister.Z = result == 0;
            cpu.IncrementPC();
        }

        public static void XORLW_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            byte value = instruction.Operand;
            UInt16 result = (UInt16)(value ^ cpu.WRegister.Register);
            cpu.WRegister.Register = (byte)result;
            cpu.StatusRegister.Z = result == 0;
            cpu.IncrementPC();
        }

        #endregion

        public byte[] RAMMemoryAndRegisters;   // Ram is 8 bit wide
        public UInt16[] ProgramMemory;        // Program memory is 14 bit wide so we use a 16 bit integer

        public PIC16FCpu(UInt16 RAMMemorySize, UInt16 ProgramMemorySize)
        {
            RAMMemoryAndRegisters = new byte[RAMMemorySize];
            ProgramMemory = new UInt16[ProgramMemorySize];
        }

        public void IncrementPC(UInt16 count = 1)
        {
            ProgramCounter += count;
            ProgramCounter = (UInt16)(ProgramCounter % ProgramMemory.Length);
        }

        public void JumpTo(UInt16 destinationAddress)
        {
            ProgramCounter = destinationAddress;
        }

        public void FillProgramMemory(string filename)
        {
            HexFileReader reader = new HexFileReader(filename, ProgramMemory.Length * 2);
            var mem = reader.Parse();
            for (int hexIndex = 0, memIndex = 0; hexIndex < ProgramMemory.Length * 2; hexIndex += 2, memIndex++)
            {
                UInt16 value = (UInt16)((mem.Cells[hexIndex].Value ) | mem.Cells[hexIndex + 1].Value << 8);
                ProgramMemory[memIndex] = value;
            }
        }

        public void ExecuteInstruction()
        {
            PIC16FInstruction instruction = Instructions.ParseMemoryCell(ProgramMemory[ProgramCounter]);

            if (instruction is null)
                return;
            instruction.InstructionAction.Invoke(instruction, this);
        }
    }

    /// <summary>
    /// Status register
    /// </summary>
    public struct Reg_Status
    {
        public bool IRP;
        public bool RP1;
        public bool RP0;
        public bool nTO;
        public bool nPD;
        public bool Z;
        public bool DC;
        public bool C;

        public byte Register
        {
            get
            {
                byte ret = 0;
                // TODO: Check if it is faster to do a series of if or a simply math operation
                ret |= IRP ? (byte)0x80 : (byte)0x00;
                ret |= RP1 ? (byte)0x40 : (byte)0x00;
                ret |= RP0 ? (byte)0x20 : (byte)0x00;
                ret |= nTO ? (byte)0x10 : (byte)0x00;
                ret |= nPD ? (byte)0x08 : (byte)0x00;
                ret |= Z ? (byte)0x04 : (byte)0x00;
                ret |= DC ? (byte)0x02 : (byte)0x00;
                ret |= C ? (byte)0x01 : (byte)0x00;
                return ret;
            }
            set
            {
                IRP = (value & 0x80) != 0;
                RP1 = (value & 0x40) != 0;
                RP0 = (value & 0x20) != 0;
                nTO = (value & 0x10) != 0;
                nPD = (value & 0x08) != 0;
                Z = (value & 0x04) != 0;
                DC = (value & 0x02) != 0;
                C = (value & 0x01) != 0;
            }
        }

        public new string ToString()
        {
            return $"IRP: {IRP}, RP1 {RP1}, RP0: {RP0}, nTO: {nTO}, nPD: {nPD}, Z: {Z}, DC: {DC}, C: {C}";
        }
    }

    /// <summary>
    /// W Register
    /// </summary>
    public struct Reg_W
    {
        public byte Register { get; set; }
    }


    /// <summary>
    /// Option register
    /// </summary>
    public struct Reg_Option
    {
        public bool RBPU;
        public bool INTEDG;
        public bool T0CS;
        public bool T0SE;
        public bool PSA;
        public bool PS2;
        public bool PS1;
        public bool PS0;

        public byte Register
        {
            get
            {
                byte ret = 0;
                // TODO: Check if it is faster to do a series of if or a simply math operation
                ret |= RBPU ? (byte)0x80 : (byte)0x00;
                ret |= INTEDG ? (byte)0x40 : (byte)0x00;
                ret |= T0CS ? (byte)0x20 : (byte)0x00;
                ret |= T0SE ? (byte)0x10 : (byte)0x00;
                ret |= PSA ? (byte)0x08 : (byte)0x00;
                ret |= PS2 ? (byte)0x04 : (byte)0x00;
                ret |= PS1 ? (byte)0x02 : (byte)0x00;
                ret |= PS0 ? (byte)0x01 : (byte)0x00;
                return ret;
            }
            set
            {
                RBPU = (value & 0x80) != 0;
                INTEDG = (value & 0x40) != 0;
                T0CS = (value & 0x20) != 0;
                T0SE = (value & 0x10) != 0;
                PSA = (value & 0x08) != 0;
                PS2 = (value & 0x04) != 0;
                PS1 = (value & 0x02) != 0;
                PS0 = (value & 0x01) != 0;
            }
        }
    }

    /// <summary>
    /// Enum to better manage the opcode destination
    /// </summary>
    public enum OpcodeDestinations : byte
    {
        FileRegister = 0,
        WRegister = 1,
        Unspecified
    }

    /// <summary>
    /// Enum to better manage the Opcode Type
    /// </summary>
    public enum OpcodeType : byte
    {
        FileRegisterOperation = 0,
        BitOperation = 1,
        LiteralAndCostantOperation = 2
    }

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

        public UInt16 AffectedBit { get => (UInt16)((Operand >> 6) & 0x07); }
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

    public class PIC16FInstructionCollection : List<PIC16FInstruction>
    {
        public PIC16FInstruction ParseMemoryCell(UInt16 memoryCellValue)
        {
            foreach (PIC16FInstruction instruction in this)
            {
                if ((memoryCellValue & instruction.OPCodeMask) == instruction.Operation)
                {
                    instruction.OPCode = memoryCellValue;
                    return instruction;
                }
            }

            return null;
        }
    }

    /// <summary>
    ///  Define a limited size stack as in the 16F CPU
    /// </summary>
    public class CPUStack : Stack<UInt16>
    {
        UInt16 _stackDepth;
        public CPUStack(UInt16 stackDepth = 8) : base()
        {
            _stackDepth = stackDepth;
        }

        public void Push(UInt16 address)
        {
            base.Push(address);
            if (this.Count > _stackDepth)
                throw new StackOverflowException();
        }

    }
}


