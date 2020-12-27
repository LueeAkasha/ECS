using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements an adder, receving as input two n bit numbers, and outputing the sum of the two numbers
    class MultiBitAdder : Gate
    {
        //Word size - number of bits in each input
        public int Size { get; private set; }

        public WireSet Input1 { get; private set; }
        public WireSet Input2 { get; private set; }
        public WireSet Output { get; private set; }
        //An overflow bit for the summation computation
        public Wire Overflow { get; private set; }

        FullAdder full1;

        public MultiBitAdder(int iSize)
        {
            Size = iSize;
            Input1 = new WireSet(Size);
            Input2 = new WireSet(Size);
            Output = new WireSet(Size);
            Overflow = new Wire();

            full1 = new FullAdder();

            full1.ConnectInput1(Input1[0]);
            full1.ConnectInput2(Input2[0]);
            Output[0].ConnectInput(full1.Output);

            for(int i = 1; i<Input1.Size; i++)
            {
                Wire temp_S = full1.Output;
                Wire temp_C = full1.CarryOutput;

                full1 = new FullAdder();

                full1.CarryInput.ConnectInput(temp_C);
                full1.ConnectInput1(Input1[i]);
                full1.ConnectInput2(Input2[i]);
                Output[i].ConnectInput(full1.Output);
            }


            Overflow.ConnectInput(full1.CarryOutput); // updating the overflow.
        }

        public override string ToString()
        {
            return Input1 + "(" + Input1.Get2sComplement() + ")" + " + " + Input2 + "(" + Input2.Get2sComplement() + ")" + " = " + Output + "(" + Output.Get2sComplement() + ")";
        }

        public void ConnectInput1(WireSet wInput)
        {
            Input1.ConnectInput(wInput);
        }
        public void ConnectInput2(WireSet wInput)
        {
            Input2.ConnectInput(wInput);
        }


        public override bool TestGate()
        {
            
            Input1.SetValue(7);
            Input2.SetValue(7);

            if (Output.Get2sComplement() != 14)
                return false;

            Input1.Set2sComplement(6);
            Input2.Set2sComplement(-6);


            if (Output.Get2sComplement()!= 0)
            {
                return false;
            }

            Input1.Set2sComplement(6);
            Input2.Set2sComplement(-1);


            if (Output.Get2sComplement() != 5)
            {
                return false;
            }

            return true;
        }
    }
}
