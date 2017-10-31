using System;

namespace SimpleExpressionInterpreter
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TokenRegexAttribute : Attribute
    {
        public string Pattern { get; }
        public int Priority { get; set; }

        public TokenRegexAttribute(string pattern)
        {
            Pattern = pattern;
        }
    }
}
