using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class LetStatement : StatetmentBase
    {
        public string Variable { get; set; }
        public Expression Value { get; set; }

        public override string ToString()
        {
            return "let " + Variable + " = " + Value + ";";
        }

        public override void Parse(TokensStack sTokens)
        {
            Token tLet = sTokens.Pop();
            if (!(tLet is Statement) || !((Statement)tLet).Name.Equals("let"))
                throw new SyntaxErrorException("$Expected let", tLet);

            Token tId = sTokens.Pop();
            if (!(tId is Identifier))
                throw new SyntaxErrorException("$Expected identifier", tId);
            else
                this.Variable = ((Identifier)tId).ToString();

            Token tEqual = sTokens.Pop();
            if (!(tEqual is Operator) || ((Operator)tEqual).Name != '=')
                throw new SyntaxErrorException("$Expected =",tEqual);


            Value = Expression.Create(sTokens);
            Value.Parse(sTokens);

           // Token tEnd = sTokens.Pop();
            //if (!(tEnd is Separator) || ((Separator)tEnd).Name != ';')
              //  throw new SyntaxErrorException("$Expected ;" + tEnd.ToString(), tEnd);
        }

    }
}
