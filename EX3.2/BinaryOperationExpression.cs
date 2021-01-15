using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class BinaryOperationExpression : Expression
    {
        public string Operator { get;  set; }
        public Expression Operand1 { get;  set; }
        public Expression Operand2 { get;  set; }

        public override string ToString()
        {
            return "(" + Operand1 + " " + Operator + " " + Operand2 + ")";
        }

        public override void Parse(TokensStack sTokens)
        {
            Token open = sTokens.Pop();
            if (!(open is Parentheses) || ((Parentheses)open).Name!='(' )
                throw new SyntaxErrorException("$Expected (" + open, open);

            Operand1 = Expression.Create(sTokens);
            Operand1.Parse(sTokens);

           // while (!(sTokens.Peek() is Operator))
          //  {
          //      sTokens.Pop();
         //   }

            Token tOperator = sTokens.Pop();
            if (!(tOperator is Operator))
                throw new SyntaxErrorException("$Expected operator ", tOperator);
            else
                Operator = "" + ((Operator)tOperator).Name;


            Operand2 = Expression.Create(sTokens);
            Operand2.Parse(sTokens);

            //while (!(sTokens.Peek() is Parentheses))
           // {
          //      sTokens.Pop();
          //  }
            Token close = sTokens.Pop();
            if (!(close is Parentheses) || ((Parentheses)close).Name != ')')
                throw new SyntaxErrorException("$Expected )", close);
        }
    }
}
