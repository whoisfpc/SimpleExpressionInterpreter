using System;

namespace ExpressionInterpreter
{
    public class LexicalInvalidException : Exception
    {
        public LexicalInvalidException()
        {
        }

        public LexicalInvalidException(string message) : base(message)
        {
        }

        public LexicalInvalidException(string message, Exception inner)
            :base(message, inner)
        {
        }
    }
}
