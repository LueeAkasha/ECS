using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //Multibit gates take as input k bits, and compute a function over all bits - z=f(x_0,x_1,...,x_k)
    class MultiBitAndGate : MultiBitGate
    {
        private AndGate and;

        public MultiBitAndGate(int iInputCount)
            : base(iInputCount)
        {
            and = new AndGate();
            and.ConnectInput1(m_wsInput[0]);
            and.ConnectInput2(m_wsInput[1]);
            Wire temp = and.Output;

            for (int i = 2; i<m_wsInput.Size; i++) {
                and = new AndGate();

                and.ConnectInput1(temp);
                and.ConnectInput2(m_wsInput[i]);

                temp = and.Output;
            }

            Output = temp;

        }

       
        public override bool TestGate()
        {
            for (int i = 0;  i< m_wsInput.Size; i++ ) {
                if (m_wsInput[i].Value == 0) {
                    if (Output.Value != 0)
                        return false;
                }
            }
            return true;
        }
    }
}
