using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This gate implements the xor operation. To implement it, follow the example in the And gate.
    class XorGate : TwoInputGate
    {
        private NAndGate NAnd;
        private OrGate or;
        private AndGate and;

        public XorGate()
        {
           
            or = new OrGate();
            NAnd = new NAndGate();
            and = new AndGate();

            
            
            Input1 = or.Input1;
            Input2 = or.Input2;

            NAnd.ConnectInput1(Input1);
            NAnd.ConnectInput2(Input2);
            and.ConnectInput1(or.Output);
            and.ConnectInput2(NAnd.Output);
            Output = and.Output;

            
            
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(xor)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Xor " + Input1.Value + "," + Input2.Value + " -> " + Output.Value;
        }


        //this method is used to test the gate. 
        //we simply check whether the truth table is properly implemented.
        public override bool TestGate()
        {
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0)
                return false;
            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 0)
                return false;
            return true;
        }
    }
}
