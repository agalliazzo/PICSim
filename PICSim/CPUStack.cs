using System;
using System.Collections.Generic;

namespace PICSim
{
    /// <summary>
    ///  Define a limited size stack as in the 16F CPU
    /// </summary>
    public class CPUStack : Stack<ushort>
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


