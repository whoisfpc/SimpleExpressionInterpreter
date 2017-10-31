using SimpleExpressionInterpreter.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

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
        /// <summary>
        /// 按照优先级排列的词法分析信息
        /// </summary>
        private SortedList<int, LexInfo> sortedLex;

        public Lexer()
        {
            var descendingComparer = Comparer<int>.Create((x, y) => {
                var result = y.CompareTo(x);
                if (result == 0)
                    return 1;
                else
                    return result;
            });
            sortedLex = new SortedList<int, LexInfo>(descendingComparer);
            var assembly = typeof(Lexer).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsDefined(typeof(TokenRegexAttribute), false))
                {
                    var tokenRegex = type.GetCustomAttribute<TokenRegexAttribute>();
                    var regex = new Regex(tokenRegex.Pattern);
                    var ctor = type.GetConstructor(new Type[] { typeof(string) });
                    sortedLex.Add(tokenRegex.Priority, new LexInfo(regex, ctor));
                }
            }
        }

        public Lexer Analyse(string source)
        {
            this.source = source;
            return this;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return new TokenEnumerator(source, sortedLex.Values);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TokenEnumerator(source, sortedLex.Values);
        }

        public class LexInfo
        {
            public Regex Regex { get; }
            public ConstructorInfo CtorInfo { get; }

            public LexInfo(Regex regex, ConstructorInfo ctorInfo)
            {
                Regex = regex;
                CtorInfo = ctorInfo;
            }
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
        private IList<Lexer.LexInfo> lexInfos;
        private Token current;
        private StringBuilder numBuilder;
        private int pos;
        private bool disposed = false;

        public TokenEnumerator(string source, IList<Lexer.LexInfo> lexInfos)
        {
            this.source = source;
            this.lexInfos = lexInfos;
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
                    // dispose resources with IDispose interface
                }
                // dispose resources witout IDispose interface
                source = null;
                lexInfos = null;
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
            if (pos >= source.Length)
            {
                return false;
            }
            foreach (var lexInfo in lexInfos)
            {
                var match = lexInfo.Regex.Match(source, pos);
                if (match.Success)
                {
                    pos = match.Index + match.Length;
                    current = lexInfo.CtorInfo.Invoke(new object[] { match.Value }) as Token;
                    return true;
                }
            }
            return false;
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
                            current = new Plus(ch.ToString());
                            break;
                        case '-':
                            current = new Minus(ch.ToString());
                            break;
                        case '*':
                            current = new Mul(ch.ToString());
                            break;
                        case '/':
                            current = new Div(ch.ToString());
                            break;
                        case '(':
                            current = new LP(ch.ToString());
                            break;
                        case ')':
                            current = new RP(ch.ToString());
                            break;
                        default:
                            current = new None(ch.ToString());
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
