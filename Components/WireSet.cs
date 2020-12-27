using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Components
{
    //This class represents a set of n wires (a cable)
    class WireSet
    {
        //Word size - number of bits in the register
        public int Size { get; private set; }
        
        public bool InputConected { get; private set; }

        //An indexer providing access to a single wire in the wireset
        public Wire this[int i]
        {
            get
            {
                return m_aWires[i];
            }
        }
        private Wire[] m_aWires;
        
        public WireSet(int iSize)
        {
            Size = iSize;
            InputConected = false;
            m_aWires = new Wire[iSize];
            for (int i = 0; i < m_aWires.Length; i++)
                m_aWires[i] = new Wire();
        }
        public override string ToString()
        {
            string s = "[";
            for (int i = m_aWires.Length - 1; i >= 0; i--)
                s += m_aWires[i].Value;
            s += "]";
            return s;
        }

        //Transform a positive integer value into binary and set the wires accordingly, with 0 being the LSB
        public void SetValue(int iValue) 
        {
            try
            {
                foreach (Wire i in m_aWires)
                {
                    i.Value = 0;
                }
                for (int i = 0; iValue != 0; i++)
                {
                    m_aWires[i].Value = iValue % 2;
                    iValue /= 2;
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new Exception("the number could not be set in the given number of bits, it's too long");
            }

        }

        //Transform the binary code into a positive integer
        public int GetValue()
        {
            int output = 0;
            for (int i=0; i<m_aWires.Length; i++) {
                output += (int)(this[i].Value * Math.Pow(2, i));
            }
            return output;
        }

        //Transform an integer value into binary using 2`s complement and set the wires accordingly, with 0 being the LSB
        public void Set2sComplement(int iValue) // 0
        {

           bool negative = iValue < 0;
           foreach(Wire i in m_aWires)
            {
                i.Value = 0;
            }
            if (negative)
            {
                
                SetValue(Math.Abs(iValue));
                int limit = m_aWires.Length;
                
                for (int  i = m_aWires.Length-1; i >= 0; i--)
                {
                    if (m_aWires[i].Value == 1)
                    {
                        limit = i;
                        break;
                    }
                }
                /*if (limit == m_aWires.Length)
                {
                    m_aWires[m_aWires.Length - 1].Value = 1;
                    return;
                }*/
                for ( int i = 0; i < m_aWires.Length ; i++)
                {
                    m_aWires[i].Value = (m_aWires[i].Value + 1) % 2;
                }
                for (int i = 0; i <=limit; i++)
                {
                    if (m_aWires[i].Value != 0)
                        m_aWires[i].Value = 0;
                    else
                    {
                        m_aWires[i].Value = 1;
                        break;
                    }
                }
                m_aWires[m_aWires.Length-1].Value = 1;
            }
            else
            {
                SetValue(iValue);
            }

        }


        //Transform the binary code in 2`s complement into an integer
        public int Get2sComplement()
        {
            bool negative = m_aWires[m_aWires.Length - 1].Value == 1;
            if (negative)
            {
                int output = 0;

                for (int i = 0; i <m_aWires.Length; i++)
                {
                    if (m_aWires[i].Value == 1)
                    {
                        output += (int)Math.Pow(2, i);
                        for(int j = i+1; j < m_aWires.Length ; j++)
                        {
                            
                            if (m_aWires[j].Value == 0)
                            {
                                output += (int)Math.Pow(2, j);
                            }
                        }

                        break;
                    }
                    
                }
                return output * (-1);
            }
            else
            {
                return GetValue();
            }


           
        }
        
        public void ConnectInput(WireSet wIn)
        {
            if (InputConected)
                throw new InvalidOperationException("Cannot connect a wire to more than one inputs");
            if(wIn.Size != Size)
                throw new InvalidOperationException("Cannot connect two wiresets of different sizes.");
            for (int i = 0; i < m_aWires.Length; i++)
                m_aWires[i].ConnectInput(wIn[i]);

            InputConected = true;
            
        }

    }
}
