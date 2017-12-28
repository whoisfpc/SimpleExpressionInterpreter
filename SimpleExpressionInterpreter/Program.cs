using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace ExpressionInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            var compiler = new Compiler();
            var executor = new Executor();
            ConsoleKeyInfo key;
            var variables = new List<float> { 1, 2, 3 };
            Console.WriteLine("predefined variables: 1, 2, 3");
            do
            {
                Console.WriteLine();
                Console.Write("input expression:");
                var source = Console.ReadLine();
                //compiler.PrintAbsyn(source);
                var bytecodes = compiler.Compile(source);
                compiler.PrintBytecode(bytecodes);
                var result = executor.Execute(bytecodes, variables);

                Console.WriteLine("result = " + result.ToString());
                Console.WriteLine("press Q quit, press other key continue...");
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Q);
        }
    }
}
