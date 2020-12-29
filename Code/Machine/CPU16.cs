using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleComponents;

namespace Machine
{
    public class CPU16 
    {
        //this "enum" defines the different control bits names
        public const int J3 = 0, J2 = 1, J1 = 2, D3 = 3, D2 = 4, D1 = 5, C6 = 6, C5 = 7, C4 = 8, C3 = 9, C2 = 10, C1 = 11, A = 12, X2 = 13, X1 = 14, Type = 15;

        public int Size { get; private set; }

        //CPU inputs
        public WireSet Instruction { get; private set; }
        public WireSet MemoryInput { get; private set; }
        public Wire Reset { get; private set; }

        //CPU outputs
        public WireSet MemoryOutput { get; private set; }
        public Wire MemoryWrite { get; private set; }
        public WireSet MemoryAddress { get; private set; }
        public WireSet InstructionAddress { get; private set; }

        //CPU components
        private ALU m_gALU;
        private Counter m_rPC;
        private MultiBitRegister m_rA, m_rD;
        private BitwiseMux m_gAMux, m_gMAMux;
        

        
        //here we initialize and connect all the components, as in Figure 5.9 in the book
        public CPU16()
        {
            Size =  16;

            Instruction = new WireSet(Size);
            MemoryInput = new WireSet(Size);
            MemoryOutput = new WireSet(Size);
            MemoryAddress = new WireSet(Size);
            InstructionAddress = new WireSet(Size);
            MemoryWrite = new Wire();
            Reset = new Wire();

            m_gALU = new ALU(Size);
            m_rPC = new Counter(Size);
            m_rA = new MultiBitRegister(Size);
            m_rD = new MultiBitRegister(Size);

            m_gAMux = new BitwiseMux(Size);
            m_gMAMux = new BitwiseMux(Size);

            m_gAMux.ConnectInput1(Instruction);
            m_gAMux.ConnectInput2(m_gALU.Output);

            m_rA.ConnectInput(m_gAMux.Output);

            m_gMAMux.ConnectInput1(m_rA.Output);
            m_gMAMux.ConnectInput2(MemoryInput);
            m_gALU.InputY.ConnectInput(m_gMAMux.Output);

            m_gALU.InputX.ConnectInput(m_rD.Output);

            m_rD.ConnectInput(m_gALU.Output);

            MemoryOutput.ConnectInput(m_gALU.Output);
            MemoryAddress.ConnectInput(m_rA.Output);

            InstructionAddress.ConnectInput(m_rPC.Output);
            m_rPC.ConnectInput(m_rA.Output);
            m_rPC.ConnectReset(Reset);

            //now, we call the code that creates the control unit
            ConnectControls();
        }

        //add here components to implement the control unit 
        private BitwiseMultiwayMux m_gJumpMux;//an example of a control unit compnent - a mux that controls whether a jump is made
        private AndGate and;
        private OrGate orOfA;
        private AndGate andOfMemoryWrite;
        private AndGate andOfpc;
        private OrGate jumpOr1;
        private OrGate jumpOr2;
        private NotGate jumpNot1;
        private NotGate jumpNot2;
        private NotGate jumpNot3;
        AndGate andOfA;
        NotGate notOfA;
        private void ConnectControls()
        {
            //1. connect control of mux 1 (selects entrance to register A)
            
            m_gAMux.ConnectControl(Instruction[Type]); // opcode

            //2. connect control to mux 2 (selects A or M entrance to the ALU)

            m_gMAMux.ConnectControl(Instruction[A]);  //  a/m 

            //3. consider all instruction bits only if C type instruction (MSB of instruction is 1)


            //4. connect ALU control bits

            m_gALU.ZeroX.ConnectInput(Instruction[C1]);
            m_gALU.NotX.ConnectInput(Instruction[C2]);
            m_gALU.ZeroY.ConnectInput(Instruction[C3]);
            m_gALU.NotY.ConnectInput(Instruction[C4]);
            m_gALU.F.ConnectInput(Instruction[C5]);
            m_gALU.NotOutput.ConnectInput(Instruction[C6]);

            //5. connect control to register D (very simple)
            and = new AndGate();
            and.ConnectInput1(Instruction[Type]);
            and.ConnectInput2(Instruction[D2]);  
            m_rD.Load.ConnectInput(and.Output);


            //6. connect control to register A (a bit more complicated)
            orOfA = new OrGate(); // shortcut for what we did in lesson "instead using and & not gates"
            
            // to implement as in lecture
            andOfA = new AndGate();
            notOfA = new NotGate();
            notOfA.ConnectInput(Instruction[Type]);
            andOfA.ConnectInput1(Instruction[Type]);
            andOfA.ConnectInput2(Instruction[D1]);
            orOfA.ConnectInput1(andOfA.Output);
            orOfA.ConnectInput2(notOfA.Output);
            m_rA.Load.ConnectInput(orOfA.Output);
            //7. connect control to MemoryWritea
            andOfMemoryWrite = new AndGate();
            andOfMemoryWrite.ConnectInput1(Instruction[Type]);
            andOfMemoryWrite.ConnectInput2(Instruction[D3]);
            MemoryWrite.ConnectInput(andOfMemoryWrite.Output);

            //8. create inputs for jump mux
            Wire on = new Wire();
            on.Value = 1;
            Wire off = new Wire();
            off.Value = 0;

            m_gJumpMux = new BitwiseMultiwayMux(1, 3);  // so i'm gonna use it as jump box in lesson
            jumpOr1 = new OrGate();
            jumpOr1.ConnectInput1(m_gALU.Zero);
            jumpOr1.ConnectInput2(m_gALU.Negative);
            jumpNot1 = new NotGate();
            jumpNot1.ConnectInput(jumpOr1.Output);
            jumpNot2 = new NotGate();
            jumpNot2.ConnectInput(m_gALU.Negative);
            jumpNot3 = new NotGate();
            jumpNot3.ConnectInput(m_gALU.Zero);
            jumpOr2 = new OrGate();
            jumpOr2.ConnectInput1(m_gALU.Zero);
            jumpOr2.ConnectInput2(m_gALU.Negative);

            Wire input1 = off; // we did somthing like this in practical session
            Wire input2 = jumpNot1.Output;
            Wire input3 = m_gALU.Zero;
            Wire input4 = jumpNot2.Output;
            Wire input5 = m_gALU.Negative;
            Wire input6 = jumpNot3.Output;
            Wire input7 = jumpOr2.Output;
            Wire input8 = on;


            //9. connect jump mux (this is the most complicated part
            m_gJumpMux.Inputs[0][0].ConnectInput(input1);
            m_gJumpMux.Inputs[1][0].ConnectInput(input2);
            m_gJumpMux.Inputs[2][0].ConnectInput(input3);
            m_gJumpMux.Inputs[3][0].ConnectInput(input4);
            m_gJumpMux.Inputs[4][0].ConnectInput(input5);
            m_gJumpMux.Inputs[5][0].ConnectInput(input6);
            m_gJumpMux.Inputs[6][0].ConnectInput(input7);
            m_gJumpMux.Inputs[7][0].ConnectInput(input8);
            
            m_gJumpMux.Control[0].ConnectInput(Instruction[J3]);
            m_gJumpMux.Control[1].ConnectInput(Instruction[J2]);
            m_gJumpMux.Control[2].ConnectInput(Instruction[J1]);   
            
            //10. connect PC load control
            andOfpc = new AndGate();
            andOfpc.ConnectInput1(Instruction[Type]);
            andOfpc.ConnectInput2(m_gJumpMux.Output[0]);


            m_rPC.ConnectLoad(andOfpc.Output);
        }


        public override string ToString()
        {
            return "A=" + m_rA + ", D=" + m_rD + ", PC=" + m_rPC + ",Ins=" + Instruction;
        }

        private string GetInstructionString()
        {
            if (Instruction[Type].Value == 0)
                return "@" + Instruction.GetValue();
            return Instruction[Type].Value + "XX " +
               "a" + Instruction[A] + " " +
               "c" + Instruction[C1] + Instruction[C2] + Instruction[C3] + Instruction[C4] + Instruction[C5] + Instruction[C6] + " " +
               "d" + Instruction[D1] + Instruction[D2] + Instruction[D3] + " " +
               "j" + Instruction[J1] + Instruction[J2] + Instruction[J3];
        }

        //use this function in debugging to print the current status of the ALU. Feel free to add more things for printing.
        public void PrintState()
        {
            Console.WriteLine("CPU state:");
            Console.WriteLine("PC=" + m_rPC + "=" + m_rPC.Output.GetValue());
            Console.WriteLine("A=" + m_rA + "=" + m_rA.Output.GetValue());
            Console.WriteLine("D=" + m_rD + "=" + m_rD.Output.GetValue());
            Console.WriteLine("Ins=" + GetInstructionString());
            Console.WriteLine("ALU=" + m_gALU);
            Console.WriteLine("inM=" + MemoryInput);
            Console.WriteLine("outM=" + MemoryOutput);
            Console.WriteLine("addM=" + MemoryAddress);
        }
    }
}
