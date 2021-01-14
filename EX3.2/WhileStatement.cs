using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class WhileStatement : StatetmentBase
    {
        public Expression Term { get; private set; }
        public List<StatetmentBase> Body { get; private set; }

        public override void Parse(TokensStack sTokens)
        {
            Token tWhile = sTokens.Pop();
            if (!(tWhile is Statement) || !((Statement)tWhile).Name.Equals("while"))
                throw new SyntaxErrorException("$Expected while", tWhile);

            Token con_open = sTokens.Pop();
            if (!(con_open is Parentheses) || !((Parentheses)con_open).Name.Equals("("))
                throw new SyntaxErrorException("$Expected (", con_open);

            Term = Expression.Create(sTokens);
            Term.Parse(sTokens);

            Token con_close = sTokens.Pop();
            if (!(con_close is Parentheses) || !((Parentheses)con_close).Name.Equals(")"))
                throw new SyntaxErrorException("$Expected )", con_close);

            Token while_open = sTokens.Pop();
            if (!(while_open is Parentheses) || !((Parentheses)while_open).Name.Equals("{"))
                throw new SyntaxErrorException("$Expected {", while_open);

            while (sTokens.Count > 0 & !(sTokens.Peek() is Parentheses))
            {
                StatetmentBase statetmentBase = StatetmentBase.Create(sTokens.Peek());
                statetmentBase.Parse(sTokens);
                Body.Add(statetmentBase);
            }
            Token while_close = sTokens.Pop();
            if (!(while_close is Parentheses) || !((Parentheses)while_close).Name.Equals("}"))
                throw new SyntaxErrorException("$Expected }", while_close);
        }

        public override string ToString()
        {
            string sWhile = "while(" + Term + "){\n";
            foreach (StatetmentBase s in Body)
                sWhile += "\t\t\t" + s + "\n";
            sWhile += "\t\t}";
            return sWhile;
        }

    }
}
