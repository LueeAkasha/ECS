using System;
using System.Collections.Generic;

namespace SimpleCompiler
{
    public class FunctionCallExpression : Expression
    {
        public string FunctionName { get; private set; }
        public List<Expression> Args { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            Token tFun = sTokens.Pop();
            if (!(tFun is Identifier))
                throw new SyntaxErrorException("$Expected identifier", tFun);
            else
                this.FunctionName = ((Identifier)tFun).ToString();
            Token fun_open = sTokens.Pop();
            if (!(fun_open is Parentheses) || ((Parentheses)fun_open).Name != '(')
                throw new SyntaxErrorException("$Expected (", fun_open);

            Args = new List<Expression>();
            // problematic scope!
            while (sTokens.Count > 0  && !((sTokens.Peek() is Parentheses) && ((Parentheses)sTokens.Peek()).Name==')')) {


                Expression arg = Expression.Create(sTokens);
                arg.Parse(sTokens);
                Args.Add(arg);
                

                if (sTokens.Count > 0 && sTokens.Peek() is Separator)//,
                    sTokens.Pop();
            }


            



            Token fun_close = sTokens.Pop();
            if (!(fun_close is Parentheses) || ((Parentheses)fun_close).Name != ')')
                throw new SyntaxErrorException("$Expected )" + fun_close.ToString() +"  "+ sTokens.Pop().ToString() + "  " + sTokens.Pop().ToString() + "  " + sTokens.Pop().ToString(), fun_close);

            //Token tEnd = sTokens.Pop();
           // if (!(tEnd is Separator) || ((Separator)tEnd).Name != ';')
             //       throw new SyntaxErrorException("$Expected ;", tEnd);
        }

        public override string ToString()
        {
            string sFunction = FunctionName + "(";
            for (int i = 0; i < Args.Count - 1; i++)
                sFunction += Args[i] + ",";
            if (Args.Count > 0)
                sFunction += Args[Args.Count - 1];
            sFunction += ")";
            return sFunction;
        }
    }
}