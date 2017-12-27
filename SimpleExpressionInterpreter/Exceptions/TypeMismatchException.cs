using System;

namespace ExpressionInterpreter
{
    public class TypeMismatchException : Exception
    {
        public TypeMismatchException()
        {
        }

        public TypeMismatchException(string message) : base(message)
        {
        }

        public TypeMismatchException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
