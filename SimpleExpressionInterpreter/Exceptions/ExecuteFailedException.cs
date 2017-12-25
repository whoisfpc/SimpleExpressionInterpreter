using System;

namespace ExpressionInterpreter
{
    public class ExecuteFailedException : Exception
    {
        public ExecuteFailedException()
        {
        }

        public ExecuteFailedException(string message) : base(message)
        {
        }

        public ExecuteFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
