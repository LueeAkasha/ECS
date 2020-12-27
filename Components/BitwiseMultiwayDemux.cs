using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Components
{
    //This class implements a demux with k outputs, each output with n wires. The input also has n wires.

    class BitwiseMultiwayDemux : Gate
    {
        //Word size - number of bits in each output
        public int Size { get; private set; }

        //The number of control bits needed for k outputs
        public int ControlBits { get; private set; }

        public WireSet Input { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet[] Outputs { get; private set; }

        BitwiseDemux bitwiseDemux;

        public BitwiseMultiwayDemux(int iSize, int cControlBits)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Control = new WireSet(cControlBits);
            Outputs = new WireSet[(int)Math.Pow(2, cControlBits)];
            
            for (int i = 0; i < Outputs.Length; i++)
            {
                Outputs[i] = new WireSet(Size);
            }

            Queue<WireSet> temp1 = new Queue<WireSet>();
            Queue<WireSet> temp2 = new Queue<WireSet>();
            
            temp1.Enqueue(Input);
            int level = cControlBits - 1;
            while (level != -1) {
                if (temp1.Count == 0) {
                    while (temp2.Count != 0) {
                        bitwiseDemux = new BitwiseDemux(iSize);
                        bitwiseDemux.ConnectControl(Control[level]);

                        bitwiseDemux.ConnectInput(temp2.Dequeue());
                        temp1.Enqueue(bitwiseDemux.Output1);
                        temp1.Enqueue(bitwiseDemux.Output2);
                    }
                }
                else if (temp2.Count == 0) {
                    while (temp1.Count != 0)
                    {
                        bitwiseDemux = new BitwiseDemux(iSize);
                        bitwiseDemux.ConnectControl(Control[level]);

                        bitwiseDemux.ConnectInput(temp1.Dequeue());
                        temp2.Enqueue(bitwiseDemux.Output1);
                        temp2.Enqueue(bitwiseDemux.Output2);
                    }
                }
                level--;
            }

            int j = 0;
            while (temp1.Count != 0) {
                Outputs[j].ConnectInput(temp1.Dequeue());
                j++;
            }
            while (temp2.Count != 0)
            {
                Outputs[j].ConnectInput(temp2.Dequeue());
                j++;
            }
        }


        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }
        public void ConnectControl(WireSet wsControl)
        {
            Control.ConnectInput(wsControl);
        }


        public override bool TestGate()
        {

            for (int i = 0; i < Input.Size ; i++) { //input [1,....,1]
                Input[i].Value = 1;
            }

            for(int j = 0; j<Control.Size; j++) // Control [0,....,0,1]
            {


                if (j != Control.Size - 1)
                {
                    Control[j].Value = 0;
                    continue;
                }
                else
                    Control[j].Value = 1;

            }
           

            int decimalValueOfControl = (int)Math.Pow(2, Control.Size-1); //output2^controlSize
            for (int k = 0; k<Size ;k++) {
                if (Outputs[decimalValueOfControl][k].Value == 0) {
                    return false;    // something went wrong!            
                }
            }

            return true; //is fix.
        }
    }
}
