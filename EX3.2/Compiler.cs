using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class Compiler
    {

        private Dictionary<string, int> m_dSymbolTable;
        private int m_cLocals;

        public Compiler()
        {
            m_dSymbolTable = new Dictionary<string, int>();
            m_cLocals = 0;

        }

        public List<string> Compile(string sInputFile)
        {
            List<string> lCodeLines = ReadFile(sInputFile);
            List<Token> lTokens = Tokenize(lCodeLines);
            TokensStack sTokens = new TokensStack();
            for (int i = lTokens.Count - 1; i >= 0; i--)
                sTokens.Push(lTokens[i]);
            JackProgram program = Parse(sTokens);
            return null;
        }

        private JackProgram Parse(TokensStack sTokens)
        {
            JackProgram program = new JackProgram();
            program.Parse(sTokens);
            return program;
        }

        public List<string> Compile(List<string> lLines)
        {

            List<string> lCompiledCode = new List<string>();
            foreach (string sExpression in lLines)
            {
                List<string> lAssembly = Compile(sExpression);
                lCompiledCode.Add("// " + sExpression);
                lCompiledCode.AddRange(lAssembly);
            }
            return lCompiledCode;
        }



        public List<string> ReadFile(string sFileName)
        {
            StreamReader sr = new StreamReader(sFileName);
            List<string> lCodeLines = new List<string>();
            while (!sr.EndOfStream)
            {
                lCodeLines.Add(sr.ReadLine());
            }
            sr.Close();
            return lCodeLines;
        }


        public List<Token> Tokenize(List<string> lCodeLines)
        {

            List<Token> lTokens = new List<Token>();
            //your code here
            char[] delimiter = { ' ', ',', ';', '*', '+', '-', '/', '<', '>', '&', '=', '|', '!', '(', ')', '[', ']', '{', '}', '\t' };
            string sToken;
            int cChars;
            for (int i = 0; i < lCodeLines.Count; i++)
            {

                string code_line = lCodeLines[i];
                int position = 0;
                while (code_line != null && code_line.Length > 0)
                {
                    if (code_line.Length > 1 && code_line[0] == '/' && code_line[1] == '/')
                    {
                        break;
                    }
                    code_line = Next(code_line, delimiter, out sToken, out cChars);
                    if (sToken != " " && sToken != "\t")
                    {

                        if (sToken.Length == 1)
                        {

                            if (Token.Parentheses.Contains(sToken[0]))
                            {
                                Parentheses token = new Parentheses(sToken[0], i, position);
                                lTokens.Add(token);
                            }
                            else if (Token.Operators.Contains(sToken[0]))
                            {
                                Operator token = new Operator(sToken[0], i, position);
                                lTokens.Add(token);
                            }
                            else if (Token.Separators.Contains(sToken[0]))
                            {
                                Separator token = new Separator(sToken[0], i, position);
                                lTokens.Add(token);
                            }
                            else if (Token.Numbers.Contains(sToken[0]))
                            {
                                Number token = new Number(sToken, i, position);
                                lTokens.Add(token);
                            }
                            else
                            {
                                if (sToken[0] >= 'a' && sToken[0] <= 'z' || sToken[0] >= 'A' && sToken[0] <= 'Z')
                                {
                                    Identifier token = new Identifier(sToken, i, position);
                                    lTokens.Add(token);
                                }
                                else
                                {
                                    Token token = new Token();
                                    token.Line = i;
                                    token.Position = position;
                                    throw new SyntaxErrorException("syntaxError", token);
                                }
                            }
                        }
                        else if (sToken.Length > 1)
                        {

                            if (Token.Statements.Contains(sToken))
                            {
                                Statement token = new Statement(sToken, i, position);
                                lTokens.Add(token);
                            }
                            else if (Token.VarTypes.Contains(sToken))
                            {
                                VarType token = new VarType(sToken, i, position);
                                lTokens.Add(token);
                            }
                            else if (Token.Constants.Contains(sToken))
                            {
                                Constant token = new Constant(sToken, i, position);
                                lTokens.Add(token);
                            }
                            else if (isNumber(sToken))
                            {
                                Number token = new Number(sToken, i, position);
                                lTokens.Add(token);
                            }
                            else
                            {
                                if (sToken[0] >= 'a' && sToken[0] <= 'z' || sToken[0] >= 'A' && sToken[0] <= 'Z')
                                {
                                    for (int j = 1; j < sToken.Length; j++)
                                    {
                                        if (!(sToken[j] >= 'a' && sToken[j] <= 'z' || sToken[j] >= 'A' && sToken[j] <= 'Z' || Token.Numbers.Contains(sToken[j])))
                                        {
                                            Token corrupt_token = new Token();
                                            corrupt_token.Line = i;
                                            corrupt_token.Position = position;
                                            throw new SyntaxErrorException("syntaxError", corrupt_token);
                                        }

                                    }
                                    Identifier token = new Identifier(sToken, i, position);
                                    lTokens.Add(token);
                                }
                                else
                                {
                                    Token token = new Token();
                                    token.Line = i;
                                    token.Position = position;
                                    throw new SyntaxErrorException("syntaxError", token);
                                }
                            }
                        }

                    }
                    position = position + sToken.Length;
                }

            }
            return lTokens;
        }

        private bool isNumber(string sToken)
        {
            for (int i = 0; i < sToken.Length; i++)
            {
                if (!Token.Numbers.Contains(sToken[i]))
                {
                    return false;
                }
            }
            return true;
        }


        //Computes the next token in the string s, from the begining of s until a delimiter has been reached. 
        //Returns the string without the token.
        private string Next(string s, char[] aDelimiters, out string sToken, out int cChars)
        {
            cChars = 1;
            sToken = s[0] + "";
            if (aDelimiters.Contains(s[0]))
                return s.Substring(1);
            int i = 0;
            for (i = 1; i < s.Length; i++)
            {
                if (aDelimiters.Contains(s[i]))
                    return s.Substring(i);
                else
                    sToken += s[i];
                cChars++;
            }
            return null;
        }

    }
}
