using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Components
{
    //This class implements a mux with k input, each input with n wires. The output also has n wires.

    class BitwiseMultiwayMux : Gate
    {
        //Word size - number of bits in each output
        public int Size { get; private set; }

        //The number of control bits needed for k outputs
        public int ControlBits { get; private set; }

        public WireSet Output { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet[] Inputs { get; private set; }

        BitwiseMux bitwiseMux;

        
        public BitwiseMultiwayMux(int iSize, int cControlBits)
        {
            Size = iSize;
            Output = new WireSet(Size);
            Control = new WireSet(cControlBits);
            Inputs = new WireSet[(int)Math.Pow(2, cControlBits)];
            
            for (int i = 0; i < Inputs.Length; i++)
            {
                Inputs[i] = new WireSet(Size);
                
            }
            WireSet[] temp;
            temp = new WireSet[Inputs.Length / 2];
            int tempPointer = 0;
            int controlindex = 0;

            for (int i = 0; i + 1 < Inputs.Length; i += 2)
            {
                bitwiseMux = new BitwiseMux(iSize);
                bitwiseMux.ConnectControl(Control[controlindex]);
                bitwiseMux.ConnectInput1(Inputs[i]);
                bitwiseMux.ConnectInput2(Inputs[i + 1]);

                temp[tempPointer] = bitwiseMux.Output;
                tempPointer++;
            }
            controlindex++;

            int limit = temp.Length;

            while (limit != 0)
            {

                tempPointer = 0;
                for (int i = 0; i + 1 < limit; i += 2)
                {
                    bitwiseMux = new BitwiseMux(iSize);
                    bitwiseMux.ConnectControl(Control[controlindex]);

                    bitwiseMux.ConnectInput1(temp[i]);
                    bitwiseMux.ConnectInput2(temp[i + 1]);
                    temp[tempPointer] = bitwiseMux.Output;
                    tempPointer++;
                }

                controlindex++;
                limit /= 2;
            }

            Output.ConnectInput(temp[0]);
            /* Queue<WireSet> temp1 = new Queue<WireSet>();
             Queue<WireSet> temp2 = new Queue<WireSet>();

             int controlindex = Control.Size-1;

             for (int i = 0; i < Inputs.Length ; i=i+2)
             {
                 bitwiseMux = new BitwiseMux(iSize);
                 bitwiseMux.ConnectControl(Control[controlindex]);
                 bitwiseMux.ConnectInput1(Inputs[i]);
                 bitwiseMux.ConnectInput2(Inputs[i]);
                 temp1.Enqueue(bitwiseMux.Output);

             }
             controlindex--;

             int level = controlindex;
             while (level != -1)
             {

                 if (temp2.Count == 0)
                 {
                     while (temp1.Count >= 2)
                     {
                         bitwiseMux = new BitwiseMux(iSize);
                         bitwiseMux.ConnectControl(Control[controlindex]);
                         bitwiseMux.ConnectInput1(temp1.Dequeue());
                         bitwiseMux.ConnectInput2(temp1.Dequeue());
                         temp2.Enqueue(bitwiseMux.Output);

                     }
                 }
                 else if (temp1.Count == 0)
                 {
                     while (temp2.Count >= 2) {
                         bitwiseMux = new BitwiseMux(iSize);
                         bitwiseMux.ConnectControl(Control[controlindex]);
                         bitwiseMux.ConnectInput1(temp2.Dequeue());
                         bitwiseMux.ConnectInput2(temp2.Dequeue());
                         temp1.Enqueue(bitwiseMux.Output);
                     }
                 }
                 controlindex--;
                 level--;
             }

             if (temp1.Count != 0)
                 Output.ConnectInput( temp1.Dequeue());
             else
                 Output.ConnectInput( temp2.Dequeue());

             */

            /* WireSet[] temp;
             temp = new WireSet[Inputs.Length/2];


             for (int i =0;i+1<Inputs.Length ; i+=2) {
                 bitwiseMux = new BitwiseMux(iSize);
                 bitwiseMux.ConnectControl(Control[controlindex]);
                 bitwiseMux.ConnectInput1(Inputs[i]);
                 bitwiseMux.ConnectInput2(Inputs[i+1]);

                 temp[tempPointer] = bitwiseMux.Output;
                 tempPointer++;
             }
             controlindex--;

             int limit = temp.Length;

             while (limit != -1) {

                 tempPointer = 0;
                 for (int i = 0; i+1 < limit ;i+=2) {
                     bitwiseMux = new BitwiseMux(iSize);
                     bitwiseMux.ConnectControl(Control[controlindex]);

                     bitwiseMux.ConnectInput1(temp[i]);
                     bitwiseMux.ConnectInput2(temp[i+1]);
                     temp[tempPointer] = bitwiseMux.Output;
                     tempPointer++;
                 }

                 controlindex--;
                 if (limit == 0) break;
                 limit /= 2;
             }

             Output = temp[0];*/

        }


        public void ConnectInput(int i, WireSet wsInput)
        {
            Inputs[i].ConnectInput(wsInput);
        }
        public void ConnectControl(WireSet wsControl)
        {
            Control.ConnectInput(wsControl);
        }

        

        public override bool TestGate()
        {
            
          
            for (int i = 0; i<Control.Size ;i++) { //[1,1,0,...,0]
                Control[i].Value = 1;
            }
            
            foreach (WireSet wireSet in Inputs) //[0,.....,0],[0,......,0],....,[0,....,0]
            {
                for (int j = 0; j < wireSet.Size; j++)
                {
                        wireSet[j].Value = 0;
                }
            }

            for(int i = 0; i<Inputs[Inputs.Length-1].Size; i++)
            {
                Inputs[Inputs.Length - 1][i].Value = 1;
            }
            


            

            for (int k = 0; k<Output.Size ; k +=  1) {
                if (Output[k].Value != 1)
                {// something went wrong!
                    Console.WriteLine(Output[k].Value);
                    return false;
                }
            }

            return true; // is working!
            
        }
    }
}
