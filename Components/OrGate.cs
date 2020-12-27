using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //This gate implements the or operation. To implement it, follow the example in the And gate.
    class OrGate : TwoInputGate
    {
        private NotGate not1;
        private NotGate not2;
        private NAndGate Nand;
        public OrGate()
        {
            //init the gates.
            not1 = new NotGate();
            not2 = new NotGate();
            Nand = new NAndGate();

     

            Nand.ConnectInput1(not1.Output);
            Nand.ConnectInput2(not2.Output);

            Output = Nand.Output;
            Input1 = not1.Input;
            Input2 = not2.Input;

        }


        public override string ToString()
        {
            return "Or " + Input1.Value + "," + Input2.Value + " -> " + Output.Value;
        }
        
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
            if (Output.Value != 1)
                return false;
            return true;
        }
    }

}
