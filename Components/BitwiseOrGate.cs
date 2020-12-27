using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Components
{
    //A two input bitwise gate takes as input two WireSets containing n wires, and computes a bitwise function - z_i=f(x_i,y_i)
    class BitwiseOrGate : BitwiseTwoInputGate
    {
        private OrGate or;

        public BitwiseOrGate(int iSize)
            : base(iSize)
        {
            or = new OrGate();

            for (int i = 0; i < Size; i++)
            {
                or.ConnectInput1(Input1[i]);
                or.ConnectInput2(Input2[i]);

                Output[i].ConnectInput(or.Output);
                or = new OrGate();
            }
        }

        //an implementation of the ToString method is called, e.g. when we use Console.WriteLine(or)
        //this is very helpful during debugging
        public override string ToString()
        {
            return "Or " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {
            for (int i = 0; i < Size; i++)
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
                if (Output[i].Value != 1)
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
