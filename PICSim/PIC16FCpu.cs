using System;

using IntelHexFormatReader;

namespace PICSim
{
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
                new PIC16FInstruction("CLRF",   0x0180, 0xff80) { InstructionAction = PIC16FCpu.CLRF_Action },
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
                new PIC16FInstruction("BSF",    0x1400, 0xfc00)     { InstructionAction = PIC16FCpu.BSF_Action },
                new PIC16FInstruction("BTFSC",  0x1800, 0xfc00)     { InstructionAction = PIC16FCpu.BTFSC_Action },
                new PIC16FInstruction("BTFSS",  0x1c00, 0xfc00)     { InstructionAction = PIC16FCpu.BTFSS_Action },

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
        public Reg_Generic WRegister;

        public Reg_Generic[] RAMMemoryAndRegisters;   // Ram is 8 bit wide
        public UInt16[] ProgramMemory;        // Program memory is 14 bit wide so we use a 16 bit integer


        private UInt16 GetBankedAddress(PIC16FInstruction instruction)
        {
            return (UInt16)(instruction.DestinationAddress | ((StatusRegister.Register & 0x60) << 2));
        }

        #region "CPU Instruction functions"
        public static void ADDWF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;
            UInt16 result = (UInt16)(cpu.WRegister.Register + registerValue);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = (byte)result;
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
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;
            UInt16 result = (UInt16)(cpu.WRegister.Register & registerValue);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = (byte)result;
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
            byte reg = cpu.RAMMemoryAndRegisters[address].Register;
            UInt16 result = (UInt16)(reg & ~(0x01 << instruction.AffectedBit));
            cpu.RAMMemoryAndRegisters[address].Register = (byte)result;
            cpu.IncrementPC();
        }

        public static void BSF_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte reg = cpu.RAMMemoryAndRegisters[address].Register;
            UInt16 result = (UInt16)(reg | (0x01 << instruction.AffectedBit));
            cpu.RAMMemoryAndRegisters[address].Register = (byte)result;
            cpu.IncrementPC();
        }

        public static void BTFSS_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte reg = cpu.RAMMemoryAndRegisters[address].Register;
            if ((reg & (0x01 << instruction.AffectedBit)) != 0) // Do 2 increment of PC if the bit is set
            {
                cpu.IncrementPC();
            }
            cpu.IncrementPC();
        }

        public static void BTFSC_Action(PIC16FInstruction instruction, PIC16FCpu cpu)
        {
            int address = cpu.GetBankedAddress(instruction);
            byte reg = cpu.RAMMemoryAndRegisters[address].Register;
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
            cpu.RAMMemoryAndRegisters[address].Register = 0;
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
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;
            byte result = (byte)~registerValue;

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = result;
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
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;
            byte result = (byte)(registerValue - 1);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = result;
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
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;
            byte result = (byte)(registerValue + 1);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = result;
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
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;
            UInt16 result = (UInt16)(cpu.WRegister.Register | registerValue);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = (byte)result;
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
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = registerValue;
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
            cpu.RAMMemoryAndRegisters[address].Register = cpu.WRegister.Register;
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
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;
            UInt16 result = (UInt16)(registerValue << 1);
            result |= cpu.StatusRegister.C ? (UInt16)0x01 : (UInt16)0x00;

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = (byte)result;
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
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;
            UInt16 result = (UInt16)(registerValue >> 1);
            result |= cpu.StatusRegister.C ? (UInt16)0x80 : (UInt16)0x00;

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = (byte)result;
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
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;
            UInt16 result = (UInt16)(registerValue - cpu.WRegister.Register);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = (byte)result;
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
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;
            byte highNibble = (byte)(registerValue >> 4);
            byte lowNibble = (byte)(registerValue & 0x0f);
            UInt16 result = (UInt16)(lowNibble << 4 | highNibble);
            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = (byte)result;
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
            byte registerValue = cpu.RAMMemoryAndRegisters[address].Register;
            UInt16 result = (UInt16)(registerValue ^ cpu.WRegister.Register);

            if (instruction.Destination == OpcodeDestinations.FileRegister)
            {
                cpu.RAMMemoryAndRegisters[address].Register = (byte)result;
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


        public PIC16FCpu(UInt16 RAMMemorySize, UInt16 ProgramMemorySize)
        {
            Random rnd = new Random();
            WRegister = new Reg_Generic();
            RAMMemoryAndRegisters = new Reg_Generic[RAMMemorySize];
            ProgramMemory = new UInt16[ProgramMemorySize];
            for (int i = 0; i < RAMMemorySize; i++) 
                RAMMemoryAndRegisters[i] = new Reg_Generic() { Register = (byte)rnd.Next(0, 255) };
            StatusRegister = new Reg_Status(RAMMemoryAndRegisters[3]);
            StatusRegister.Register &= 0x18;
            RAMMemoryAndRegisters[0x0a].Register = 0;
            RAMMemoryAndRegisters[0x0b].Register &= 0x01;

            // Mirroring register from bank 0 to bank 1
            RAMMemoryAndRegisters[0x80] = RAMMemoryAndRegisters[0x00];
            RAMMemoryAndRegisters[0x82] = RAMMemoryAndRegisters[0x02];
            RAMMemoryAndRegisters[0x83] = RAMMemoryAndRegisters[0x03];
            RAMMemoryAndRegisters[0x84] = RAMMemoryAndRegisters[0x04];
            RAMMemoryAndRegisters[0x8a] = RAMMemoryAndRegisters[0x0a];
            RAMMemoryAndRegisters[0x8b] = RAMMemoryAndRegisters[0x0b];

            //RAMMemoryAndRegisters[0x0a].Register = 0;
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
                UInt16 value = (UInt16)((mem.Cells[hexIndex].Value) | mem.Cells[hexIndex + 1].Value << 8);
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
}


