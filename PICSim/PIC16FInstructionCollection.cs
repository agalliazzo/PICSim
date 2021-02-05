using System;
using System.Collections.Generic;

namespace PICSim
{
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
}


