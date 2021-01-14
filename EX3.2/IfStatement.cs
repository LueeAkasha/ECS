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
            if (((Statement)sTokens.Peek()).Name.Equals("if"))
            {
                Token tIf = sTokens.Pop();
                if (!(tIf is Statement) || !((Statement)tIf).Name.Equals("if"))
                    throw new SyntaxErrorException("$Expected if", tIf);

                Token con_open = sTokens.Pop();
                if (!(con_open is Parentheses) || !((Parentheses)con_open).Name.Equals("("))
                    throw new SyntaxErrorException("$Expected (", con_open);

                Term = Expression.Create(sTokens);
                Term.Parse(sTokens);

                Token con_close = sTokens.Pop();
                if (!(con_close is Parentheses) || !((Parentheses)con_close).Name.Equals(")"))
                    throw new SyntaxErrorException("$Expected )", con_close);

                Token open_if = sTokens.Pop();
                if (!(open_if is Parentheses) || !((Parentheses)open_if).Name.Equals("{"))
                    throw new SyntaxErrorException("$Expected {", open_if);
                
                while(sTokens.Count > 0 & !(sTokens.Peek() is Parentheses))
                {
                    StatetmentBase statetmentBase = StatetmentBase.Create(sTokens.Peek());
                    statetmentBase.Parse(sTokens);
                    DoIfTrue.Add(statetmentBase);
                }

                Token close_if = sTokens.Pop();
                if (!(close_if is Parentheses) || !((Parentheses)close_if).Name.Equals("}"))
                    throw new SyntaxErrorException("$Expected }", close_if);

                if ((sTokens.Peek() is Statement) && ((Statement)sTokens.Peek()).Name.Equals("else"))
                {

                    Token tElse = sTokens.Pop();
                    if (!(tIf is Statement) || !((Statement)tIf).Name.Equals("else"))
                        throw new SyntaxErrorException("$Expected else", tElse);


                    Token open_else = sTokens.Pop(); // not must

                    if (!(open_else is Parentheses) || !((Parentheses)open_else).Name.Equals("{"))
                        throw new SyntaxErrorException("$Expected {", open_else);


                    while (sTokens.Count > 0 & !(sTokens.Peek() is Parentheses))
                    {
                        StatetmentBase statetmentBase = StatetmentBase.Create(sTokens.Peek());
                        statetmentBase.Parse(sTokens);
                        DoIfFalse.Add(statetmentBase);
                    }


                    Token close_else = sTokens.Pop();
                    if (!(close_else is Parentheses) || !((Parentheses)close_else).Name.Equals("}"))
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
