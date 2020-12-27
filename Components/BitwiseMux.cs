using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A bitwise gate takes as input WireSets containing n wires, and computes a bitwise function - z_i=f(x_i)
    class BitwiseMux : BitwiseTwoInputGate
    {
        public Wire ControlInput { get; private set; }

        private MuxGate mux;

        public BitwiseMux(int iSize)
            : base(iSize)
        {

            ControlInput = new Wire();
            mux = new MuxGate();

            for (int i=0; i< Size; i++) {

                mux.ConnectInput1(Input1[i]);
                mux.ConnectInput2(Input2[i]);
                mux.ConnectControl(ControlInput);

                Output[i].ConnectInput(mux.Output);
                mux = new MuxGate();
            }
           

        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }



        public override string ToString()
        {
            return "Mux " + Input1 + "," + Input2 + ",C" + ControlInput.Value + " -> " + Output;
        }




        public override bool TestGate()
        {
            for (int i =  0; i<Size; i++) {
                if (ControlInput.Value == 0)
                {
                    Input1[i].Value = 0;
                    Input2[i].Value = 0;
                    if (Output[i].Value != 0)
                        return false;
                    Input1[i].Value = 0;
                    Input2[i].Value = 1;
                    if (Output[i].Value != 0)
                        return false;
                    Input1[i].Value = 1;
                    Input2[i].Value = 0;
                    if (Output[i].Value != 1)
                        return false;
                    Input1[i].Value = 1;
                    Input2[i].Value = 1;
                    if (Output[i].Value != 1)
                        return false;

                }
                else if (ControlInput.Value == 1)
                {

                    Input1[i].Value = 0;
                    Input2[i].Value = 0;
                    if (Output[i].Value != 0)
                        return false;
                    Input1[i].Value = 0;
                    Input2[i].Value = 1;
                    if (Output[i].Value != 1)
                        return false;
                    Input1[i].Value = 1;
                    Input2[i].Value = 0;
                    if (Output[i].Value != 0)
                        return false;
                    Input1[i].Value = 1;
                    Input2[i].Value = 1;
                    if (Output[i].Value != 1)
                        return false;

                }
            }
            return true;
        }
    }
}
