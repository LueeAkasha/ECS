using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //A two input bitwise gate takes as input two WireSets containing n wires, and computes a bitwise function - z_i=f(x_i,y_i)
    class BitwiseAndGate : BitwiseTwoInputGate
    {
        private AndGate and;

        public BitwiseAndGate(int iSize)
            : base(iSize)
        {
            and = new AndGate();

            for (int i = 0; i < Size; i++) {
                and.ConnectInput1(Input1[i]);
                and.ConnectInput2(Input2[i]);

                Output[i].ConnectInput(and.Output);
                and = new AndGate();
            }
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(and)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "And " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {
           
            for (int i = 0; i<Size ;i++) {

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
                if (Output[i].Value != 0)
                    return false;
                Input1[i].Value = 1;
                Input2[i].Value = 1;
                if (Output[i].Value != 1)
                    return false;

            }

            return true;

            
        }
    }
}
