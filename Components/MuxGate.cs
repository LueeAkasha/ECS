using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A mux has 2 inputs. There is a single output and a control bit, selecting which of the 2 inpust should be directed to the output.
    class MuxGate : TwoInputGate
    {
        public Wire ControlInput { get; private set; }
        private NotGate not;
        private AndGate and1;
        private AndGate and2;
        private OrGate or;


        public MuxGate()
        {
            ControlInput = new Wire();

            not = new NotGate();
            and1 = new AndGate();
            and2 = new AndGate();
            or = new OrGate();

            not.ConnectInput(ControlInput);

            and1.ConnectInput1(not.Output);
            //and1.ConnectInput2();
            
            and2.ConnectInput1(ControlInput);
            //and2.ConnectInput2(Input2);

            Input1 = and1.Input2;
            Input2 = and2.Input2;
      
            or.ConnectInput1(and1.Output);
            or.ConnectInput2(and2.Output);
            Output = or.Output;

        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }


        public override string ToString()
        {
            return "Mux " + Input1.Value + "," + Input2.Value + ",C" + ControlInput.Value + " -> " + Output.Value;
        }



        public override bool TestGate()
        {
            if (ControlInput.Value == 0) {
                Input1.Value = 0;
                Input2.Value = 0;
                if (Output.Value != 0)
                    return false;
                Input1.Value = 0;
                Input2.Value = 1;
                if (Output.Value != 0)
                    return false;
                Input1.Value = 1;
                Input2.Value = 0;
                if (Output.Value != 1)
                    return false;
                Input1.Value = 1;
                Input2.Value = 1;
                if (Output.Value != 1)
                    return false;

            }
            else if (ControlInput.Value == 1) {

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
                if (Output.Value != 0)
                    return false;
                Input1.Value = 1;
                Input2.Value = 1;
                if (Output.Value != 1)
                    return false;
             
            }
            return true;
        }
    }
}
