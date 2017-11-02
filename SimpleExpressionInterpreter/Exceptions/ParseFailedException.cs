﻿using System;

namespace ExpressionInterpreter
{
    public class ParseFailedException : Exception
    {
        public ParseFailedException()
        {
        }

        public ParseFailedException(string message) : base(message)
        {
        }

        public ParseFailedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
