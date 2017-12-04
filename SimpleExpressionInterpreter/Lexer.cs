using ExpressionInterpreter.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ExpressionInterpreter
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
        private IList<LexInfo> lexinfos;

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
                    var ctor = type.GetConstructor(new Type[] { typeof(string), typeof(int) });
                    sortedLex.Add(tokenRegex.Priority, new LexInfo(regex, ctor));
                }
            }
            lexinfos = sortedLex.Values;
        }

        public Lexer Analyse(string source)
        {
            this.source = source;
            return this;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            int pos = 0;
            if (pos >= source.Length)
            {
                yield break;
            }
            while (pos < source.Length)
            {
                var token = FindToken(ref pos);
                if (!token.Ignore)
                {
                    yield return token;
                }
            }
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            int pos = 0;
            if (pos >= source.Length)
            {
                yield break;
            }
            while (pos < source.Length)
            {
                var token = FindToken(ref pos);
                if (!token.Ignore)
                {
                    yield return token;
                }
            }
            yield break;
        }

        private Token FindToken(ref int pos)
        {
            for (int i = 0; i < lexinfos.Count; i++)
            {
                var lexInfo = lexinfos[i];
                var match = lexInfo.Regex.Match(source, pos);
                if (match.Success)
                {
                    pos = match.Index + match.Length;
                    return lexInfo.CtorInfo.Invoke(new object[] { match.Value, match.Index }) as Token;
                }
            }
            return new Error("", pos);
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
}
