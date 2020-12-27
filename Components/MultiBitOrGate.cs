using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    //Multibit gates take as input k bits, and compute a function over all bits - z=f(x_0,x_1,...,x_k)

    class MultiBitOrGate : MultiBitGate
    {
        private OrGate or;

        public MultiBitOrGate(int iInputCount)
            : base(iInputCount)
        {
            or = new OrGate();
            or.ConnectInput1(m_wsInput[0]);
            or.ConnectInput2(m_wsInput[1]);
            Wire temp = or.Output;

            for (int i = 2; i < iInputCount; i++)
            {
                or = new OrGate();

                or.ConnectInput1(temp);
                or.ConnectInput2(m_wsInput[i]);

                temp = or.Output;
            }

            Output = temp;

        }

        public override bool TestGate()
        {
            for (int i = 0; i < m_wsInput.Size; i++)
            {
                if (m_wsInput[i].Value == 1)
                {
                    if (Output.Value != 1)
                        return false;
                }
            }
            return true;
        }
    }
}
