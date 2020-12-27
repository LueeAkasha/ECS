using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This class implements a FullAdder, taking as input 3 bits - 2 numbers and a carry, and computing the result in the output, and the carry out.
    class FullAdder : TwoInputGate
    {
        public Wire CarryInput { get; private set; }
        public Wire CarryOutput { get; private set; }

        HalfAdder half1;
        HalfAdder half2;
        OrGate or;

        public FullAdder()
        {
            CarryInput = new Wire();
            CarryOutput = new Wire();
            half1 = new HalfAdder();
            half2 = new HalfAdder();
            or = new OrGate();

            half1.ConnectInput1(Input1);
            half1.ConnectInput2(Input2);


            half2.ConnectInput1(CarryInput);
            half2.ConnectInput2(half1.Output);

            or.ConnectInput1(half1.CarryOutput);
            or.ConnectInput2(half2.CarryOutput);

            CarryOutput.ConnectInput(or.Output);
            Output.ConnectInput(half2.Output);
        }


        public override string ToString()
        {
            return Input1.Value + "+" + Input2.Value + " (C" + CarryInput.Value + ") = " + Output.Value + " (C" + CarryOutput.Value + ")";
        }

        public override bool TestGate()
        {
            CarryInput.Value = 0;

            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0 || CarryOutput.Value != 0)
                return false;

            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1 || CarryOutput.Value != 0)
                return false;

            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1 || CarryOutput.Value != 0)
                return false;

            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 0 || CarryOutput.Value != 1)
                return false;

            /************************************************/

            CarryInput.Value = 1;

            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 1 || CarryOutput.Value != 0)
                return false;

            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 0 || CarryOutput.Value != 1)
                return false;

            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 0 || CarryOutput.Value != 1)
                return false;

            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 1 || CarryOutput.Value != 1)
                return false;

            /************************************************/

            return true;
        }
    }
}
