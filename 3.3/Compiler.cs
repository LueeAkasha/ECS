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


        public Compiler()
        {

        }


        public List<VarDeclaration> ParseVarDeclarations(List<string> lVarLines)
        {
            List<VarDeclaration> lVars = new List<VarDeclaration>();
            for(int i = 0; i < lVarLines.Count; i++)
            {
                List<Token> lTokens = Tokenize(lVarLines[i], i);
                TokensStack stack = new TokensStack(lTokens);
                VarDeclaration var = new VarDeclaration();
                var.Parse(stack);
                lVars.Add(var);
            }
            return lVars;
        }


        public List<LetStatement> ParseAssignments(List<string> lLines)
        {
            List<LetStatement> lParsed = new List<LetStatement>();
            List<Token> lTokens = Tokenize(lLines);
            TokensStack sTokens = new TokensStack();
            for (int i = lTokens.Count - 1; i >= 0; i--)
                sTokens.Push(lTokens[i]);
            while(sTokens.Count > 0)
            {
                LetStatement ls = new LetStatement();
                ls.Parse(sTokens);
                lParsed.Add(ls);
                if (sTokens.Count > 0 && sTokens.Peek() is Separator)//,
                    sTokens.Pop();
            }
            return lParsed;
        }

 

        public List<string> GenerateCode(LetStatement aSimple, Dictionary<string, int> dSymbolTable)
        {
            List<string> lAssembly = new List<string>();
            //add here code for computing a single let statement containing only a simple expression
           

            if (aSimple.Value is NumericExpression) {
                lAssembly.Add("@" + ((NumericExpression)aSimple.Value).Value);
                lAssembly.Add("D=A");
            }
            else if (aSimple.Value is BinaryOperationExpression) {
                if (((BinaryOperationExpression)aSimple.Value).Operand1 is VariableExpression && !dSymbolTable.ContainsKey(((BinaryOperationExpression)aSimple.Value).Operand1.ToString()))
                    throw new Exception(((BinaryOperationExpression)aSimple.Value).Operand1 + " Was not defind!");
                lAssembly.Add("@" + ((BinaryOperationExpression)aSimple.Value).Operand1.ToString());
                lAssembly.Add("D=A");
                lAssembly.Add("@OPERAND1");
                lAssembly.Add("M=D");

                    if (((BinaryOperationExpression)aSimple.Value).Operand1 is VariableExpression && !dSymbolTable.ContainsKey(((BinaryOperationExpression)aSimple.Value).Operand2.ToString()))
                        throw new Exception(((BinaryOperationExpression)aSimple.Value).Operand2 + " Was not defind!");
                lAssembly.Add("@" + ((BinaryOperationExpression)aSimple.Value).Operand2);
                lAssembly.Add("D=A");
                lAssembly.Add("@OPERAND2");
                lAssembly.Add("M=D");


                lAssembly.Add("@OPERAND1");
                lAssembly.Add("D=M");
                lAssembly.Add("@OPERAND2");
                lAssembly.Add("D=D" + ((BinaryOperationExpression)aSimple.Value).Operator.ToString() + "M");
                
            }
            else if (aSimple.Value is UnaryOperatorExpression) {
                if (!dSymbolTable.ContainsKey(((UnaryOperatorExpression)aSimple.Value).Operand.ToString()))
                    throw new Exception(((UnaryOperatorExpression)aSimple.Value).Operand + " Was not defind!");
                lAssembly.Add("@"+ ((UnaryOperatorExpression)aSimple.Value).Operand);
                lAssembly.Add("D=A");
                lAssembly.Add("D="+((UnaryOperatorExpression)aSimple.Value).Operator+"D");
            }
            else if (aSimple.Value is VariableExpression)
            {
                if (!dSymbolTable.ContainsKey(((VariableExpression)aSimple.Value).Name))
                    throw new Exception(dSymbolTable.ContainsKey(((VariableExpression)aSimple.Value).Name) + " was not defind!");
                lAssembly.Add("@"+ ((VariableExpression)aSimple.Value).Name);
                lAssembly.Add("D=M");
            }
            else {
                throw new Exception(aSimple.Value.GetType() + " is not supported yet!");
            }

            if(!dSymbolTable.ContainsKey(aSimple.Variable))
                throw new Exception(aSimple.Variable + " Was not defind!");
            lAssembly.Add("@"+aSimple.Variable);
            lAssembly.Add("M=D");

            lAssembly.Add("@RESULT");
            lAssembly.Add("M=D");
            lAssembly.Add("@RESULT");
            lAssembly.Add("D=M");
            lAssembly.Add("@ADDRESS");
            lAssembly.Add("A=M");
            lAssembly.Add("M=D");


            
            

            return lAssembly;
        }


        public Dictionary<string, int> ComputeSymbolTable(List<VarDeclaration> lDeclerations)
        {
            Dictionary<string, int> dTable = new Dictionary<string, int>();
            //add here code to comptue a symbol table for the given var declarations
            Queue<VarDeclaration> artificial_vars = new Queue<VarDeclaration>();
            for (int i = 0; i < lDeclerations.Count; i++)
            {
                if (lDeclerations[i].Name[0]=='_') {
                    artificial_vars.Enqueue(lDeclerations[i]);
                }
                else
                    dTable.Add(lDeclerations[i].Name , dTable.Count);
            }
            while(artificial_vars.Count > 0)
            {
                dTable.Add(artificial_vars.Dequeue().Name, dTable.Count);
            }
            //real vars should come before (lower indexes) than artificial vars (starting with _), and their indexes must be by order of appearance.
            //for example, given the declarations:
            //var int x;
            //var int _1;
            //var int y;
            //the resulting table should be x=0,y=1,_1=2
            //throw an exception if a var with the same name is defined more than once
            return dTable;
        }


        public List<string> GenerateCode(List<LetStatement> lSimpleAssignments, List<VarDeclaration> lVars)
        {
            List<string> lAssembly = new List<string>();
            Dictionary<string, int> dSymbolTable = ComputeSymbolTable(lVars);
            foreach (LetStatement aSimple in lSimpleAssignments)
                lAssembly.AddRange(GenerateCode(aSimple, dSymbolTable));
            return lAssembly;
        }

        public List<LetStatement> SimplifyExpressions(LetStatement s, List<VarDeclaration> lVars)
        {
            //add here code to simply expressins in a statement. 
            //add var declarations for artificial variables.
            return null;
        }
        public List<LetStatement> SimplifyExpressions(List<LetStatement> ls, List<VarDeclaration> lVars)
        {
            List<LetStatement> lSimplified = new List<LetStatement>();
            foreach (LetStatement s in ls)
                lSimplified.AddRange(SimplifyExpressions(s, lVars));
            return lSimplified;
        }

   


        public LetStatement ParseStatement(List<Token> lTokens)
        {
            TokensStack sTokens = new TokensStack();
            for (int i = lTokens.Count - 1; i >= 0; i--)
                sTokens.Push(lTokens[i]);
            LetStatement s = new LetStatement();
            s.Parse(sTokens);
            return s;
        }

 
        public List<Token> Tokenize(List<string> lCodeLines)
        {
            List<Token> lTokens = new List<Token>();
            for (int i = 0; i < lCodeLines.Count; i++)
            {
                string sLine = lCodeLines[i];
                List<Token> lLineTokens = Tokenize(sLine, i);
                lTokens.AddRange(lLineTokens);
            }
            return lTokens;
        }


        public List<Token> Tokenize(string sLine, int iLine)
        {

            List<Token> lTokens = new List<Token>();
            //your code here
            char[] delimiter = { ' ', ',', ';', '*', '+', '-', '/', '<', '>', '&', '=', '|', '!', '(', ')', '[', ']', '{', '}', '\t' };
            string sToken;
            int cChars;


            string code_line = sLine;
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
                                Parentheses token = new Parentheses(sToken[0], iLine, position);
                                lTokens.Add(token);
                            }
                            else if (Token.Operators.Contains(sToken[0]))
                            {
                                Operator token = new Operator(sToken[0], iLine, position);
                                lTokens.Add(token);
                            }
                            else if (Token.Separators.Contains(sToken[0]))
                            {
                                Separator token = new Separator(sToken[0], iLine, position);
                                lTokens.Add(token);
                            }
                            else if (Token.Numbers.Contains(sToken[0]))
                            {
                                Number token = new Number(sToken, iLine, position);
                                lTokens.Add(token);
                            }
                            else
                            {
                                if (sToken[0] >= 'a' && sToken[0] <= 'z' || sToken[0] >= 'A' && sToken[0] <= 'Z')
                                {
                                    Identifier token = new Identifier(sToken, iLine, position);
                                    lTokens.Add(token);
                                }
                                else
                                {
                                    Token token = new Token();
                                    token.Line = iLine;
                                    token.Position = position;
                                    throw new SyntaxErrorException("syntaxError", token);
                                }
                            }
                        }
                        else if (sToken.Length > 1)
                        {

                            if (Token.Statements.Contains(sToken))
                            {
                                Statement token = new Statement(sToken, iLine, position);
                                lTokens.Add(token);
                            }
                            else if (Token.VarTypes.Contains(sToken))
                            {
                                VarType token = new VarType(sToken, iLine, position);
                                lTokens.Add(token);
                            }
                            else if (Token.Constants.Contains(sToken))
                            {
                                Constant token = new Constant(sToken, iLine, position);
                                lTokens.Add(token);
                            }
                            else if (isNumber(sToken))
                            {
                                Number token = new Number(sToken, iLine, position);
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
                                            corrupt_token.Line = iLine;
                                            corrupt_token.Position = position;
                                            throw new SyntaxErrorException("syntaxError", corrupt_token);
                                        }

                                    }
                                    Identifier token = new Identifier(sToken, iLine, position);
                                    lTokens.Add(token);
                                }
                                else
                                {
                                    Token token = new Token();
                                    token.Line = iLine;
                                    token.Position = position;
                                    throw new SyntaxErrorException("syntaxError", token);
                                }
                            }
                        }

                    }
                    position = position + sToken.Length;
                

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
