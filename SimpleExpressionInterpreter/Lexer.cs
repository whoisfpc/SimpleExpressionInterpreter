using SimpleExpressionInterpreter.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace SimpleExpressionInterpreter
{
    /// <summary>
    /// Lexer, scanner source, generate tokens
    /// </summary>
    public class Lexer : IEnumerable<Token>
    {
        /// <summary>
        /// expression source
        /// </summary>
        private string source;

        public Lexer(string source)
        {
            this.source = source;
        }

        static void Source2Tokens(string source, IList<Token> tokens)
        {
            StringBuilder numBuilder = new StringBuilder();
            char ch;
            for (int i = 0; i < source.Length; i++)
            {
                ch = source[i];
                if ((ch >= '0' && ch <= '9') || ch == '.')
                {
                    numBuilder.Append(ch);
                }
                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '(' || ch == ')')
                {
                    if (numBuilder.Length > 0)
                    {
                        tokens.Add(new Num(numBuilder.ToString()));
                        numBuilder.Clear();
                    }
                    Token tmpToken;
                    switch (ch)
                    {
                        case '+':
                            tmpToken = new Plus();
                            break;
                        case '-':
                            tmpToken = new Minus();
                            break;
                        case '*':
                            tmpToken = new Mul();
                            break;
                        case '/':
                            tmpToken = new Div();
                            break;
                        case '(':
                            tmpToken = new LP();
                            break;
                        case ')':
                            tmpToken = new RP();
                            break;
                        default:
                            tmpToken = new None();
                            break;
                    }
                    tokens.Add(tmpToken);
                }
            }
            if (numBuilder.Length > 0)
            {
                tokens.Add(new Num(numBuilder.ToString()));
                numBuilder.Clear();

            }
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return new TokenEnumerator(source);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TokenEnumerator(source);
        }
    }

    public class TokenEnumerator : IEnumerator<Token>
    {
        public Token Current
        {
            get
            {
                if (source == null || current == null)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    return current;
                }
            }
        }

        object IEnumerator.Current => Current;

        private string source;
        private Token current;
        private StringBuilder numBuilder;
        private int pos;

        public TokenEnumerator(string source)
        {
            this.source = source;
            numBuilder = new StringBuilder();
            pos = 0;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            //TODO: 需要加look ahead，实现正确的输出
            for (int i = pos; i < source.Length; i++)
            {
                char ch = source[i];
                if ((ch >= '0' && ch <= '9') || ch == '.')
                {
                    numBuilder.Append(ch);
                }
                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '(' || ch == ')')
                {
                    if (numBuilder.Length > 0)
                    {
                        this.current = new Num(numBuilder.ToString());
                        numBuilder.Clear();
                        pos = i;
                        return true;
                    }
                    switch (ch)
                    {
                        case '+':
                            current = new Plus();
                            break;
                        case '-':
                            current = new Minus();
                            break;
                        case '*':
                            current = new Mul();
                            break;
                        case '/':
                            current = new Div();
                            break;
                        case '(':
                            current = new LP();
                            break;
                        case ')':
                            current = new RP();
                            break;
                        default:
                            current = new None();
                            break;
                    }
                }
            }
            return false;
        }

        public void Reset()
        {
            current = null;
            numBuilder.Clear();
            pos = 0;
        }
    }
}
