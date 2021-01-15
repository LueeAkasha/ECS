using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class IfStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> DoIfTrue { get; private set; }
        public List<StatetmentBase> DoIfFalse { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            DoIfTrue = new List<StatetmentBase>();
            DoIfFalse = new List<StatetmentBase>();
            if (((Statement)sTokens.Peek()).Name.Equals("if"))
            {
                Token tIf = sTokens.Pop();
                if (!(tIf is Statement) || !((Statement)tIf).Name.Equals("if"))
                    throw new SyntaxErrorException("$Expected if", tIf);

                Token con_open = sTokens.Pop();
                if (!(con_open is Parentheses) || ((Parentheses)con_open).Name != '(')
                    throw new SyntaxErrorException("$Expected (", con_open);

                Term = Expression.Create(sTokens);
                Term.Parse(sTokens);
               /* while (!(sTokens.Peek() is Parentheses))
                {
                    sTokens.Pop();
                }*/
                Token con_close = sTokens.Pop();
                if (!(con_close is Parentheses) || ((Parentheses)con_close).Name != ')')
                    throw new SyntaxErrorException("$Expected )", con_close);

                Token open_if = sTokens.Pop();
                if (!(open_if is Parentheses) || ((Parentheses)open_if).Name != '{')
                    throw new SyntaxErrorException("$Expected {", open_if);

                
                while(sTokens.Count > 0 & !(sTokens.Peek() is Parentheses))
                {
                    StatetmentBase statetmentBase = StatetmentBase.Create(sTokens.Peek());
                    statetmentBase.Parse(sTokens);
                    DoIfTrue.Add(statetmentBase);

                    if (sTokens.Count > 0 && sTokens.Peek() is Separator)//,
                        sTokens.Pop();

                }

                Token close_if = sTokens.Pop();
                if (!(close_if is Parentheses) || ((Parentheses)close_if).Name != '}')
                    throw new SyntaxErrorException("$Expected }", close_if);

                if ((sTokens.Peek() is Statement) && ((Statement)sTokens.Peek()).Name.Equals("else"))
                {

                    Token tElse = sTokens.Pop();
                    if (!(tElse is Statement) || !((Statement)tElse).Name.Equals("else"))
                        throw new SyntaxErrorException("$Expected else" + tElse.ToString(), tElse);


                    Token open_else = sTokens.Pop(); // not must
                    if (!(open_else is Parentheses) || ((Parentheses)open_else).Name != '{')
                        throw new SyntaxErrorException("$Expected {", open_else);

                    
                    while (sTokens.Count > 0 & !(sTokens.Peek() is Parentheses))
                    {
                        StatetmentBase statetmentBase = StatetmentBase.Create(sTokens.Peek());
                        statetmentBase.Parse(sTokens);
                        DoIfFalse.Add(statetmentBase);

                        if (sTokens.Count > 0 && sTokens.Peek() is Separator)//,
                            sTokens.Pop();
                    }


                    Token close_else = sTokens.Pop();
                    if (!(close_else is Parentheses) || ((Parentheses)close_else).Name != '}')
                        throw new SyntaxErrorException("$Expected }", close_else);
                }
            }
        }

        public override string ToString()
        {
            string sIf = "if(" + Term + "){\n";
            foreach (StatetmentBase s in DoIfTrue)
                sIf += "\t\t\t" + s + "\n";
            sIf += "\t\t}";
            if (DoIfFalse.Count > 0)
            {
                sIf += "else{";
                foreach (StatetmentBase s in DoIfFalse)
                    sIf += "\t\t\t" + s + "\n";
                sIf += "\t\t}";
            }
            return sIf;
        }

    }
}
