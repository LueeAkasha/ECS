using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    class UnaryOperatorExpression : Expression
    {
        public string Operator { get; set; }
        public Expression Operand { get; set; }

        public override string ToString()
        {
            return Operator + Operand;
        }

        public override void Parse(TokensStack sTokens)
        {
            Token tOperator = sTokens.Pop();
            if (!(tOperator is Operator))
                throw new SyntaxErrorException("$Expected operator", tOperator);
            else
            {
                Operator = ((Operator)tOperator).Name.ToString();
                Operand = Expression.Create(sTokens);
                Operand.Parse(sTokens);
              /*  while (!(sTokens.Peek() is Separator))
                {
                    sTokens.Pop();
                }*/
            }

           // Token tEnd = sTokens.Pop();
            //if (!(tEnd is Separator) || ((Separator)tEnd).Name!=';')
              //  throw new SyntaxErrorException("$Expected ;" + tEnd.ToString(), tEnd);

        }
    }
}
