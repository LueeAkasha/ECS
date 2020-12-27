using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class is used to implement the ALU
    //The specification can be found at https://b1391bd6-da3d-477d-8c01-38cdf774495a.filesusr.com/ugd/56440f_2e6113c60ec34ed0bc2035c9d1313066.pdf slides 48,49
    class ALU : Gate
    {
        //The word size = number of bit in the input and output
        public int Size { get; private set; }

        //Input and output n bit numbers
        public WireSet InputX { get; private set; }
        public WireSet InputY { get; private set; }
        public WireSet Output { get; private set; }

        //Control bit 
        public Wire ZeroX { get; private set; }
        public Wire ZeroY { get; private set; }
        public Wire NotX { get; private set; }
        public Wire NotY { get; private set; }
        public Wire F { get; private set; }
        public Wire NotOutput { get; private set; }

        //Bit outputs
        public Wire Zero { get; private set; }
        public Wire Negative { get; private set; }


        //your code here
        BitwiseMux Zx;
        BitwiseMux Zy;
        BitwiseMux nX;
        BitwiseMux nY;
        BitwiseMux f;
        BitwiseMux nO;
        BitwiseNotGate notX;
        BitwiseNotGate notY;
        BitwiseNotGate notOutput;
        BitwiseAndGate and;
        FullAdder fullAdder;

        public ALU(int iSize)
        {
            Size = iSize;
            InputX = new WireSet(Size);
            InputY = new WireSet(Size);
            ZeroX = new Wire();
            ZeroY = new Wire();
            NotX = new Wire();
            NotY = new Wire();
            F = new Wire();
            NotOutput = new Wire();
            Negative = new Wire();            
            Zero = new Wire();


            //Create and connect all the internal components

            //init the muxes
            Zx = new BitwiseMux(iSize);
            Zy = new BitwiseMux(iSize);
            nX = new BitwiseMux(iSize);
            nY = new BitwiseMux(iSize);
            f = new BitwiseMux(iSize);
            nO = new BitwiseMux(iSize);
            notX = new BitwiseNotGate(iSize);
            notY = new BitwiseNotGate(iSize);
            notOutput = new BitwiseNotGate(iSize);
            and = new BitwiseAndGate(iSize);
            fullAdder = new FullAdder();


            WireSet zeros = new WireSet(iSize); 
            for(int i = 0; i<zeros.Size; i++)
            {
                zeros[i].ConnectInput(Zero);
            }
            Zx.ConnectInput1(InputX);
            Zx.ConnectInput2(zeros);
            Zx.ConnectControl(ZeroX);

            /*for (int i = 0; i < zeros.Size; i++)
            {
                zeros[i].ConnectInput(Zero);
            }*/
            Zy.ConnectInput1(InputY);
            Zy.ConnectInput2(zeros);
            Zy.ConnectControl(ZeroY);

            nX.ConnectInput1(Zx.Output);
            notX.ConnectInput(Zx.Output);
            nX.ConnectInput2(notX.Output);
            nX.ConnectControl(NotX);

            nY.ConnectInput1(Zy.Output);
            notY.ConnectInput(Zy.Output);
            nY.ConnectInput2(notY.Output);
            nY.ConnectControl(NotY);

            and.ConnectInput1(nX.Output);
            and.ConnectInput2(nY.Output);

            WireSet addResult = new WireSet(iSize);

            fullAdder.ConnectInput1(nX.Output[0]);
            fullAdder.ConnectInput2(nY.Output[0]);
            Wire Co = fullAdder.CarryOutput;
            addResult[0].ConnectInput(fullAdder.Output);
            for(int i=1; i<iSize; i++)
            {
                fullAdder = new FullAdder();
                fullAdder.ConnectInput1(nX.Output[i]);
                fullAdder.ConnectInput2(nY.Output[i]);
                fullAdder.CarryInput.ConnectInput(Co);
                Co = new Wire();
                Co.ConnectInput(fullAdder.CarryOutput);
                addResult[i].ConnectInput(fullAdder.Output);
            }

            f.ConnectInput1(and.Output);
            f.ConnectInput2(addResult);
            f.ConnectControl(F);

            nO.ConnectInput1(f.Output);
            notOutput.ConnectInput(f.Output);
            nO.ConnectInput2(notOutput.Output);
            nO.ConnectControl(NotOutput);

            Output = nO.Output;
        }

        public override bool TestGate()
        {
            throw new NotImplementedException();
        }
    }
}
