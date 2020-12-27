using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class Program
    {
        static void Main(string[] args)
        {
            //This is an example of a testing code that you should run for all the gates that you create

            //Create a gate
            // AndGate and = new AndGate();
            //Test that the unit testing works properly
            /* if (!and.TestGate())
                 Console.WriteLine("bugbug");*/
            /*  OrGate or = new OrGate();
              if (!or.TestGate())
                  Console.WriteLine("bugbug");*/
            //    XorGate xor = new XorGate();
            //  OrGate or = new OrGate();
            //   MuxGate mux = new MuxGate();

            ///   MultiBitAndGate m = new MultiBitAndGate(12);

            //    BitwiseNotGate n = new BitwiseNotGate(200);
            //    if (!n.TestGate())
            //       Console.WriteLine(" xor bugbug");
            //Now we ruin the nand gates that are used in all other gates. The gate should not work properly after this.
            /*  NAndGate.Corrupt = true;
              if (and.TestGate())
                  Console.WriteLine("bugbug");

              */


            //   set.Set2sComplement(-15);
            //  Console.WriteLine(set);
            //   Console.WriteLine(set.Get2sComplement());


            // Console.WriteLine(set);

            /*   ALU alu = new ALU(8);
               WireSet seven = new WireSet(8);
               seven.SetValue(7);
               WireSet ten = new WireSet(8);
               ten.SetValue(10);

               alu.InputX.ConnectInput(seven);
               alu.InputY.ConnectInput( ten);


               Console.WriteLine(alu.Output.GetValue());
               Console.WriteLine("done");
               Console.ReadLine();*/

            MultiBitAdder add = new MultiBitAdder(8);
            if (!add.TestGate())
                Console.WriteLine("bugbug");

            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
