using SimpleExpressionInterpreter.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
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
        private bool disposed = false;

        public TokenEnumerator(string source)
        {
            this.source = source;
            numBuilder = new StringBuilder();
            pos = 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                source = null;
                current = null;
                numBuilder = null;
            }
            disposed = true;
        }

        ~TokenEnumerator()
        {
            Dispose(false);
        }

        public bool MoveNext()
        {
            char ch;
            char nextCh;
            //TODO: 需要改用正则表达式获取token，以实现更强大的功能
            for (int i = pos; i < source.Length; i++)
            {
                ch = source[i];
                nextCh = i != source.Length - 1 ? source[i + 1] : ' ';
                if ((ch >= '0' && ch <= '9') || ch == '.')
                {
                    numBuilder.Append(ch);
                    // 没有处理'.'多次的情况
                    if ((nextCh < '0' || nextCh > '0') && nextCh != '.')
                    {
                        pos = i + 1;
                        current = new Num(numBuilder.ToString());
                        numBuilder.Clear();
                        return true;
                    }
                }
                else if (ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '(' || ch == ')')
                {
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
                    pos = i + 1;
                    return true;
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
